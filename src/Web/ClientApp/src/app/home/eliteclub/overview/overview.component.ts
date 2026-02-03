import { Component, OnInit } from '@angular/core'
import { EliteClubClient } from 'src/app/shared/api.generated.clients'

@Component({
  selector: 'app-overview',
  templateUrl: './overview.component.html',
})
export class OverviewComponent implements OnInit {
  overview = ''

  constructor(private elitclubService: EliteClubClient) {}

  ngOnInit(): void {
    const eliteclubId = localStorage.getItem('eliteclubId')
    if (eliteclubId) {
      this.elitclubService.overview(eliteclubId).subscribe((data) => {
        const { overview } = data
        if (overview) {
          this.overview = overview
        }
      })
    }
  }
}
