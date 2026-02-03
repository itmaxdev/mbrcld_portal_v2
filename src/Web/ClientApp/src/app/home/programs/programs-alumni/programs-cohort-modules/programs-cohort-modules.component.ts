import { Component, OnInit } from '@angular/core'
import {
  ListMaterialsByModuleIdViewModel,
  MaterialsClient,
} from 'src/app/shared/api.generated.clients'
import { ActivatedRoute } from '@angular/router'
import { SectionDataService } from 'src/app/shared/services/section-data.service'

@Component({
  selector: 'app-programs-cohort-modules',
  templateUrl: './programs-cohort-modules.component.html',
})
export class ProgramsCohortModulesComponent implements OnInit {
  materialsData: Array<ListMaterialsByModuleIdViewModel> = []
  modulesId = ''
  constructor(
    private materialsService: MaterialsClient,
    private section: SectionDataService,
    private route: ActivatedRoute
  ) {}

  goBack() {
    this.section.redirectBack(2)
  }

  ngOnInit() {
    this.modulesId = this.route.snapshot.paramMap.get('modulesId')
    this.materialsService.cohortMaterials(this.modulesId).subscribe((data) => {
      this.materialsData = data
    })
  }
}
