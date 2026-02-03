import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core'

@Component({
  selector: 'app-class-member-item-small',
  template: `
    <div
      (click)="select()"
      class="flex p-2 cursor-pointer bg-gray-200"
      [ngClass]="{ 'w-full': fullWidth, 'bg-gray-500': data.isActive }"
    >
      <div class="image flex items-center">
        <img
          class="w-10 h-10 rounded-full"
          [src]="data.profilePictureUrl ? data.profilePictureUrl : 'assets/images/no-photo.png'"
          alt=""
        />
      </div>
      <div class="ml-2 flex items-center">
        <p class="text-lg" style="color: #265a8c;" i18n>{{ data.name || data.fullName }}</p>
      </div>
    </div>
  `,
})
export class ClassMemberItemSmallController implements OnInit {
  @Input() data: any
  @Input() fullWidth = false
  @Output() selectApplicant: EventEmitter<string> = new EventEmitter<string>()

  constructor() {}

  ngOnInit(): void {}

  select() {
    this.selectApplicant.emit(this.data.id)
  }
}
