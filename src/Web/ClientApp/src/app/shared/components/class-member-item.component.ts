import { Component, Input, OnInit, ViewEncapsulation, Output, EventEmitter } from '@angular/core'
import { Router } from '@angular/router'
import { MenuItem, MessageService } from 'primeng/api'
import { tap } from 'rxjs/operators'
import { ChatClient } from '../api.generated.clients'
import { SectionDataService } from '../services/section-data.service'

@Component({
  selector: 'app-class-member-item',
  template: `
    <p-toast position="top-right" key="tr"></p-toast>
    <div class="studentCard" [ngClass]="{ col2: !fullWidth }">
      <a (click)="onProfileClick()" class="inner cursor-pointer">
        <div class="imgBox">
          <picture>
            <img
              [src]="data.profilePictureUrl"
              onerror="this.src='assets/images/no-photo.png'"
              alt=""
              width="100"
              height="100"
              loading="lazy"
            />
          </picture>
        </div>

        <div class="contentBox">
          <div class="title">{{ data.name || data.fullName }}</div>
        </div>
      </a>
      <div *ngIf="role === 3 && !isEliteClub" class="toggleBtn absolute top-2 right-2">
        <button
          type="button"
          pButton
          pRipple
          class="bg-white h-8"
          icon="none"
          (click)="menu.toggle($event); $event.stopPropagation()"
        >
          <i class="pi pi-ellipsis-h pi-2 text-gray-600" style="font-size: 2rem;"></i>
        </button>
        <p-menu #menu [popup]="true" [model]="items"></p-menu>
      </div>
    </div>

    <p-dialog
      [header]="'Please select to which group you want to add ' + data.fullName"
      [style]="{ height: '50vh', width: '30vw' }"
      [(visible)]="addToExistingGroup"
    >
      <div *ngIf="isGroupsReady; else loadingGroups" class="flex w-full justify-between">
        <p-dropdown
          [options]="groups"
          [(ngModel)]="selectedGroup"
          optionLabel="name"
          placeholder="Select a Group"
          appendTo="body"
        ></p-dropdown>
      </div>
      <ng-template pTemplate="footer">
        <p-button
          icon="pi pi-check"
          (click)="addToGroup()"
          label="Add to Group"
          class="p-button-text"
        ></p-button>
      </ng-template>
      <ng-template #loadingGroups>
        <app-progress-spinner></app-progress-spinner>
      </ng-template>
    </p-dialog>
  `,
  styles: [
    `
      .studentCard {
        position: relative;
      }
      .toggleBtn .p-button {
        background: none;
        color: gray;
        border: none !important;
      }
      .toggleBtn .p-button:focus {
        box-shadow: none !important;
      }
      .toggleBtn .p-button:hover {
        background: none !important;
        color: gray;
      }
    `,
  ],
  encapsulation: ViewEncapsulation.None,
  providers: [MessageService],
})
export class ClassMemberItemController implements OnInit {
  @Input() data: any
  @Input() fullWidth = false
  @Input() isEliteClub = false
  @Output() openProfile = new EventEmitter<string>()

  public items: MenuItem[]
  public role: number
  public id: string
  public roomId: string = undefined
  public addToExistingGroup = false
  public groups: any[]
  public selectedGroup: any
  public isGroupsReady = false

  onProfileClick() {
    this.openProfile.emit(this.data.id)
  }

  constructor(
    private chatClient: ChatClient,
    private router: Router,
    private section: SectionDataService,
    private messageService: MessageService
  ) {}

  ngOnInit(): void {
    const profileInfo = JSON.parse(localStorage.getItem('profile_info'))
    this.role = profileInfo.role
    this.id = profileInfo.id

    this.items = [
      {
        items: [
          {
            label: 'Start Chat',
            icon: 'pi pi-comment',
            command: () => {
              const selectedParticipants: string[] = []
              selectedParticipants.push(this.data.id)
              const body: any = {
                name: this.data.name || this.data.fullName || 'Chat With User',
                moduleId: '12341234-1234-1234-1234-123412341234',
                participants: selectedParticipants,
              }
              if (this.id !== this.data.id) {
                Promise.all([
                  this.chatClient
                    .room(body)
                    .pipe(
                      tap((data) => {
                        const response = JSON.parse(data)
                        if (response.value.participants.length === 0) {
                          this.roomId = response.value.id
                        }
                        this.router.navigateByUrl(
                          this.section.redirectBack(1, true) + '/chat/' + this.roomId
                        )
                      })
                    )
                    .toPromise(),
                ])
              } else {
                this.messageService.add({
                  severity: 'error',
                  key: 'tr',
                  life: 6000,
                  closable: true,
                  summary: 'Error',
                  detail: 'You cant start chatting with you.',
                })
              }
            },
          },
          {
            label: 'Add to existing group',
            icon: 'pi pi-comments',
            command: () => {
              this.addToExistingGroup = true
              this.isGroupsReady = false
              this.chatClient.rooms(this.data.id).subscribe((data) => {
                this.groups = data
                this.isGroupsReady = true
              })
            },
          },
        ],
      },
    ]
  }

  addToGroup() {
    this.chatClient
      .roomPost(this.data.id, this.selectedGroup.id)
      .pipe(
        tap(() => {
          this.messageService.add({
            severity: 'success',
            key: 'tr',
            life: 6000,
            closable: true,
            summary: 'Success',
            detail: this.data.fullName + ' added to group.',
          })
          this.addToExistingGroup = false
        })
      )
      .toPromise()
  }
}
