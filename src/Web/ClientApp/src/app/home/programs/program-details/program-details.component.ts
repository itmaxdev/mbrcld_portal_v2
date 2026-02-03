import { Component, OnInit } from '@angular/core'

@Component({
  selector: 'app-program-details',
  templateUrl: './program-details.component.html',
  styleUrls: ['./program-details.component.css'],
})
export class ProgramDetailsComponent implements OnInit {
  galleryImages
  displayCustom: boolean
  activeIndex = 0

  constructor() {}

  ngOnInit() {
    this.galleryImages = [
      {
        small: 'assets/images/program/1046/3_thumb.jpeg',
        medium: 'assets/images/program/1046/3_big-thumb.jpeg',
        big: 'assets/images/program/1046/3.jpeg',
      },
      {
        small: 'assets/images/program/1047/1_thumb.jpeg',
        medium: 'assets/images/program/1047/1_big-thumb.jpeg',
        big: 'assets/images/program/1047/1.jpeg',
      },
      {
        small: 'assets/images/program/1048/2_thumb.jpeg',
        medium: 'assets/images/program/1048/2_big-thumb.jpeg',
        big: 'assets/images/program/1048/2.jpeg',
      },
      {
        small: 'assets/images/program/1049/1fqawebiv03rust6qeyek43af3puolu6rjdywwvb_thumb.jpeg',
        medium:
          'assets/images/program/1049/1fqawebiv03rust6qeyek43af3puolu6rjdywwvb_big-thumb.jpeg',
        big: 'assets/images/program/1049/1fqawebiv03rust6qeyek43af3puolu6rjdywwvb.jpeg',
      },
    ]
  }

  imageClick(index) {
    this.activeIndex = index
    this.displayCustom = true
  }
}
