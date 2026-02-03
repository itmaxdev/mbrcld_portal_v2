import { ITrainingCourse } from './models'
import { TrainingCoursesFacadeService } from './training-courses-facade.service'
import { Component, OnInit } from '@angular/core'
import { FormControl, FormGroup, Validators } from '@angular/forms'
import { ActivatedRoute, Router } from '@angular/router'
import { ConfirmationService } from 'primeng/api'
import { CountryListService, ICountryListItem } from 'src/app/core/country-list.service'

@Component({
  selector: 'app-training-courses',
  templateUrl: './training-courses.component.html',
  styleUrls: ['./training-courses.component.scss'],
  providers: [ConfirmationService],
})
export class TrainingCoursesComponent implements OnInit {
  trainingCourseForm: FormGroup
  trainingCourses: Array<ITrainingCourse> = []
  isFormSubmitting = false
  ready = false
  editingTrainingCourse: ITrainingCourse = {}
  countries: ICountryListItem[] = []

  private _showAddTrainingCourseDialog = false

  get showAddTrainingCourseDialog() {
    return this._showAddTrainingCourseDialog
  }

  set showAddTrainingCourseDialog(show: boolean) {
    this._showAddTrainingCourseDialog = show
    if (!show) {
      this.editingTrainingCourse = {}
      this.trainingCourseForm.reset()
    }
  }

  constructor(
    private facade: TrainingCoursesFacadeService,
    private countryList: CountryListService,
    private router: Router,
    private activatedRoute: ActivatedRoute,
    private confirmationService: ConfirmationService
  ) {}

  async ngOnInit() {
    this.buildForm()

    await Promise.all([
      this.facade.loadTrainingCourses().then((trainingCourses: Array<ITrainingCourse>) => {
        this.trainingCourses = trainingCourses
      }),
      this.countryList.getCountryList().then((data) => (this.countries = data)),
    ])

    this.ready = true
  }

  openAddTrainingCourseDialog() {
    this.showAddTrainingCourseDialog = true
  }

  openTrainingCoursesDialogForEditing(trainingCourse: ITrainingCourse) {
    this.editingTrainingCourse = trainingCourse
    this.trainingCourseForm.patchValue(trainingCourse)
    this.showAddTrainingCourseDialog = true
  }

  async updateTrainingCourse() {
    this.isFormSubmitting = true
    const formValues = this.trainingCourseForm.value

    await this.facade.updateTrainingCourse(this.editingTrainingCourse.id, formValues)

    Object.assign(this.editingTrainingCourse, formValues)
    this.showAddTrainingCourseDialog = false
    this.isFormSubmitting = false
  }

  async addTrainingCourse(): Promise<void> {
    if (!this.trainingCourseForm.valid || this.isFormSubmitting) {
      return
    }

    this.isFormSubmitting = true
    try {
      const formValues = this.trainingCourseForm.value as FormModel
      const id = await this.facade.addTrainingCourse(formValues)
      this.trainingCourses.push({ id, ...formValues })
      this.showAddTrainingCourseDialog = false
    } finally {
      this.isFormSubmitting = false
    }
  }

  async removeTrainingCourse(id: string): Promise<void> {
    this.confirmationService.confirm({
      message: $localize`Are you sure?`,
      accept: async () => {
        await this.facade.removeTrainingCourse(id)
        this.trainingCourses = this.trainingCourses.filter((trainingCourse: ITrainingCourse) => {
          return trainingCourse.id !== id
        })
      },
    })
  }

  getCountryName(countryCode: string): string {
    countryCode = countryCode?.toLowerCase()
    const country = this.countries.find((x) => x.value.toLowerCase() === countryCode)
    return country?.label
  }

  goToNextForm(): void {
    this.router.navigate(['../memberships'], { relativeTo: this.activatedRoute })
  }

  private buildForm(): void {
    this.trainingCourseForm = new FormGroup({
      name: new FormControl('', [Validators.required]),
      provider: new FormControl('', [Validators.required]),
      country: new FormControl('', [Validators.required]),
      graduationDate: new FormControl('', [Validators.required]),
    } as FormModel)
  }
}

type FormModel = { [key in keyof ITrainingCourse]: any }
