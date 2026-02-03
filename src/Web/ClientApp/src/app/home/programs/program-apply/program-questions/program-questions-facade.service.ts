import { ProgramApplyService } from '../program-apply.service'
import { Injectable } from '@angular/core'
import { from, Observable } from 'rxjs'
import { shareReplay, switchMap } from 'rxjs/operators'
import {
  EnrollmentsClient,
  ProgramsClient,
  AnswerForUpsertDto,
  ListProgramAnswersByEnrollmentIdViewModel,
  ListProgramQuestionByProgramIdViewModel,
} from 'src/app/shared/api.generated.clients'

@Injectable({
  providedIn: 'root',
})
export class ProgramQuestionsFacade {
  private cachedQuestions: Observable<ListProgramQuestionByProgramIdViewModel[]>
  private cachedAnswers: Observable<ListProgramAnswersByEnrollmentIdViewModel[]>

  constructor(
    private programsClient: ProgramsClient,
    private enrollmentsClient: EnrollmentsClient,
    private shared: ProgramApplyService
  ) {}

  loadQuestions() {
    if (!this.cachedQuestions) {
      this.cachedQuestions = from(this.shared.getProgramId()).pipe(
        switchMap((programId) => this.programsClient.questions(programId)),
        shareReplay()
      )
    }
    return this.cachedQuestions.toPromise()
  }

  loadAnswers() {
    if (!this.cachedAnswers) {
      this.cachedAnswers = from(this.shared.getEnrollmentId()).pipe(
        switchMap((enrollmentId) => this.enrollmentsClient.answersGet(enrollmentId)),
        shareReplay()
      )
    }
    return this.cachedAnswers.toPromise()
  }

  storeAnswers(answers: AnswerForUpsertDto[]) {
    this.cachedAnswers = null
    return from(this.shared.getEnrollmentId())
      .pipe(switchMap((enrollmentId) => this.enrollmentsClient.answersPatch(enrollmentId, answers)))
      .toPromise()
  }
}
