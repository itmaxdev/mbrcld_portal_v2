import { Component, OnInit } from '@angular/core'
import { ActivatedRoute } from '@angular/router'
import {
  ListInstructorProjectsViewModel,
  ProjectsClient,
} from 'src/app/shared/api.generated.clients'
import * as moment from 'moment'
import { tap } from 'rxjs/operators'
import { MessageService, PrimeNGConfig } from 'primeng/api'
import { SectionDataService } from 'src/app/shared/services/section-data.service'
import { FormControl, FormGroup, Validators } from '@angular/forms'

@Component({
  selector: 'app-programs-submitted-project',
  templateUrl: './programs-submitted-project.component.html',
  styleUrls: ['./programs-submitted-project.component.scss'],
  providers: [MessageService],
})
export class ProgramsSubmittedProjectComponent implements OnInit {
  id: string
  moduleId: string
  modalHeader = 'Reject Project'
  modalHeaderApprove = 'Approve Project'
  isOpenedDialog = false
  isOpenedDialogApprove = false
  ready = true
  endDate: string
  startDate: string
  rejectText: string
  approveText: string
  rejectForm: FormGroup
  onSend = false
  onApprove = false
  uploadedFile: any
  projectData: ListInstructorProjectsViewModel

  constructor(
    private route: ActivatedRoute,
    private projects: ProjectsClient,
    private section: SectionDataService,
    private primengConfig: PrimeNGConfig,
    private messageService: MessageService
  ) {}

  goBack() {
    this.section.redirectBack(1)
  }

  showSuccess(action) {
    this.messageService.add({
      key: 'tr',
      severity: 'success',
      summary: 'Success',
      detail: 'The project was ' + action + '!',
    })
  }

  sendAttachment() {
    if (this.uploadedFile.currentFiles.length > 0 && this.rejectForm.valid) {
      const body: any = {
        data: this.uploadedFile.currentFiles[0],
        fileName: this.uploadedFile.currentFiles[0].name,
      }
      this.projects
        .addAttachment(this.id, this.approveText, body)
        .pipe(
          tap(() => {
            this.showSuccess('sended for review')
            this.isOpenedDialog = false
            setTimeout(() => {
              this.section.redirectBack(1)
            }, 3000)
          })
        )
        .toPromise()
    }
  }

  onSelect(event) {
    this.uploadedFile = event
  }

  openApproveDialog() {
    this.isOpenedDialogApprove = true
  }

  approveProject() {
    this.onApprove = true
    const body: any = {
      id: this.id,
    }
    this.projects
      .approveProject(body)
      .pipe(
        tap(() => {
          this.showSuccess('approved')
          this.onApprove = false
          setTimeout(() => {
            this.section.redirectBack(1)
          }, 3000)
        })
      )
      .toPromise()
  }

  rejectProject() {
    this.isOpenedDialog = true
  }

  ngOnInit(): void {
    this.rejectForm = new FormGroup({
      approveText: new FormControl(null, [Validators.required]),
    })

    this.ready = false
    this.primengConfig.ripple = true
    this.id = this.route.snapshot.paramMap.get('projectId')
    this.moduleId = this.route.snapshot.paramMap.get('modulesId')
    this.projects.underreviewProjects(this.moduleId).subscribe((data) => {
      this.projectData = data.filter((item) => item.id == this.id)[0]
      this.startDate = moment(this.projectData.startDate)
        .lang('en')
        .format('dddd, MMMM Do YYYY, h:mm:ss a')
      this.endDate = moment(this.projectData.endDate)
        .lang('en')
        .format('dddd, MMMM Do YYYY, h:mm:ss a')
      this.ready = true
    })
  }
}
