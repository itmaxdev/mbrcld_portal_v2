import { ProgramApplyService } from '../program-apply.service'
import { Component } from '@angular/core'
import { MessageService } from 'primeng/api'
import { ProfileFacade } from 'src/app/home/profile/common/profile-facade.service'
import { ApiProblemDetails } from 'src/app/shared/api.generated.clients'
import { Router } from '@angular/router'

declare let dataLayer: any[]

@Component({
  selector: 'app-acknowledgment',
  templateUrl: 'acknowledgment.component.html',
  providers: [MessageService],
})
export class AcknowledgmentComponent {
  accepted = false
  successfullyEnrolled = false
  isSubmitting = false
  activeIndex = 4

  constructor(
    private shared: ProgramApplyService,
    private profile: ProfileFacade,
    private messageService: MessageService,
    private router: Router
  ) { }

  async completeEnrollment() {
    if (!this.accepted) {
      return
    }

    this.isSubmitting = true

    try {
      const { completionPercentage } = await this.profile.loadFormProgress()
      if (completionPercentage < 100) {
        this.router.navigate(['home', 'profile'])
        return
      }

      await this.shared.completeEnrollment()
      this.successfullyEnrolled = true

      // Push GTM event upon successful enrollment
      if (typeof dataLayer !== 'undefined') {
        dataLayer.push({
          'event': 'formSubmission',
          'formId': 'completeEnrollment',
          'status': 'success'
        });

      }

    } catch (e) {
      if (e instanceof ApiProblemDetails && e.errors.includes('MissingInformation')) {
        this.messageService.add({
          summary: $localize`Missing information`,
          detail: $localize`Please fill in all the required information then try again`,
          closable: true,
          life: 8000,
          severity: 'error',
        })
      }
    } finally {
      this.isSubmitting = false
    }
  }
}
