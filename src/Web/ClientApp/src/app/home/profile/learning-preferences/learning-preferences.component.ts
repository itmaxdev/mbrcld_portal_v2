import { LearningPreferencesFacadeService } from './learning-preferences-facade.service'
import { Component, OnInit } from '@angular/core'
import { ActivatedRoute, Router } from '@angular/router'

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
  selector: 'app-learning-preferences',
  templateUrl: './learning-preferences.component.html',
  styleUrls: ['./learning-preferences.component.scss'],
})
export class LearningPreferencesComponent implements OnInit {
  learningPreferencesOptions: IListItem[] = options
  learningPreferences: Array<number>
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
      await Promise.all([
        this.facadeService
          .loadLearningPreferences()
          .then((data) => (this.learningPreferences = data.selectedValues)),
      ])
    } finally {
      this.ready = true
      this.dirty = false
    }
  }

  async submitLearningPreferences() {
    if (this.isSubmitting) {
      return
    }

    if (this.dirty) {
      this.isSubmitting = true

      try {
        await this.facadeService.submitLearningPreferences({
          selectedValues: this.learningPreferences,
        })
      } finally {
        this.isSubmitting = false
      }
    }

    this.goToNextForm()
  }

  goToNextForm(): void {
    this.router.navigate(['../languages'], { relativeTo: this.activatedRoute })
  }
}
