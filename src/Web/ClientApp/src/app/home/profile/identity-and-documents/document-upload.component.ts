import { ProfileFacade } from '../common/profile-facade.service'
import { Component, Input, OnInit } from '@angular/core'
import { ConfirmationService } from 'primeng/api'

@Component({
  selector: 'app-document-upload',
  template: `
    <p-fileUpload
      appPrimeNGi18n
      name="file"
      [accept]="accept"
      [maxFileSize]="maxFileSize"
      [url]="postUrl"
      [auto]="true"
      (onUpload)="handleOnUpload($event)"
    >
      <ng-template pTemplate="content">
        <ng-container *ngIf="uploadedDocument as file">
          <div class="flex justify-between items-center">
            <div class="flex-shrink flex-grow-0 overflow-hidden max-w-sm">
              <div class="truncate" [title]="file.fileName">
                {{ file.fileName }}
              </div>
            </div>
            <button
              title="Delete"
              i18n-title
              pButton
              type="button"
              icon="pi pi-trash"
              class="p-button-danger"
              [style]="{ width: '2.5rem' }"
              (click)="handleOnRemove()"
            ></button>
          </div>
        </ng-container>
      </ng-template>
    </p-fileUpload>
    <p-confirmDialog
      appPrimeNGi18n
      header="Confirmation"
      i18n-header
      icon="pi pi-exclamation-triangle"
      styleClass="w-full max-w-sm"
    >
    </p-confirmDialog>
  `,
  providers: [ConfirmationService],
})
export class DocumentUploadComponent implements OnInit {
  @Input() identifier = ''

  @Input() maxFileSize = 2097152

  // @Input() accept = 'image/*'
  @Input() accept = '.bmp,.png,.jpg,.jpeg,.pdf,.docx'

  uploadedDocument: Document

  constructor(
    private profileFacade: ProfileFacade,
    private confirmationService: ConfirmationService
  ) {}

  async ngOnInit() {
    if (!this.identifier) {
      return
    }

    const documents = await this.profileFacade.loadUserDocuments()
    if (documents) {
      this.uploadedDocument = documents.find((e) => e.identifier === this.identifier) as Document
    }
  }

  get postUrl(): string {
    return `/api/profile/documents/${encodeURIComponent(this.identifier)}`
  }

  handleOnUpload(event: any) {
    const [file] = event.files as File[]
    this.uploadedDocument = { fileName: file.name }
    this.profileFacade.loadFormProgress()
  }

  async handleOnRemove() {
    this.confirmationService.confirm({
      message: $localize`Are you sure?`,
      accept: async () => {
        await this.profileFacade.removeUserDocument(this.identifier)
        this.uploadedDocument = null
      },
    })
  }
}

type Document = { fileName: string }
