import { Component, OnInit } from '@angular/core'
import { Location } from '@angular/common'
import { ActivatedRoute } from '@angular/router'
import {
  ListUserSpecialProjectsViewModel,
  SpecialProjectsClient,
} from 'src/app/shared/api.generated.clients'

@Component({
  selector: 'app-project-view-by-id',
  template: ` <div class="inline-flex items-center mb-8 cursor-pointer contents" (click)="goBack()">
      <img src="assets/images/ico-arrow-left.svg" class="w-6 h-6" alt="" />
      <p class="text-lg ml-2 text-gray-700" i18n>
        Back to Ideas
      </p>
    </div>
    <div *ngIf="ready; else loading">
      <h1 class="text-3xl text-center">{{ idea.name }}</h1>
      <div>
        <p class="text-xl text-gray-600">{{ idea.alumniName }}</p>
        <p class="text-xl text-black mt-2">{{ idea.description }}</p>
        <p class="text-xl text-black mt-2">{{ idea.body }}</p>
      </div>
      <div class="flex justify-center">
        <a [href]="idea.documentUrl" download>
          <p-button label="Download Attachment" icon="pi pi-download" iconPos="right"></p-button>
        </a>
      </div>
    </div>
    <ng-template #loading>
      <app-progress-spinner></app-progress-spinner>
    </ng-template>`,
})
export class ProjectViewByIdComponent implements OnInit {
  public ideaId: string
  public idea: ListUserSpecialProjectsViewModel
  public ready = false

  constructor(
    private specialProjects: SpecialProjectsClient,
    private route: ActivatedRoute,
    private location: Location
  ) {}

  ngOnInit() {
    this.ideaId = this.route.snapshot.paramMap.get('ideaId')
    if (this.ideaId) {
      this.specialProjects.specialProjectsGet(this.ideaId).subscribe((data) => {
        this.idea = data
        this.ready = true
      })
    }
  }

  goBack() {
    this.location.back()
  }
}
