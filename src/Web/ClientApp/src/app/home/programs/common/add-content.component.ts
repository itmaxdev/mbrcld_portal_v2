import { tap } from 'rxjs/operators'
import { Location } from '@angular/common'
import { ActivatedRoute } from '@angular/router'
import { FormControl, FormGroup, Validators } from '@angular/forms'
import { Component, OnInit, ChangeDetectorRef } from '@angular/core'
import { ContentsClient } from 'src/app/shared/api.generated.clients'
import { SectionDataService } from 'src/app/shared/services/section-data.service'

@Component({
  selector: 'app-add-content',
  template: `
    <div>
      <div class="p-8 flex justify-between items-center border border-gray-400 rounded-t-lg">
        <button pButton type="button" label="Go Back" (click)="goBack()" i18n-label></button>
        <p class="text-2xl">{{ contentHeader }}</p>
        <button
          pButton
          [disabled]="contentForm.valid ? false : true"
          type="button"
          label="Save"
          i18n-label
          (click)="sendContentData()"
        ></button>
      </div>
      <div class="m-0 p-8 border border-t-0 border-gray-400 rounded-b-lg">
        <form [formGroup]="contentForm" class="p-fluid p-formgrid p-grid profile-form gap-6">
          <div class="w-full">
            <div class="p-field">
              <label for="title" i18n>Title <span class="text-red-500">*</span></label>
              <input
                [(ngModel)]="contentTitle"
                pInputText
                formControlName="title"
                id="enterTitle"
                type="text"
                class="text-lg"
                placeholder="Enter Title"
                i18n-placeholder
              />
              <div *ngIf="contentForm.get('title').invalid && contentForm.get('title').touched">
                <small
                  *ngIf="contentForm.get('title').errors.required"
                  controlName="enterTitle"
                  class="p-invalid text-red-600"
                  i18n
                  >This field is required</small
                >
              </div>
            </div>
          </div>
          <div class="w-full">
            <div class="p-field">
              <label for="title" i18n>Order <span class="text-red-500">*</span></label>
              <p-inputNumber
                [(ngModel)]="order"
                pInputText
                formControlName="order"
                id="order"
                type="text"
                class="text-lg"
                placeholder="Enter Order"
                mode="decimal"
                inputId="withoutgrouping"
                [useGrouping]="false"
                i18n-placeholder
              >
              </p-inputNumber>
              <div *ngIf="contentForm.get('order').invalid && contentForm.get('order').touched">
                <small
                  *ngIf="contentForm.get('order').errors.required"
                  controlName="order"
                  class="p-invalid text-red-600"
                  i18n
                  >This field is required</small
                >
              </div>
            </div>
          </div>
          <div class="w-full">
            <p-editor
              [(ngModel)]="text"
              name="text"
              formControlName="text"
              [style]="{ height: '230px' }"
            ></p-editor>
            <div *ngIf="contentForm.get('text').invalid && contentForm.get('text').touched">
              <small
                *ngIf="contentForm.get('text').errors.required"
                controlName="enterTitle"
                class="p-invalid text-red-600"
                i18n
                >This field is required</small
              >
            </div>
          </div>
        </form>
      </div>
    </div>
  `,
})
export class AddContentComponent implements OnInit {
  contentData: any
  contentForm: FormGroup
  text = ''
  order = ''
  sectionId: string
  contentHeader = 'Add Content'
  contentId: string = null
  contentTitle = ''

  constructor(
    private _location: Location,
    private route: ActivatedRoute,
    private cdRef: ChangeDetectorRef,
    private contents: ContentsClient,
    public sectionData: SectionDataService
  ) {}

  async sendContentData() {
    this.contentId = this.route.snapshot.paramMap.get('contentId')
      ? this.route.snapshot.paramMap.get('contentId')
      : undefined

    if (this.contentForm.valid) {
      await Promise.all([
        this.contents
          .contentsPost(
            undefined,
            this.sectionId,
            this.contentId,
            this.contentForm.value.title,
            0,
            this.contentForm.value.order,
            1,
            this.contentForm.value.text,
            undefined,
            new Date()
          )
          .pipe(
            tap(() => {
              this.sectionData.redirectBack(2)
            })
          )
          .toPromise(),
      ])
    }
  }

  goBack() {
    this._location.back()
  }

  ngAfterViewChecked() {
    this.cdRef.detectChanges()
  }

  async ngOnInit() {
    this.sectionId = this.route.snapshot.paramMap.get('sectionId')
    this.contentId = this.route.snapshot.paramMap.get('contentId')

    this.contentForm = new FormGroup({
      title: new FormControl(null, [Validators.required]),
      order: new FormControl(null, [Validators.required]),
      text: new FormControl(null, [Validators.required]),
    })

    if (this.contentId) {
      this.contentHeader = 'Edit Content'
      this.contentId = undefined

      if (history.state.data) {
        this.contentId = history.state.data.contentId
      }

      if (this.contentId) {
        await Promise.all([
          this.contents.sectionContents(history.state.data.sectionId).subscribe((data) => {
            this.contentData = data.filter((item) => item.id == this.contentId)
            this.text = this.contentData[0].text
            this.contentTitle = this.contentData[0].name
            this.order = this.contentData[0].order
          }),
        ])
      } else {
        this._location.back()
      }
    }
  }
}
