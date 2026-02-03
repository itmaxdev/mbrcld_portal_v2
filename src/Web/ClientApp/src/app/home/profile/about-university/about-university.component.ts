import { Component, OnInit } from '@angular/core'
import { ActivatedRoute, Router } from '@angular/router'
import { tap } from 'rxjs/operators'
import { ProfileClient } from 'src/app/shared/api.generated.clients'
import { ProfileFacade } from '../common/profile-facade.service'

@Component({
  selector: 'app-about-university',
  templateUrl: './about-university.component.html',
  styleUrls: ['./about-university.component.scss'],
})
export class AboutUniversityComponent implements OnInit {
  textUniversity: string
  ready = false

  constructor(
    private router: Router,
    private activatedRoute: ActivatedRoute,
    private profile: ProfileClient,
    private facade: ProfileFacade
  ) {}

  goToNextPage(): void {
    this.facade.loadProfile().then((data) => {
      if (data.representedUniversityIntroduction != this.textUniversity) {
        this.profile
          .aboutUniversity(this.textUniversity)
          .pipe(
            tap(() => {
              this.router.navigate(['../general-information'], { relativeTo: this.activatedRoute })
            })
          )
          .toPromise()
      } else {
        this.router.navigate(['../general-information'], { relativeTo: this.activatedRoute })
      }
    })
  }

  ngOnInit(): void {
    this.facade.reloadProfile().then((data: any) => {
      this.textUniversity = data.representedUniversityIntroduction
      this.ready = true
    })
  }
}
