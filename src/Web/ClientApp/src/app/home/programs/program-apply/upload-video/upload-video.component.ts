import { ProgramApplyService } from '../program-apply.service'
import { Component, OnDestroy, OnInit } from '@angular/core'
import { Subject, timer } from 'rxjs'
import { takeUntil, takeWhile, tap } from 'rxjs/operators'

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
  status = 1
  isVideoUploaded = false
  isDestroyed = false
  private destroy$ = new Subject<boolean>()

  constructor(private shared: ProgramApplyService) {}

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
