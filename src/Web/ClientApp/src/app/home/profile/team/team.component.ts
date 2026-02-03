import { Component, OnInit } from '@angular/core'
import { FormControl, FormGroup, Validators } from '@angular/forms'
import { tap } from 'rxjs/operators'
import {
  FileParameter,
  IListTeamMembersViewModel,
  UniversityTeamMembersClient,
} from 'src/app/shared/api.generated.clients'

@Component({
  selector: 'app-team',
  templateUrl: './team.component.html',
  styleUrls: ['./team.component.scss'],
})
export class TeamComponent implements OnInit {
  teamForm: FormGroup
  ready = false
  uploadedFile: any = undefined
  editForm: FormGroup
  addMember = false
  editMember = false
  editMemberId: string
  members: IListTeamMembersViewModel[]

  constructor(private universityTeamMembers: UniversityTeamMembersClient) {}

  ngOnInit(): void {
    this.buildForm()
    this.buildEditForm()
    this.universityTeamMembers.instructor().subscribe((data) => {
      this.members = data
      this.ready = true
    })
  }

  private buildForm() {
    this.teamForm = new FormGroup({
      firstName: new FormControl('', [Validators.required]),
      lastName: new FormControl('', [Validators.required]),
      linkedIn: new FormControl('', [Validators.required]),
      jobPosition: new FormControl('', [Validators.required]),
      education: new FormControl('', [Validators.required]),
      email: new FormControl('', [Validators.required, Validators.email]),
      residenceCountry: new FormControl('', [Validators.required]),
      nationality: new FormControl('', [Validators.required]),
      aboutMember: new FormControl('', []),
    })
  }

  private buildEditForm() {
    this.editForm = new FormGroup({
      firstName: new FormControl('', [Validators.required]),
      lastName: new FormControl('', [Validators.required]),
      linkedIn: new FormControl('', [Validators.required]),
      jobPosition: new FormControl('', [Validators.required]),
      education: new FormControl('', [Validators.required]),
      email: new FormControl('', [Validators.required, Validators.email]),
      residenceCountry: new FormControl('', [Validators.required]),
      nationality: new FormControl('', [Validators.required]),
      aboutMember: new FormControl('', []),
    })
  }

  onSelect(event) {
    this.uploadedFile = event
  }

  async onSubmit() {
    if (!this.teamForm.valid || !this.uploadedFile) {
      return
    }

    if (this.teamForm.dirty) {
      const formValue: any = this.teamForm.value
      const body: FileParameter = {
        data: this.uploadedFile.currentFiles[0],
        fileName: this.uploadedFile.currentFiles[0].name,
      }
      this.universityTeamMembers
        .universityTeamMemberPost(
          formValue.firstName,
          formValue.lastName,
          formValue.aboutMember,
          formValue.email,
          formValue.nationality,
          formValue.residenceCountry,
          formValue.jobPosition,
          formValue.education,
          formValue.linkedIn,
          body
        )
        .pipe(
          tap(() => {
            window.location.reload()
          })
        )
        .toPromise()
    }
  }

  async onSave() {
    if (!this.editForm.valid) {
      return
    }

    if (this.editForm.valid) {
      const formValue: any = this.editForm.value
      let body: FileParameter | undefined = undefined
      if (this.uploadedFile) {
        body = {
          data: this.uploadedFile.currentFiles[0],
          fileName: this.uploadedFile.currentFiles[0].name,
        }
      }

      this.universityTeamMembers
        .universityTeamMemberPut(
          this.editMemberId,
          formValue.firstName,
          formValue.lastName,
          formValue.aboutMember,
          formValue.email,
          formValue.nationality,
          formValue.residenceCountry,
          formValue.jobPosition,
          formValue.education,
          formValue.linkedIn,
          body
        )
        .pipe(
          tap(() => {
            window.location.reload()
          })
        )
        .toPromise()
    }
  }

  deleteItem(event: string) {
    const body: any = { id: event }
    this.members = this.members.filter((item) => item.id !== event)
    this.universityTeamMembers
      .deactivate(body)
      .pipe(tap(() => {}))
      .toPromise()
  }

  teamMember(event: string) {
    this.editMemberId = event
    this.universityTeamMembers.universityTeamMemberGet(event).subscribe((data: any) => {
      this.editForm.setValue({
        firstName: data.firstName,
        lastName: data.lastName,
        jobPosition: data.jobPosition,
        education: data.education,
        email: data.email,
        linkedIn: data.linkedIn,
        residenceCountry: data.residenceCountry,
        nationality: data.nationality,
        aboutMember: data.aboutMember,
      })
    })
    this.editMember = true
  }
}
