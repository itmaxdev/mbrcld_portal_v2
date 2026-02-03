import * as moment from 'moment'
import { Component, OnInit } from '@angular/core'
import {
  ListModulesByEliteClubIdViewModel,
  ModulesClient,
} from 'src/app/shared/api.generated.clients'

@Component({
  selector: 'app-elite-communication',
  templateUrl: './elite-communication.component.html',
  styleUrls: ['./elite-communication.component.scss'],
})
export class EliteCommunicationComponent implements OnInit {
  modules: Array<ListModulesByEliteClubIdViewModel>
  constructor(private modulesService: ModulesClient) {}

  ngOnInit(): void {
    const eliteclubId = localStorage.getItem('eliteclubId')
    if (eliteclubId) {
      this.modulesService.eliteClubModules(eliteclubId).subscribe((data) => {
        if (data) {
          data.forEach(
            (item) =>
              (item.startDate = moment(item.startDate).lang('en').format('[Start] DD/MM/YY') as any)
          )
          this.modules = data
        }
      })
    }
  }
}
