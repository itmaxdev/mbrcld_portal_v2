import { Component, OnInit } from '@angular/core'
import { FormControl, FormGroup, Validators } from '@angular/forms'
import { ActivatedRoute, Router } from '@angular/router'
import { ConfirmationService } from 'primeng/api'
import { CountryListService, ICountryListItem } from 'src/app/core/country-list.service'
import { TrainingCoursesFacadeService } from '../../../profile/training-courses/training-courses-facade.service'
import { ITrainingCourse } from '../../../profile/training-courses/models'

@Component({
  selector: 'app-registrant-training-courses',
  templateUrl: './training-courses.component.html',
  styleUrls: ['./training-courses.component.scss'],
  providers: [ConfirmationService],
})
export class RegistrantTrainingCoursesComponent implements OnInit {
  trainingCourseForm: FormGroup
  trainingCourses: ITrainingCourse[] = []
  editingTrainingCourse: ITrainingCourse = {}
  showTrainingDialog = false
  isFormSubmitting = false
  ready = false
  countries: ICountryListItem[] = []

  constructor(
    private router: Router,
    private activatedRoute: ActivatedRoute,
    private facade: TrainingCoursesFacadeService,
    private countryList: CountryListService,
    private confirmationService: ConfirmationService
  ) {}

  async ngOnInit() {
    this.buildForm()
    await this.fetchContent()
    this.ready = true
  }

  async fetchContent() {
    try {
      const [courses, countries] = await Promise.all([
        this.facade.loadTrainingCourses(),
        this.countryList.getCountryList(),
      ])
      this.trainingCourses = courses
      this.countries = countries
    } catch (error) {
      console.error('Error fetching training courses content:', error)
    }
  }

  private buildForm(): void {
    this.trainingCourseForm = new FormGroup({
      name: new FormControl('', [Validators.required]),
      provider: new FormControl('', [Validators.required]),
      country: new FormControl('', [Validators.required]),
      graduationDate: new FormControl('', [Validators.required]),
    })
  }

  openAddDialog() {
    this.editingTrainingCourse = {}
    this.trainingCourseForm.reset()
    this.showTrainingDialog = true
  }

  openEditDialog(course: ITrainingCourse) {
    this.editingTrainingCourse = course
    const patchValue = { ...course }
    if (course.graduationDate) {
      patchValue.graduationDate = this.formatDate(course.graduationDate) as any
    }
    this.trainingCourseForm.patchValue(patchValue)
    this.showTrainingDialog = true
  }

  private formatDate(date: any): string {
    const d = new Date(date)
    return d.toISOString().split('T')[0]
  }

  async saveTrainingCourse() {
    if (this.trainingCourseForm.invalid || this.isFormSubmitting) {
      this.trainingCourseForm.markAllAsTouched()
      return
    }

    this.isFormSubmitting = true
    try {
      const formValue = { ...this.trainingCourseForm.value }
      if (formValue.graduationDate) {
        formValue.graduationDate = new Date(formValue.graduationDate)
      }

      if (this.editingTrainingCourse.id) {
        await this.facade.updateTrainingCourse(this.editingTrainingCourse.id, formValue)
      } else {
        await this.facade.addTrainingCourse(formValue)
      }

      await this.fetchContent()
      this.showTrainingDialog = false
    } catch (error) {
      console.error('Error saving training course:', error)
    } finally {
      this.isFormSubmitting = false
    }
  }

  async removeTrainingCourse(course: ITrainingCourse) {
    this.confirmationService.confirm({
      message: $localize`Are you sure you want to delete this training course record?`,
      accept: async () => {
        await this.facade.removeTrainingCourse(course.id)
        await this.fetchContent()
      },
    })
  }

  getCountryName(countryCode: string): string {
    countryCode = countryCode?.toLowerCase()
    const country = this.countries.find((x) => x.value.toLowerCase() === countryCode)
    return country?.label || countryCode
  }

  onSubmit() {
    this.router.navigate(['../achievements'], { relativeTo: this.activatedRoute })
  }

  goBack() {
    this.router.navigate(['../education'], { relativeTo: this.activatedRoute })
  }
}
