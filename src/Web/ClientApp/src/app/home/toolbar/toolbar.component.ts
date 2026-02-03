import { Component } from '@angular/core'
import { NavBarService } from '../nav-bar/nav-bar.service'

@Component({
  selector: 'app-toolbar',
  templateUrl: './toolbar.component.html',
  styleUrls: ['./toolbar.component.scss'],
})
export class ToolbarComponent {
  constructor(private navBarService: NavBarService) {}

  openNavBar() {
    this.navBarService.open()
  }
}
