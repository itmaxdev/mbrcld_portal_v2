import { Component, Input, OnInit } from '@angular/core'

@Component({
  selector: 'app-profile-image',
  template: `
    <div [ngClass]="{ 'image-block': !small }">
      <img
        [src]="profileImage"
        alt=""
        [class]="small ? 'w-16 h-16' : 'w-32 h-32'"
        class="rounded-full object-cover profile-image"
        onerror="this.src= 'assets/images/no-photo.png'"
      />
      <div *ngIf="!small" class="w-6 h-6 rounded-full flex justify-center items-center circle">
        <div
          *ngIf="rolebadge != 3 && rolebadge != 2"
          class="w-4 h-4 rounded-full circle-inner"
        ></div>
        <div *ngIf="rolebadge === 3" class="alumni-hat-icon-container">
          <img src="assets/images/graduation-hat.svg" class="w-6 h-6 hat-icon" />
        </div>
        <div *ngIf="rolebadge === 2" class="applicant-book-icon-container">
          <img src="assets/images/open-book.svg" class="w-6 h-6 book-icon " />
        </div>
      </div>
    </div>
  `,
  styles: [
    `
      .circle {
        background: white;
        position: relative;
        left: 80px;
        bottom: 24px;
      }

      .circle-inner {
        background: #51d88a;
      }

      .image-block {
        height: 112px;
      }

      .profile-image {
        max-width: unset;
      }
      .alumni-hat-icon-container {
        background: #e89600;
        border-radius: 50%;
        padding: 3px;
      }

      .alumni-hat-icon-container > .hat-icon {
        max-width: unset;
        filter: invert(100%) sepia(0%) saturate(0%) hue-rotate(224deg) brightness(220%)
          contrast(200%);
      }

      .applicant-book-icon-container {
        background: #0071be;
        border-radius: 50%;
        padding: 5px;
      }

      .applicant-book-icon-container > .book-icon {
        max-width: unset;
        filter: invert(100%) sepia(0%) saturate(0%) hue-rotate(224deg) brightness(220%)
          contrast(200%);
      }
    `,
  ],
})
export class ProfileImageComponent implements OnInit {
  @Input() small = false
  @Input() profileImage: string
  @Input() rolebadge: any = 0
  ngOnInit(): void {
    if (!this.small) {
      const profileInfo = JSON.parse(localStorage.getItem('profile_info'))
      this.profileImage = profileInfo.profilePictureUrl
    }
  }
}
