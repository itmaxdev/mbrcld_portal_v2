import { Component, OnInit } from '@angular/core'
import { Location } from '@angular/common'
import { ActivatedRoute } from '@angular/router'
import { ListProjectIdeasViewModel, ProjectIdeasClient } from 'src/app/shared/api.generated.clients'
import * as moment from 'moment'

@Component({
  selector: 'app-project-view-by-id',
  template: ` <div *ngIf="ready; else loading">
      <div class="inline-flex items-center mb-8 cursor-pointer contents" (click)="goBack()">
        <img src="assets/images/ico-arrow-left.svg" class="w-6 h-6" alt="" />
        <p class="text-lg ml-2 text-gray-700" i18n>
          Back to Ideas
        </p>
      </div>
      <div
        class="m-0 p-8 border border-gray-400 rounded-t-lg flex justify-center items-center bg-white"
      >
        <img
          _ngcontent-ohf-c126=""
          src="assets/images/ico-project-ide-black.svg"
          class="w-8 h-8"
          alt=""
        />
        <h1 class="text-3xl ml-4">{{ idea.name }}</h1>
      </div>
      <div class="m-0 p-8 border border-t-0 border-gray-400 rounded-b-lg bg-white">
        <div class="p-fluid w-full">
          <div class="text-xl text-black" i18n>Written by</div>
          <p class="text-xl text-gray-600">{{ idea.alumniName }}</p>
        </div>
        <hr class="my-3" />
        <div class="p-fluid w-full">
          <div class="text-xl text-black" i18n>Description</div>
          <p class="text-xl text-gray-600">{{ idea.description }}</p>
        </div>
        <hr class="my-3" />
        <div class="p-fluid w-full">
          <div class="text-xl text-black" i18n>Idea</div>
          <p class="text-xl text-gray-600">{{ idea.body }}</p>
        </div>
      </div>
      <div class="flex justify-center my-3">
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
  public idea: ListProjectIdeasViewModel
  public ready = false

  constructor(
    private projectIdeas: ProjectIdeasClient,
    private route: ActivatedRoute,
    private location: Location
  ) {}

  ngOnInit() {
    this.ideaId = this.route.snapshot.paramMap.get('ideaId')
    if (this.ideaId) {
      this.projectIdeas.projectIdeasGetById(this.ideaId).subscribe((data) => {
        this.idea = data
        this.ready = true
      })
    }
  }

  goBack() {
    this.location.back()
  }
}
