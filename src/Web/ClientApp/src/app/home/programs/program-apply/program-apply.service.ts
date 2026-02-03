import { Injectable } from '@angular/core'
import { from, Observable } from 'rxjs'
import { shareReplay, switchMap } from 'rxjs/operators'
import {
  EnrollmentsClient,
  IGetEnrollmentByIdViewModel,
  IGetEnrollmentStatusByIdViewModel,
} from 'src/app/shared/api.generated.clients'
import { SectionDataService } from 'src/app/shared/services/section-data.service'

@Injectable({ providedIn: 'root' })
export class ProgramApplyService {
  private cachedEnrollment$: Observable<IGetEnrollmentByIdViewModel>

  constructor(private enrollmentsClient: EnrollmentsClient, private section: SectionDataService) {}

  setProgramId(programId: string) {
    localStorage.setItem('programId', programId)
  }

  getProgramId(): Promise<string> {
    return new Promise((resolve) => resolve(localStorage.getItem('programId')))
  }

  goBack() {
    this.section.redirectBack(3)
  }

  setEnrollmentId(enrollmentId: string) {
    localStorage.setItem('enrollmentId', enrollmentId)
  }

  getEnrollmentId(): Promise<string> {
    return new Promise((resolve) => resolve(localStorage.getItem('enrollmentId')))
  }

  getEnrollment(ignoreCache?: boolean): Promise<IGetEnrollmentByIdViewModel> {
    if (ignoreCache || !this.cachedEnrollment$) {
      this.cachedEnrollment$ = from(this.getEnrollmentId()).pipe(
        switchMap((enrollmentId) => this.enrollmentsClient.enrollmentsGetById(enrollmentId)),
        shareReplay()
      )
    }
    return this.cachedEnrollment$.toPromise()
  }

  async getVideoUploadStatus() {
    const enrollmentId = await this.getEnrollmentId()
    if (enrollmentId) {
      const resp = await this.enrollmentsClient.getVideoUploadStatus(enrollmentId)
      return resp
    }
  }

  completeEnrollment(): Promise<void> {
    this.cachedEnrollment$ = null
    return this.getEnrollmentId().then((id) => this.enrollmentsClient.complete(id).toPromise())
  }

  getEnrollmentStatus(): Promise<IGetEnrollmentStatusByIdViewModel> {
    return from(this.getEnrollmentId())
      .pipe(
        switchMap((enrollmentId) => this.enrollmentsClient.enrollmentStatus(enrollmentId)),
        shareReplay()
      )
      .toPromise()
  }
}
