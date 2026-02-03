import { expand, map, tap } from 'rxjs/operators'
import { Location } from '@angular/common'
import { ActivatedRoute } from '@angular/router'
import { FileUploadValidators } from '@iplab/ngx-file-upload'
import { FormControl, FormGroup, Validators } from '@angular/forms'
import { ChangeDetectorRef, Component, OnInit, ViewEncapsulation } from '@angular/core'
import { ContentsClient } from 'src/app/shared/api.generated.clients'
import { SectionDataService } from 'src/app/shared/services/section-data.service'
import { VimeoUploadService } from 'src/app/shared/services/vimeo-upload.service'
import { EMPTY } from 'rxjs'

export class uploadFiles {
  constructor(public video: File, public path: string, public uploadURI: string) {
    this.video = video
    this.path = path
    this.uploadURI = uploadURI
  }
}

@Component({
  selector: 'app-add-video',
  template: `
    <div>
      <div class="p-8 flex justify-between items-center border border-gray-400 rounded-t-lg">
        <button pButton type="button" label="Go Back" (click)="goBack()" i18n-label></button>
        <p class="text-2xl">{{ videoHeader }}</p>
        <div class="flex items-center">
          <button
            pButton
            [disabled]="videoForm.valid && !invalidFormat ? false : true"
            type="button"
            label="Save"
            (click)="sendVideoData()"
            i18n-label
          ></button>
          <div *ngIf="ready" class="loading">
            <app-progress-spinner></app-progress-spinner>
          </div>
        </div>
      </div>
      <div class="m-0 p-8 border border-t-0 border-gray-400 rounded-b-lg">
        <form [formGroup]="videoForm" class="p-fluid p-formgrid p-grid profile-form gap-6">
          <div class="w-full">
            <div class="p-field">
              <label for="title" i18n>Video Title <span class="text-red-500">*</span></label>
              <input
                [(ngModel)]="title"
                pInputText
                formControlName="title"
                id="title"
                type="text"
                class="text-lg"
                placeholder="Enter Video Title"
                i18n-placeholder
              />
              <div *ngIf="videoForm.get('title').invalid && videoForm.get('title').touched">
                <small
                  *ngIf="videoForm.get('title').errors.required"
                  controlName="enterTitle"
                  class="p-invalid"
                  i18n
                  >This field is required</small
                >
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
                <div *ngIf="videoForm.get('order').invalid && videoForm.get('order').touched">
                  <small
                    *ngIf="videoForm.get('order').errors.required"
                    controlName="order"
                    class="p-invalid text-red-600"
                    i18n
                    >This field is required</small
                  >
                </div>
              </div>
            </div>
            <div class="p-field">
              <label for="videoUrl" i18n>Upload Video</label>
              <input
                [(ngModel)]="videoUrl"
                (ngModelChange)="checkInputValue($event)"
                pInputText
                formControlName="videoUrl"
                id="uploadVideo"
                type="text"
                class="text-lg"
                placeholder="Please Upload a video"
                i18n-placeholder
              />
            </div>
          </div>
          <div class="w-full">
            <p class="text-center text-gray-600 font-semibold">or</p>
          </div>
          <div class="w-full">
            <file-upload
              [(ngModel)]="uploadedVideo"
              (change)="checkUploadedFile()"
              formControlName="uploadedVideo"
              [multiple]="false"
              name="video"
            ></file-upload>
            <div *ngIf="upload.isVideoUploaded">
              <p-progressBar mode="indeterminate" [style]="{ height: '6px' }"></p-progressBar>
            </div>
            <div>
              <small *ngIf="invalidFormat" class="text-red-600 p-2">Wrong file format</small>
            </div>
          </div>
        </form>
      </div>
    </div>
  `,
  styles: [
    `
      .loading .p-progress-spinner {
        width: 25px !important;
      }

      .loading div {
        height: 25px !important;
        padding: 0 !important;
      }
    `,
  ],
  encapsulation: ViewEncapsulation.None,
})
export class AddVideoComponent implements OnInit {
  videoData: any
  sectionId: string
  title = ''
  order = ''
  videoUrl = ''
  uploadedVideo: File[]
  contentId: string
  videoId: string = null
  videoHeader = 'Add Video'
  invalidFormat = false
  ready = false
  vimeoVideoUrl: string

  private filesControl = new FormControl(null, FileUploadValidators.filesLimit(1))

  public videoForm = new FormGroup({
    files: this.filesControl,
  })

  //asdasd
  videoList: FileList
  pendingFiles: uploadFiles[] = []

  constructor(
    private _location: Location,
    private route: ActivatedRoute,
    private cdRef: ChangeDetectorRef,
    private contents: ContentsClient,
    public sectionData: SectionDataService,
    public upload: VimeoUploadService
  ) {}

  uploadVimeoVideo(files: FileList): void {
    const file: File = files[0]
    const isAccepted = this.checkAllowedType(file.type)
    if (isAccepted) {
      this.videoList = files
      const recursion = this.getLink(this.videoList[0], 0, this.videoList).pipe(
        expand((res) => {
          return res.index > res.arr.length - 1
            ? EMPTY
            : this.getLink(this.videoList[res.index], res.index, this.videoList)
        })
      )

      recursion.subscribe((x) => {
        if (x.index > x.arr.length - 1) {
          this.videoUpload()
        }
      })
    }
  }

  getLink = (video: File, index, arr) => {
    return this.upload.createVideo(video).pipe(
      map((response) => {
        const videoFile = new uploadFiles(
          video,
          response.body.link,
          response.body.upload.upload_link
        )
        this.pendingFiles.push(videoFile)
        return {
          data: response,
          index: index + 1,
          arr: arr,
        }
      })
    )
  }

  videoUpload() {
    const success = () => {}

    const upload: Array<any> = []

    this.videoUrl = this.pendingFiles[0].path

    for (let i = 0; i < this.pendingFiles.length; i++) {
      upload.push(
        this.upload.tusUpload(this.pendingFiles[i], i, this.pendingFiles, upload, success)
      )
    }
    upload[0].start()
  }

  checkAllowedType(filetype: string): boolean {
    const allowed = ['mov', 'wmv', 'avi', 'flv', 'mp4']
    const videoType = filetype.split('/').pop()
    return allowed.includes(videoType)
  }

  checkUploadedFile() {
    this.invalidFormat = !this.videoForm.value.uploadedVideo[0].type.includes('video/')
    this.videoForm.get('videoUrl').clearValidators()
    this.videoForm.get('videoUrl').updateValueAndValidity()

    if (!this.invalidFormat) {
      const uploadedFile = this.videoForm.value.uploadedVideo
      //upload Vimeo
      this.uploadVimeoVideo(uploadedFile)
    }
  }

  checkInputValue(inputValue) {
    if (inputValue == '') {
      this.invalidFormat = false
      this.videoForm.get('uploadedVideo').setValidators([Validators.required])
      this.videoForm.get('uploadedVideo').updateValueAndValidity()
      this.videoForm.get('videoUrl').clearValidators()
      this.videoForm.get('videoUrl').updateValueAndValidity()
    } else {
      this.videoForm.get('videoUrl').setValidators([Validators.required])
      this.videoForm.get('videoUrl').updateValueAndValidity()
      this.videoForm.get('uploadedVideo').clearValidators()
      this.videoForm.get('uploadedVideo').updateValueAndValidity()
    }
  }

  sendVideoData() {
    this.ready = true
    if (this.videoForm.valid && !this.invalidFormat && this.videoUrl !== '') {
      this.contents
        .contentsPost(
          null,
          this.sectionId,
          this.videoId,
          this.videoForm.value.title,
          0,
          this.videoForm.value.order,
          2,
          undefined,
          this.videoUrl,
          new Date()
        )
        .pipe(
          tap(() => {
            this.ready = false
            this.goBack()
          })
        )
        .toPromise()
    }

    this.videoId = this.route.snapshot.paramMap.get('videoId')
      ? this.route.snapshot.paramMap.get('videoId')
      : undefined
  }

  goBack() {
    this._location.back()
  }

  ngAfterViewChecked() {
    this.cdRef.detectChanges()
  }

  async ngOnInit() {
    this.sectionId = this.route.snapshot.paramMap.get('sectionId')
    this.videoId = this.route.snapshot.paramMap.get('videoId')
      ? this.route.snapshot.paramMap.get('videoId')
      : undefined

    this.videoForm = new FormGroup({
      title: new FormControl(null, [Validators.required]),
      order: new FormControl(null, [Validators.required]),
      videoUrl: new FormControl(null, [Validators.nullValidator]),
      uploadedVideo: new FormControl(null, [Validators.required]),
    })

    if (this.videoId) {
      this.videoHeader = 'Edit Video'
      this.videoId = undefined

      if (history.state.data) {
        this.videoId = history.state.data.contentId
      }

      if (this.videoId) {
        await Promise.all([
          this.contents.sectionContents(history.state.data.sectionId).subscribe((data) => {
            this.videoData = data.filter((item) => item.id == this.videoId)
            this.title = this.videoData[0].name
            this.order = this.videoData[0].order
            this.videoUrl = this.videoData[0].url
          }),
        ])
      } else {
        this._location.back()
      }
    }
  }
}
