import { Injectable } from '@angular/core'
import { Subject } from 'rxjs'

@Injectable({ providedIn: 'root' })
export class NavBarService {
  private visible = false

  visibilityChange = new Subject<boolean>()

  toggle() {
    this.visible = !this.visible
    this.notify()
  }

  open() {
    this.visible = true
    this.notify()
  }

  close() {
    this.visible = false
    this.notify()
  }

  private notify() {
    this.visibilityChange.next(this.visible)
  }
}
