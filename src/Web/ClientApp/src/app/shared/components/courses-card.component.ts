import { Component, Input } from '@angular/core'

@Component({
  selector: 'app-courses-card',
  template: `
    <div
      class="card border-2 border-gray-300 rounded-lg grid gap-2 transition duration-300 ease-in-out hover:border-blue-300"
      [class]="accomplishment ? 'py-4 px-6' : 'py-8 px-6'"
    >
      <div class="text">
        <p class="name-of-card text-gray-800 text-base font-semibold">{{ courseName }}</p>
      </div>
      <div *ngIf="accomplishment" class="count">
        <p class="text-3xl text-blue-600 font-semibold">{{ assignmentsCount | number }}</p>
      </div>
    </div>
  `,
  styles: [
    `
      .name-of-card {
        min-height: 45px;
      }
    `,
  ],
})
export class CoursesCardComponent {
  @Input() courseName: string
  @Input() assignmentsCount: number
  @Input() accomplishment = true
}
