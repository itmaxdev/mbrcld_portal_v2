import { HttpClient } from '@angular/common/http'
import { Inject, Injectable, InjectionToken, Optional } from '@angular/core'
import { HubConnection, HubConnectionBuilder } from '@microsoft/signalr'
import { from } from 'rxjs'
import { chatMesage } from './chatMesage'
import { MessagePackHubProtocol } from '@microsoft/signalr-protocol-msgpack'

export const API_BASE_URL = new InjectionToken<string>('API_BASE_URL')

@Injectable({
  providedIn: 'root',
})
export class SignalrService {
  private baseUrl: string
  public userId: string
  public messages: any[] = []
  public files: any[] = []
  public unreadMessages: any = {}
  public lastMessageOfRooms: any = {}
  public noMessages = false
  public currentRoomId: string
  public hubConnection: HubConnection
  private connectionUrl: string

  constructor(@Optional() @Inject(API_BASE_URL) baseUrl?: string) {
    this.baseUrl = baseUrl !== undefined && baseUrl !== null ? baseUrl : ''
    this.connectionUrl = this.baseUrl + '/chatHub'
  }

  // public connect = () => {
  //   this.startConnection()
  //   this.addListeners()
  // }

  public sendMessageToHub(message: string, roomId: string, userId: string) {
    const promise = this.hubConnection
      .invoke(
        'SendMessageToGroup',
        this.buildChatMessage(undefined, message, roomId, userId),
        roomId
      )
      .then()
      .catch()

    return from(promise)
  }

  public uploadFileToHub(fileUrl: string, roomId: string, userId: string) {
    const promise = this.hubConnection.invoke('UploadFile', fileUrl, roomId, userId).then().catch()

    return from(promise)
  }

  public markMessageAsReaded() {
    const promise = this.hubConnection
      .invoke(
        'MarkMessageAsReaded',
        this.buildChatMessage(
          this.messages[this.messages.length - 1].id,
          this.messages[this.messages.length - 1].text,
          this.currentRoomId,
          this.messages[this.messages.length - 1].userId
        ),
        this.userId
      )
      .then()
      .catch()
  }

  public createNewGroupInHub(groupName: string) {
    const promise = this.hubConnection
      .invoke('CreateGroup', this.hubConnection.connectionId, groupName)
      .then()
      .catch()

    return from(promise)
  }

  public getConnection(): HubConnection {
    return new HubConnectionBuilder()
      .withUrl(this.connectionUrl)
      .withHubProtocol(new MessagePackHubProtocol())
      .build()
  }

  private buildChatMessage(id: string, message: string, roomId: string, userId): chatMesage {
    return {
      Id: id,
      Text: message,
      UserId: userId,
      RoomId: roomId,
      MessageTypes: 0,
      DateTime: new Date(),
      ConnectionId: this.hubConnection.connectionId,
    }
  }

  scrollToBottom() {
    setTimeout(() => {
      const elem = document.getElementsByClassName('messages')[0]
      if (elem) {
        elem.scroll(0, elem.scrollHeight)
      }
    }, 0)
  }

  // private startConnection() {
  //   this.hubConnection = this.getConnection()

  //   this.hubConnection.start().then().catch()
  // }

  public addListeners() {
    this.hubConnection.on('messageReceivedFromApi', (data: chatMesage) => {
      this.messages.push(data)
    })
    this.hubConnection.on('messageReceivedFromHub', (data: chatMesage) => {
      this.messages.push(data)
    })
    this.hubConnection.on('newUserConnected', (message: string) => {})
    this.hubConnection.on('handleUploadedFile', (data: any) => {
      const body = {
        text: null,
        roomId: data.GroupName,
        userId: data.UserId,
        messageType: 1,
        time: new Date(),
        file: {
          path: data.Path,
          fileName: data.FileName,
        },
      }
      if (this.noMessages) {
        this.messages = [body]
        this.files = [body]
        this.noMessages = false
      } else {
        this.messages.push(body)
        this.files.push(body)
      }
      this.markMessageAsReaded()
      this.lastMessageOfRooms[data.GroupName] = 'File'
      if (this.currentRoomId != data.GroupName) {
        this.unreadMessages[data.GroupName]++
      }
      this.scrollToBottom()
    })
    this.hubConnection.on('handleMessage', (data: any) => {
      const body = {
        id: data.Id,
        text: data.Text,
        messageType: 0,
        time: data.DateTime,
        roomId: data.RoomId,
        userId: data.UserId,
      }
      if (this.noMessages) {
        this.messages = [body]
        this.noMessages = false
      } else {
        this.messages.push(body)
      }

      this.markMessageAsReaded()
      this.lastMessageOfRooms[data.RoomId] = data.Text
      if (this.currentRoomId != data.RoomId) {
        this.unreadMessages[data.RoomId]++
      }
      this.scrollToBottom()
    })
  }

  public stopConnection() {
    this.hubConnection.stop()
  }
}
