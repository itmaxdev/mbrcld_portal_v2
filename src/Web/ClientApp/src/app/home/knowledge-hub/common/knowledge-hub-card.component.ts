import { Component, Input } from '@angular/core'

@Component({
  selector: 'app-knowledge-hub-card',
  template: `
    <div
      class="border border-gray-300 rounded-lg cursor-pointer transition-all duration-300 hover:border-gray-600"
    >
      <a [routerLink]="groupId">
        <div class="image">
          <img class="rounded-t-lg h-64 object-cover" [src]="imageUrl" alt="" />
        </div>
        <div class="p-6">
          <p class="font-semibold">{{ groupName }}</p>
        </div>
      </a>
    </div>
  `,
})
export class KnowledgeHubCardComponent {
  @Input() imageUrl: string
  @Input() groupName: string
  @Input() groupId: string
}
