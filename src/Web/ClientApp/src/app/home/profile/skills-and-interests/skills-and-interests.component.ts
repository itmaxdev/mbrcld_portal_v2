import { Component, OnInit } from '@angular/core'
import { ConfirmationService } from 'primeng/api'
import { SkillAndInterestFacadeService } from './skills-and-interests-facade.service'
import { ISkillAndInterest } from './models'
import { FormGroup, FormControl, Validators } from '@angular/forms'
import { ActivatedRoute, Router } from '@angular/router'

@Component({
  selector: 'app-skills-and-interests',
  templateUrl: './skills-and-interests.component.html',
  styleUrls: ['./skills-and-interests.component.scss'],
  providers: [ConfirmationService],
})
export class SkillsAndInterestsComponent implements OnInit {
  skillsAndInterestsForm: FormGroup
  skillsAndInterests: Array<any> = []
  ready = false
  isFormSubmitting = false

  private _showSkillAndInterestDialog = false

  constructor(
    private facadeService: SkillAndInterestFacadeService,
    private confirmationService: ConfirmationService,
    private router: Router,
    private activatedRoute: ActivatedRoute
  ) {}

  private buildForm(): void {
    this.skillsAndInterestsForm = new FormGroup({
      name: new FormControl('', [Validators.required]),
    })
  }

  async addSkillAndInterest(): Promise<void> {
    try {
      const formValues = this.skillsAndInterestsForm.value as ISkillAndInterest
      const id = await this.facadeService.addSkillAndInterest(formValues)
      this.showSkillAndInterestDialog = false

      this.skillsAndInterests.push({ id, ...formValues })
    } catch (error) {
      console.log(error)
    } finally {
      this.isFormSubmitting = false
    }
  }

  async removeSkillAndInterest(id: string): Promise<void> {
    this.confirmationService.confirm({
      message: $localize`Are you sure?`,
      accept: async () => {
        await this.facadeService.removeSkillAndInterest(id)
        this.skillsAndInterests = this.skillsAndInterests.filter((skillAndInterest: any) => {
          return skillAndInterest.id !== id
        })
      },
    })
  }

  async ngOnInit() {
    this.buildForm()

    try {
      await this.facadeService.loadSkillAndInterest().then((skillsAndInterests: Array<any>) => {
        this.skillsAndInterests = skillsAndInterests
      })
    } catch (error) {
      console.log(error)
    } finally {
      this.ready = true
    }
  }

  openSkillAndInterestDialog() {
    this._showSkillAndInterestDialog = true
  }

  get showSkillAndInterestDialog() {
    return this._showSkillAndInterestDialog
  }

  set showSkillAndInterestDialog(show: boolean) {
    this._showSkillAndInterestDialog = show
    if (!show) {
      this.skillsAndInterestsForm.reset()
    }
  }

  goToNextPage() {
    this.router.navigate(['../general-information'], { relativeTo: this.activatedRoute })
  }
}
