import { Component, OnInit } from '@angular/core'
import { ActivatedRoute, Router } from '@angular/router'
import {
  ChatClient,
  FileParameter,
  ModulesClient,
  ProfileClient,
} from 'src/app/shared/api.generated.clients'
import { SignalrService } from './signalr.service'
import { tap } from 'rxjs/operators'
import { SectionDataService } from 'src/app/shared/services/section-data.service'
import { MessageService } from 'primeng/api'

@Component({
  selector: 'app-message-chat',
  templateUrl: './message-chat.component.html',
  styleUrls: ['./message-chat.component.scss'],
  providers: [MessageService],
})
export class MessageChatComponent implements OnInit {
  text = ''
  role: number
  allChats: any
  ready = false
  userId: string
  roomId: string
  moduleId: string
  messageData: any
  title = 'chat-ui'
  activeChat: any = {}
  roomName = ''
  applicantsList: any[] = []
  isVisibleCreateGroupBtn = false
  instructorId: string
  messageReady = false
  uploadedFiles: any = undefined
  selectedRows: any[] = []
  currentChatUsers: any[] = []
  applicantsDialog = false
  isApplicantsReady = false

  constructor(
    private router: Router,
    private route: ActivatedRoute,
    private chatClient: ChatClient,
    private profile: ProfileClient,
    private modulesClient: ModulesClient,
    public signalRService: SignalrService,
    private messageService: MessageService,
    private sectionData: SectionDataService
  ) {
    this.disconnect().then(() => {
      this.connect()
    })
  }

  ngOnInit(): void {
    this.ready = false
    this.messageReady = false
    this.isApplicantsReady = false
    this.moduleId = this.route.snapshot.paramMap.get('modulesId')
    const profileInfo = JSON.parse(localStorage.getItem('profile_info'))
    this.role = profileInfo.role
    this.userId = this.signalRService.userId = profileInfo.id
    this.roomId = this.route.snapshot.paramMap.get('roomId')

    if (this.role === 3 && !this.moduleId) {
      this.isVisibleCreateGroupBtn = false
      this.moduleId = '12341234-1234-1234-1234-123412341234'
    } else {
      this.isVisibleCreateGroupBtn = true
      this.modulesClient.modules(this.moduleId).subscribe((data) => {
        this.instructorId = data.instructorId
      })
    }

    if (this.role === 2) {
      this.modulesClient.modules(this.moduleId).subscribe((data) => {
        this.instructorId = data.instructorId
      })
    }

    switch (this.role) {
      case 3:
        this.profile.usersForChat().subscribe((data) => {
          this.applicantsList = data
          this.applicantsList.map((item) => (item['selected'] = false))
          this.isApplicantsReady = true
        })
        break
      case 4:
        this.modulesClient.moduleApplicants(this.moduleId).subscribe((data) => {
          this.applicantsList = data
          this.applicantsList.map((item) => (item['selected'] = false))
          this.isApplicantsReady = true
        })
        break
    }

    this.signalRService.hubConnection.start().then(() => {
      this.signalRService.addListeners()
      this.chatClient.chat(this.moduleId).subscribe((data) => {
        this.allChats = JSON.parse(data)
        this.allChats.map((item, index) => {
          if (item.lastMessage) {
            this.signalRService.lastMessageOfRooms[item.lastMessage.roomId] = item.lastMessage.text
            if (item.lastMessage.userId != this.userId) {
              this.signalRService.unreadMessages[item.lastMessage.roomId] = item.unreadMessagesCount
            } else {
              this.signalRService.unreadMessages[item.lastMessage.roomId] = 0
            }
          }

          if (this.roomId) {
            if (this.roomId === item.id) {
              this.activeChat[item.id] = true
              this.currentChatUsers = item.participants
              this.signalRService.currentRoomId = item.id
            } else {
              this.activeChat[item.id] = false
            }
          } else {
            if (index === 0) {
              this.activeChat[item.id] = true
              this.currentChatUsers = item.participants
              this.signalRService.currentRoomId = item.id
            } else {
              this.activeChat[item.id] = false
            }
          }

          this.signalRService.createNewGroupInHub(item.id)
        })
        if (this.allChats.length > 0) {
          this.chatClient.messages(this.allChats[0].id, undefined).subscribe((messageData) => {
            this.signalRService.files = []
            this.signalRService.messages = JSON.parse(messageData)
            this.signalRService.messages.map((item) => {
              if (item.messageType == 1) {
                this.signalRService.files.push(item)
              }
            })

            if (this.signalRService.messages.length > 0) {
              this.signalRService.noMessages = false
              this.signalRService.markMessageAsReaded()
            } else {
              this.signalRService.noMessages = true
            }
            this.messageReady = true
            this.ready = true
            this.signalRService.scrollToBottom()
          })
        } else {
          this.messageReady = true
          this.ready = true
        }
      })
    })
  }

  goBack() {
    this.sectionData.redirectBack(2)
  }

  showDialog() {
    this.applicantsDialog = true
  }

  selectRow(checkValue) {
    if (checkValue) {
      this.selectedRows = this.applicantsList
    } else {
      this.selectedRows = []
    }
  }

  createRoom() {
    const selectedParticipants: string[] = []
    this.selectedRows.map((item) => selectedParticipants.push(item.id))
    const body: any = {
      name: this.roomName,
      moduleId: this.moduleId,
      participants: selectedParticipants,
    }

    Promise.all([
      this.chatClient
        .room(body)
        .pipe(
          tap((data) => {
            const response = JSON.parse(data)
            if (response.value.participants.length === 0) {
              this.roomId = response.value.id
              this.messageService.add({
                key: 'tr',
                life: 5000,
                severity: 'success',
                summary: 'Success',
                detail: 'The chat already exists. Reloading after 5 seconds!',
              })
              setTimeout(() => {
                window.location.reload()
              }, 5000)
            } else {
              this.selectedRows = []
              this.applicantsDialog = false
              window.location.reload()
            }
          })
        )
        .toPromise(),
    ])
  }

  async disconnect() {
    if (this.signalRService.hubConnection) {
      this.signalRService.stopConnection()
    }
  }

  public connect = async () => {
    this.signalRService.hubConnection = this.signalRService.getConnection()
  }

  createRoomWithInstructor() {
    const selectedParticipants: string[] = [this.instructorId]
    const body: any = {
      name: '',
      moduleId: this.moduleId,
      participants: selectedParticipants,
    }

    Promise.all([
      this.chatClient
        .room(body)
        .pipe(
          tap((data) => {
            const res = JSON.parse(data)
            const newRoute = this.sectionData.redirectBack(1, true) + '/' + res.value.id
            this.router.navigateByUrl(newRoute)
            setTimeout(() => {
              window.location.reload()
            })
          })
        )
        .toPromise(),
    ])
  }

  onUpload(event, form) {
    const file: FileParameter = {
      data: event.currentFiles[0],
      fileName: event.currentFiles[0].name,
    }
    if (file) {
      this.chatClient.upload(this.signalRService.currentRoomId, file).subscribe((res: any) => {
        const path = JSON.parse(res).value
        const body = {
          text: null,
          roomId: this.signalRService.currentRoomId,
          userId: this.userId,
          messageType: 1,
          time: new Date(),
          file: {
            path: path,
            fileName: event.currentFiles[0].name,
          },
        }

        this.signalRService.uploadFileToHub(path, this.signalRService.currentRoomId, this.userId)
      })
    }

    for (const file of event.files) {
      this.uploadedFiles = file
    }
    form.clear()
  }

  changeParticipant(event) {
    if (this.signalRService.currentRoomId !== event) {
      this.messageReady = false
      this.signalRService.unreadMessages[event] = 0

      for (const property in this.activeChat) {
        this.activeChat[property] = false
      }

      this.allChats.map((item) => {
        if (item.id == event) {
          this.currentChatUsers = item.participants
          this.signalRService.currentRoomId = item.id
        }
      })

      this.activeChat[event] = true

      this.messageReady = false

      this.chatClient.messages(event, undefined).subscribe((data) => {
        this.signalRService.files = []
        this.signalRService.messages = JSON.parse(data)
        this.signalRService.messages.map((item) => {
          if (item.messageType == 1) {
            this.signalRService.files.push(item)
          }
        })
        if (this.signalRService.messages.length > 0) {
          this.signalRService.noMessages = false
          this.signalRService.markMessageAsReaded()
        } else {
          this.signalRService.noMessages = true
        }
        this.messageReady = true
        this.signalRService.scrollToBottom()
      })
    }
  }

  sendMessage(): void {
    this.signalRService
      .sendMessageToHub(this.text, this.signalRService.currentRoomId, this.userId)
      .subscribe({
        next: (_) => ((this.text = ''), this.signalRService.scrollToBottom()),
        error: (err) => console.error(err),
      })
  }
}
