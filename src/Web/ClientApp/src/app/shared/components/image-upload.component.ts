import { Component, Input, Output, EventEmitter } from '@angular/core'

@Component({
  selector: 'app-image-upload',
  template: `
    <div class="p-field w-full">
      <label for="coverPhoto">{{ inputLabel }}</label>
      <input
        pInputText
        [id]="inputId"
        type="text"
        class="imageName text-lg"
        [placeholder]="inputPlaceholder"
        [(ngModel)]="imgName"
        [name]="inputId"
      />
      <p-fileUpload
        mode="basic"
        name="demo[]"
        [chooseLabel]="buttonLabel"
        accept="image/*"
        class="uploadBtn relative text-right"
        [auto]="true"
        (onSelect)="onUpload($event)"
      ></p-fileUpload>
    </div>
  `,
  styles: [
    `
      .imageName {
        padding-right: 40%;
        overflow: hidden;
        text-overflow: ellipsis;
      }
    `,
  ],
})
export class ImageUploadComponent {
  @Input() inputLabel: string
  @Input() inputId: string
  @Input() inputPlaceholder: string
  @Input() buttonLabel: string
  @Input() imgName: string
  @Output() onUploadImage: EventEmitter<any> = new EventEmitter<any>()

  onUpload(event) {
    for (const file of event.files) {
      this.onUploadImage.emit(file)
      this.imgName = file.name
    }
  }
}
