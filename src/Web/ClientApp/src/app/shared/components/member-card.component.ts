import { Component, Input, OnInit, ViewEncapsulation, EventEmitter, Output } from '@angular/core'
import { IListTeamMembersViewModel } from '../api.generated.clients'
import { ConfirmationService } from 'primeng/api'

@Component({
  selector: 'app-member-card',
  template: `
    <div class="member-card p-4 bg-gray-100">
      <span *ngIf="userRole === 4" class="delete flex justify-end">
        <i
          (click)="deleteDialog()"
          class="pi pi-times"
          style="
          font-size: 1rem;
          color: #9ca3af;
          cursor: pointer;
        "
        ></i>
      </span>
      <div (click)="viewMember()" class="cursor-pointer grid gap-4">
        <div class="header flex items-center">
          <div class="image">
            <img [src]="data.pictureUrl" alt="" class="w-20 h-20 rounded-full" />
          </div>
          <div class="ml-4">
            <p class="text-blue-400 font-semibold text-xl">{{ data.name }}</p>
          </div>
        </div>
        <div class="member-caption">
          <p class="member-caption-text text-gray-500 text-xl">{{ data.jobPosition }}</p>
        </div>
        <div class="member-content">
          <p class="member-content-text text-gray-600 text-lg">
            {{ data.aboutMember }}
          </p>
        </div>
        <div class="soc_links w-16">
          <div class="icons grid grid-cols-2 gap-4">
            <a [href]="'mailto:' + data.email"
              ><img class="w-6 h-6" src="assets/images/ico-envelope.svg" alt=""
            /></a>
            <a [href]="data.linkedIn" target="_blank"
              ><img class="w-6 h-6" src="assets/images/ico-linkedin-blue.svg" alt=""
            /></a>
          </div>
        </div>
      </div>
    </div>
    <p-confirmDialog
      [style]="{ width: '40vw' }"
      [(visible)]="deleteMemberDialog"
      [baseZIndex]="10000"
    ></p-confirmDialog>
  `,
  styles: [
    `
      .p-button-icon-only {
        display: none;
      }

      .member-card {
        max-width: 400px;
      }

      .member-caption {
        overflow: hidden;
        text-overflow: ellipsis;
      }

      .member-caption-text {
        height: 31px;
        min-height: 31px;
        max-height: 31px;
        overflow: hidden;
        display: -webkit-box;
        -webkit-line-clamp: 1;
        -webkit-box-orient: vertical;
      }

      .member-content {
        overflow: hidden;
        text-overflow: ellipsis;
      }

      .member-content-text {
        height: 70px;
        min-height: 70px;
        overflow: hidden;
        display: -webkit-box;
        -webkit-line-clamp: 3;
        -webkit-box-orient: vertical;
      }
    `,
  ],
  providers: [ConfirmationService],
  encapsulation: ViewEncapsulation.None,
})
export class MemberCardComponent implements OnInit {
  deleteMemberDialog = false
  @Input() userRole = 4
  @Input() data: IListTeamMembersViewModel
  @Output() clickedId: EventEmitter<string> = new EventEmitter<string>()
  @Output() clickedMember: EventEmitter<string> = new EventEmitter<string>()
  constructor(private confirmationsService: ConfirmationService) {}
  ngOnInit(): void {}

  deleteDialog() {
    this.deleteMemberDialog = true
    this.confirmationsService.confirm({
      message: 'Are you sure that you want to delete the member?',
      header: 'Confirmation',
      icon: 'pi pi-exclamation-triangle',
      accept: () => {
        this.deleteMember()
      },
      reject: () => {},
    })
  }

  deleteMember() {
    this.clickedId.emit(this.data.id)
  }

  viewMember() {
    this.clickedMember.emit(this.data.id)
  }
}
