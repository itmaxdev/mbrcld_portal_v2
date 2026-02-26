import { ProgramApplyService } from '../program-apply.service'
import { Component, OnDestroy, OnInit } from '@angular/core'
import { Subject, timer } from 'rxjs'
import { HttpClient } from '@angular/common/http'
import { EnrollmentsClient } from 'src/app/shared/api.generated.clients'
import { SectionDataService } from 'src/app/shared/services/section-data.service'

@Component({
  selector: 'app-upload-video',
  templateUrl: 'upload-video.component.html',
})
/*
  statuses:
  0: idle
  1: busy
  2: error
*/
export class UploadVideoComponent implements OnInit, OnDestroy {
  status = 0
  isVideoUploaded = false
  isDestroyed = false
  private destroy$ = new Subject<boolean>()

  constructor(
    private shared: ProgramApplyService,
    private http: HttpClient,
    private enrollmentsClient: EnrollmentsClient,
    private section: SectionDataService
  ) {}

  async ngOnInit() {
    await this.checkIfUploaded()
  }

  ngOnDestroy() {
    this.isDestroyed = true
    this.destroy$.next(true)
  }

  async onBeforeUpload(e) {
    const enrollmentId = await this.shared.getEnrollmentId()
    if (enrollmentId) {
      e.formData.append('enrollmentId', enrollmentId)
    }
    this.status = 1
  }

  async onUploadDone(e) {
    this.checkIfUploaded()
  }

  async onUploadError(e, fileUpload) {
    this.status = 2
  }

  async onFileSelected(event: any) {
    const file: File = event.target.files[0]
    if (!file) return

    this.status = 1

    try {
      const enrollmentId = await this.shared.getEnrollmentId()

      const formData = new FormData()
      formData.append('file', file)

      if (enrollmentId) {
        formData.append('enrollmentId', enrollmentId)
      }

      await this.videoUpload(formData)

      await this.checkIfUploaded()
    } catch (err) {
      this.status = 2
    }
  }

  async videoUpload(formData: FormData): Promise<any> {
    return this.http.post('/api/programs/upload-video', formData).toPromise()
  }

  private async checkIfUploaded() {
    const resp = await this.shared.getVideoUploadStatus()
    if (resp === null) {
      setTimeout(() => {
        if (this.isDestroyed) return
        this.checkIfUploaded()
      }, 2000)
      return
    }
    this.isVideoUploaded = resp
    this.status = 0
  }
}
