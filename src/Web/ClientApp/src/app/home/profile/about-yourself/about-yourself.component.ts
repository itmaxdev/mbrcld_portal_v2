import { Component, OnInit } from '@angular/core'
import { ActivatedRoute, Router } from '@angular/router'
import { tap } from 'rxjs/operators'
import { ProfileClient } from 'src/app/shared/api.generated.clients'
import { ProfileFacade } from '../common/profile-facade.service'

@Component({
  selector: 'app-about-yourself',
  templateUrl: './about-yourself.component.html',
  styleUrls: ['./about-yourself.component.scss'],
})
export class AboutYourselfComponent implements OnInit {
  textYourself: string
  ready = false

  constructor(
    private router: Router,
    private activatedRoute: ActivatedRoute,
    private profile: ProfileClient,
    private facade: ProfileFacade
  ) {}

  goToNextPage(): void {
    this.facade.loadProfile().then((data) => {
      if (data.aboutInstructor != this.textYourself) {
        this.profile
          .aboutInstructor(this.textYourself)
          .pipe(
            tap(() => {
              this.router.navigate(['../about-university'], { relativeTo: this.activatedRoute })
            })
          )
          .toPromise()
      } else {
        this.router.navigate(['../about-university'], { relativeTo: this.activatedRoute })
      }
    })
  }

  ngOnInit(): void {
    this.facade.reloadProfile().then((data: any) => {
      this.textYourself = data.aboutInstructor
      this.ready = true
    })
  }
}
