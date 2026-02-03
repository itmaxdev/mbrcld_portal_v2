import { ProgramQuestionsFacade } from './program-questions-facade.service'
import { Component, OnInit } from '@angular/core'
import { FormControl, FormGroup, Validators } from '@angular/forms'
import { ActivatedRoute, Router } from '@angular/router'
import { AnswerForUpsertDto } from 'src/app/shared/api.generated.clients'
import { MessageService } from 'primeng/api'

@Component({
  selector: 'app-program-questions',
  templateUrl: 'program-questions.component.html',
  providers: [MessageService],
})
export class ProgramQuestionsComponent implements OnInit {
  activeIndex = 2
  questionsForm: FormGroup
  questions = []
  isSubmitting = false
  ready = false

  constructor(
    private facade: ProgramQuestionsFacade,
    private messageService: MessageService,
    private router: Router,
    private activatedRoute: ActivatedRoute
  ) {}

  async ngOnInit() {
    const [questions, answers] = await Promise.all([
      this.facade.loadQuestions(),
      this.facade.loadAnswers(),
    ])

    this.questions = questions

    this.buildForm(answers)
    this.ready = true
  }

  async save(showToast?: boolean) {
    if (this.isSubmitting) {
      return
    }

    if (this.questionsForm.dirty) {
      this.isSubmitting = true

      try {
        const formValue = this.getDirtyFormValues()
        const answers: AnswerForUpsertDto[] = []

        Object.keys(formValue).forEach((key) => {
          answers.push(
            new AnswerForUpsertDto({
              questionId: key,
              answerText: formValue[key],
            })
          )
        })

        await this.facade.storeAnswers(answers)

        this.questionsForm.markAsPristine()

        if (showToast) {
          this.messageService.add({
            summary: $localize`Form saved successfully`,
            severity: 'success',
            life: 5000,
            closable: true,
          })
        }
      } finally {
        this.isSubmitting = false
      }
    }
  }

  async saveAndGoToNextForm() {
    if (this.questionsForm.invalid || this.isSubmitting) {
      return
    }

    await this.save()

    this.router.navigate(['../smart-assessment'], { relativeTo: this.activatedRoute })
    // this.router.navigate(['../smart-assessment'], { relativeTo: this.activatedRoute })
  }

  private getDirtyFormValues() {
    const formControls = this.questionsForm.controls

    return Object.keys(formControls)
      .filter((key) => formControls[key]?.dirty)
      .reduce((agg, key) => {
        agg[key] = formControls[key].value
        return agg
      }, {} as any)
  }

  private buildForm(answers: any[]) {
    this.questionsForm = new FormGroup({})
    this.questions.forEach((x) => {
      const answer = answers.find((a) => a.questionId === x.id)?.answerText ?? ''
      this.questionsForm.addControl(x.id, new FormControl(answer, [Validators.required]))
    })
  }
}
