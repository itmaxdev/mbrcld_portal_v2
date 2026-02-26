import { Component, OnInit } from '@angular/core'
import { FormControl, FormGroup, Validators } from '@angular/forms'
import { tap } from 'rxjs/operators'
import {
  FileParameter,
  IListTeamMembersViewModel,
  UniversityTeamMembersClient,
} from 'src/app/shared/api.generated.clients'

@Component({
  selector: 'app-registrant-module-team',
  templateUrl: './module-team.component.html',
})
export class RegistrantModuleTeamComponent implements OnInit {
  teamForm: FormGroup
  ready = false
  uploadedFile: any = undefined
  addMember = false
  editMember = false
  showMemberPopup = false
  editMemberId: string
  members: IListTeamMembersViewModel[]

  constructor(private universityTeamMembers: UniversityTeamMembersClient) {}

  ngOnInit(): void {
    this.buildForm()
    this.loadMembers()
  }

  private loadMembers() {
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

  onSelect(event) {
    this.uploadedFile = event
  }

  openAddMember() {
    this.addMember = true
    this.editMember = false
    this.teamForm.reset()
    this.showMemberPopup = true
  }

  async onSubmit() {
    if (this.teamForm.invalid) {
      this.teamForm.markAllAsTouched()
      return
    }

    if (!this.editMember && !this.uploadedFile) {
      return
    }

    const formValue: any = this.teamForm.value
    let body: FileParameter | undefined = undefined

    if (this.uploadedFile && this.uploadedFile.files && this.uploadedFile.files.length > 0) {
      body = {
        data: this.uploadedFile.files[0],
        fileName: this.uploadedFile.files[0].name,
      }
    }

    try {
      if (this.editMember) {
        await this.universityTeamMembers
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
          .toPromise()
      } else {
        await this.universityTeamMembers
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
          .toPromise()
      }

      this.showMemberPopup = false
      this.addMember = false
      this.editMember = false
      this.uploadedFile = undefined
      this.teamForm.reset()
      this.loadMembers()
    } catch (error) {
      console.error('Error saving team member:', error)
    }
  }

  deleteItem(event: string) {
    const body: any = { id: event }
    this.universityTeamMembers
      .deactivate(body)
      .pipe(
        tap(() => {
          this.members = this.members.filter((item) => item.id !== event)
        })
      )
      .toPromise()
  }

  teamMember(event: string) {
    this.editMemberId = event
    this.universityTeamMembers.universityTeamMemberGet(event).subscribe((data: any) => {
      this.teamForm.patchValue({
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
      this.addMember = false
      this.editMember = true
      this.showMemberPopup = true
    })
  }
}
