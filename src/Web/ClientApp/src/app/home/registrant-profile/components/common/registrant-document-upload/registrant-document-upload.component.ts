import { Component, Input, OnInit, ViewChild, ElementRef } from '@angular/core'
import { ProfileFacade } from '../../../../profile/common/profile-facade.service'
import { UserDocumentsClient } from 'src/app/shared/api.generated.clients'
import { finalize } from 'rxjs/operators'

@Component({
  selector: 'app-registrant-document-upload',
  templateUrl: './registrant-document-upload.component.html',
  styleUrls: ['./registrant-document-upload.component.scss'],
})
export class RegistrantDocumentUploadComponent implements OnInit {
  @Input() identifier = ''
  @Input() label = ''
  @Input() required = false
  @Input() accept = '.bmp,.png,.jpg,.jpeg,.pdf,.docx'

  @ViewChild('fileInput') fileInput: ElementRef<HTMLInputElement>

  uploadedDocument: any = null
  isUploading = false
  uploadProgress = 0

  constructor(private facade: ProfileFacade, private documentClient: UserDocumentsClient) {}

  async ngOnInit() {
    if (!this.identifier) return

    // Load existing documents
    const documents = await this.facade.loadUserDocuments()
    if (documents) {
      this.uploadedDocument = documents.find((d) => d.identifier === this.identifier)
    }
  }

  onFileSelected(event: any) {
    const files = event.target.files as FileList
    if (files.length > 0) {
      this.uploadFile(files[0])
    }
  }

  async uploadFile(file: File) {
    this.isUploading = true
    this.uploadProgress = 0

    const fileParameter = {
      data: file,
      fileName: file.name,
    }

    // Note: The generated client doesn't seem to support progress reporting out of the box easily
    // without custom interceptors or using lower level HttpClient calls.
    // For now, we'll simulate progress or just show a spinner.
    // Actually, primeng's FileUpload does it, but we are building a custom one.

    this.documentClient
      .documentsPost(this.identifier, fileParameter)
      .pipe(
        finalize(() => {
          this.isUploading = false
        })
      )
      .subscribe({
        next: () => {
          this.uploadedDocument = {
            fileName: file.name,
            identifier: this.identifier,
          }
          this.facade.reloadUserDocuments()
          this.facade.loadFormProgress()
        },
        error: (err) => {
          console.error('Upload failed', err)
        },
      })
  }

  async removeFile() {
    if (!window.confirm($localize`Are you sure you want to delete this document?`)) return

    try {
      await this.facade.removeUserDocument(this.identifier)
      await this.facade.reloadUserDocuments()
      this.uploadedDocument = null
      if (this.fileInput) {
        this.fileInput.nativeElement.value = ''
      }
    } catch (err) {
      console.error('Delete failed', err)
    }
  }

  triggerUpload() {
    if (this.fileInput) {
      this.fileInput.nativeElement.click()
    }
  }
}
