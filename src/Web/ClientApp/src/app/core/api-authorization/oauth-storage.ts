import { OAuthStorage } from 'angular-oauth2-oidc'
import { Injectable } from '@angular/core'
// import * as SecureLocalStorage from 'secure-ls'

@Injectable()
export class SecureStorage implements OAuthStorage {
  // private ls: SecureLocalStorage
  private storage = localStorage

  constructor() {
    // this.ls = new SecureLocalStorage({
    //   encodingType: 'aes',
    // })
  }

  getItem(key: string): string {
    // return this.ls.get(key)
    return this.storage.getItem(key)
  }

  removeItem(key: string): void {
    // this.ls.remove(key)
    this.storage.removeItem(key)
  }

  setItem(key: string, data: string): void {
    // this.ls.set(key, data)
    this.storage.setItem(key, data)
  }
}
