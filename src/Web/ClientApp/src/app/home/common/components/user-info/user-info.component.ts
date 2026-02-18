import { IUserGeneralInformation } from '../../../profile/common/profile.models'
import { Component, Inject, LOCALE_ID, OnDestroy, OnInit } from '@angular/core'
import { ConfirmationService } from 'primeng/api'
import { Subject } from 'rxjs'
import { takeUntil } from 'rxjs/operators'
import {
  IGetUserProfileViewModel,
  ProfileFacade,
} from '../../../profile/common/profile-facade.service'
import { MessageService } from 'primeng/api'
import { ProgramsClient } from 'src/app/shared/api.generated.clients'

@Component({
  selector: 'app-user-info',
  template: `
    <ng-container *ngIf="ready">
      <h1 class="font-bold text-2xl mb-4" i18n>My Profile</h1>
      <div class="flex flex-row profile-progress">
        <div class="relative flex-shrink-0 flex-grow-0">
          <img
            class="w-32 h-32 rounded-full object-contain"
            [src]="profilePicUrl"
            onerror="this.src= 'assets/images/no-photo.png'"
            alt=""
          />
          <button
            pButton
            icon="pi pi-camera"
            class="p-button-rounded absolute bottom-0 end-0"
            (click)="this.updateProfilePicture()"
          ></button>
        </div>
        <div class="flex flex-col flex-grow max-w-sm justify-center ps-8">
          <h3 class="font-semibold text-xl mb-6" i18n>{{ fullName }}</h3>
          <p-progressBar *ngIf="role != 4" [value]="progressBarPercentage"></p-progressBar>
        </div>
      </div>
      <p-toast></p-toast>
    </ng-container>

    <p-dialog
      #uploadProfilePictureDialog
      header="Update Profile Picture"
      i18n-header
      styleClass="w-full max-w-sm"
      [(visible)]="showUploadProfilePictureDialog"
      [modal]="true"
      [focusTrap]="true"
      [focusOnShow]="true"
      [closable]="!upload.progress"
      [draggable]="false"
    >
      <div class="py-4">
        <p-fileUpload
          #upload
          appPrimeNGi18n
          mode="basic"
          accept="image/*"
          url="/api/profile/profile-picture"
          name="file"
          [disabled]="upload.progress"
          [auto]="true"
          [maxFileSize]="2097152"
          (onUpload)="handleOnUpload()"
        ></p-fileUpload>
      </div>
      <div *ngIf="upload.progress" class="pt-4">
        <p-progressBar mode="indeterminate"></p-progressBar>
      </div>
    </p-dialog>

    <p-confirmDialog
      appPrimeNGi18n
      header="Confirmation"
      i18n-header
      icon="pi pi-exclamation-triangle"
      styleClass="w-full max-w-sm"
    >
    </p-confirmDialog>
  `,
  styleUrls: ['./user-info.component.scss'],
  providers: [ConfirmationService],
})
export class UserInfoComponent implements OnInit, OnDestroy {
  fullName: string
  profilePicUrl: string
  ready = false
  role: number
  progressBarPercentage = 0

  private destroy$ = new Subject<boolean>()

  _showUploadProfilePictureDialog = false

  get showUploadProfilePictureDialog(): boolean {
    return this._showUploadProfilePictureDialog
  }

  set showUploadProfilePictureDialog(value: boolean) {
    this._showUploadProfilePictureDialog = value
  }

  constructor(
    @Inject(LOCALE_ID) private locale: string,
    private facade: ProfileFacade,
    private programs: ProgramsClient,
    private messageService: MessageService
  ) {}

  async ngOnInit() {
    const profile: any = await this.facade.loadGeneralInformation()
    this.handleProfileChanges(profile)

    this.role = profile.role
    this.ready = true

    this.facade.profileChanges.pipe(takeUntil(this.destroy$)).subscribe((profile) => {
      this.handleProfileChanges(profile)
    })

    this.loadFormsProgress()
  }

  ngOnDestroy() {
    this.destroy$.next(true)
  }

  async loadFormsProgress() {
    const formProgress = await this.facade.loadFormProgress()
    this.progressBarPercentage = formProgress.completionPercentage

    this.facade.formChanges.pipe(takeUntil(this.destroy$)).subscribe((formProgress) => {
      if (
        this.progressBarPercentage != formProgress.completionPercentage &&
        formProgress.completionPercentage == 100
      ) {
        var inprogressPrograms = this.programs.inprogressPrograms().subscribe((data) => {
          if (data) {
            let shouldInform = !false
            for (let x = 0; x < data.length; x++) {
              if (data[x].completed < 100) {
                shouldInform = true
                break
              }
            }
            if (shouldInform) {
              this.messageService.add({
                summary: $localize`Information`,
                detail: $localize`Thank you for completing the profile, please go to programs to start enrolling!`,
                severity: 'success',
                closable: true,
                life: 5000,
              })
            }
          }
          inprogressPrograms.unsubscribe()
        })
      }
      this.progressBarPercentage = formProgress.completionPercentage
    })
  }

  updateProfilePicture() {
    this.showUploadProfilePictureDialog = true
  }

  async handleOnUpload() {
    try {
      this.facade.reloadProfile()
    } finally {
      this.showUploadProfilePictureDialog = false
    }
  }

  private handleProfileChanges(profile: IGetUserProfileViewModel) {
    this.updateFullName(profile)
    this.profilePicUrl = profile.profilePictureUrl
  }

  private updateFullName(profile: IUserGeneralInformation) {
    if (this.locale === 'ar') {
      this.fullName = `${profile.firstName_AR || ''} ${profile.lastName_AR || ''}`.trim()
    } else {
      this.fullName = `${profile.firstName || ''} ${profile.lastName || ''}`.trim()
    }
  }
}
