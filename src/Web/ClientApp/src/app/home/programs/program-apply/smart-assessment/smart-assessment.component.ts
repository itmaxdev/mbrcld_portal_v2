import { ProgramApplyService } from '../program-apply.service'
import { Component, OnDestroy, OnInit } from '@angular/core'
import { Subject, timer } from 'rxjs'
import { takeUntil, takeWhile, tap } from 'rxjs/operators'

@Component({
  selector: 'app-smart-assessment',
  templateUrl: 'smart-assessment.component.html',
})
export class SmartAssessmentComponent implements OnInit, OnDestroy {
  pymetricsUrl: string
  isAssessmentCompleted = false
  ready = false
  activeIndex = 4

  private destroy$ = new Subject<boolean>()

  constructor(private shared: ProgramApplyService) {}

  async ngOnInit() {
    await this.getEnrollment()

    this.ready = true

    timer(60000, 60000)
      .pipe(
        takeUntil(this.destroy$),
        takeWhile(() => !this.pymetricsUrl || !this.isAssessmentCompleted),
        tap(() => this.getEnrollment())
      )
      .subscribe()
  }

  ngOnDestroy() {
    this.destroy$.next(true)
  }

  openPymetricsPage() {
    window.open(this.pymetricsUrl, '__mbrcld_pymetrics')
  }

  private async getEnrollment() {
    const enrollment = await this.shared.getEnrollment(true)

    if (enrollment.pymetricsUrl) {
      this.pymetricsUrl = enrollment.pymetricsUrl
    }

    if (enrollment.pymetricsCompleted) {
      this.isAssessmentCompleted = true
    }
  }
}
