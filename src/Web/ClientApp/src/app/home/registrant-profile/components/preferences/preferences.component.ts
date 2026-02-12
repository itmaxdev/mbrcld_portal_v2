import { Component, OnInit } from '@angular/core'
import { ActivatedRoute, Router } from '@angular/router'
import { LearningPreferencesFacadeService } from '../../../profile/learning-preferences/learning-preferences-facade.service'

interface IListItem {
  value: number
  label: string
}

const options: IListItem[] = [
  { value: 1, label: $localize`Physical Workshops` },
  { value: 2, label: $localize`One to One (Mentor)` },
  { value: 3, label: $localize`Books` },
  { value: 4, label: $localize`Videos` },
  { value: 5, label: $localize`E-Books` },
  { value: 6, label: $localize`Class` },
  { value: 7, label: $localize`Group` },
  { value: 8, label: $localize`Audiobooks` },
  { value: 9, label: $localize`Articles` },
  { value: 10, label: $localize`Online` },
]

@Component({
  selector: 'app-registrant-preferences',
  templateUrl: './preferences.component.html',
})
export class RegistrantPreferencesComponent implements OnInit {
  learningPreferencesOptions: IListItem[] = options
  learningPreferences: Array<number> = []
  ready = false
  dirty = false
  isSubmitting = false

  constructor(
    private router: Router,
    private activatedRoute: ActivatedRoute,
    private facadeService: LearningPreferencesFacadeService
  ) {}

  async ngOnInit() {
    try {
      const data = await this.facadeService.loadLearningPreferences()
      this.learningPreferences = data.selectedValues || []
    } catch (error) {
      console.error('Error loading learning preferences', error)
    } finally {
      this.ready = true
      this.dirty = false
    }
  }

  onPreferenceChange(value: number, event: any) {
    const checked = event.target.checked
    if (checked) {
      if (!this.learningPreferences.includes(value)) {
        this.learningPreferences.push(value)
      }
    } else {
      this.learningPreferences = this.learningPreferences.filter((v) => v !== value)
    }
    this.dirty = true
  }

  isPreferenceChecked(value: number): boolean {
    return this.learningPreferences.includes(value)
  }

  async onSubmit() {
    if (this.isSubmitting) {
      return
    }

    this.isSubmitting = true
    try {
      if (this.dirty) {
        await this.facadeService.submitLearningPreferences({
          selectedValues: this.learningPreferences,
        })
      }
      this.goToNext()
    } catch (error) {
      console.error('Error submitting learning preferences', error)
    } finally {
      this.isSubmitting = false
    }
  }

  goBack() {
    this.router.navigate(['../achievements'], { relativeTo: this.activatedRoute })
  }

  goToNext() {
    this.router.navigate(['../skills'], { relativeTo: this.activatedRoute })
  }
}
