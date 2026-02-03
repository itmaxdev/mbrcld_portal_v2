import { Component, OnDestroy, OnInit } from '@angular/core'
import { Router, NavigationEnd } from '@angular/router'
import { Subject } from 'rxjs'
import { takeUntil } from 'rxjs/operators'
import { EliteClubClient } from 'src/app/shared/api.generated.clients'
import { ProfileFacade } from '../profile/common/profile-facade.service'
import { NavBarService } from './nav-bar.service'

interface KeyValuePair {
  key?: string
  value?: boolean
}

interface NavLink {
  title: string
  icon: string
  route: string
  isVisibleForInstructor: boolean
  isVisibleForApplicant: boolean
  isVisibleForRegistrant: boolean
  isVisibleForAlumni: boolean
}

@Component({
  selector: 'app-nav-bar',
  templateUrl: './nav-bar.component.html',
  styleUrls: ['./nav-bar.component.scss'],
})
export class NavBarComponent implements OnDestroy, OnInit {
  urlFragments: string[] = []
  role: number
  isActiveEliteclub: boolean

  navLinks: NavLink[] = [
    {
      title: $localize`General Information`,
      icon: 'ico-profile.svg',
      route: 'general-information',
      isVisibleForInstructor: true,
      isVisibleForApplicant: true,
      isVisibleForRegistrant: true,
      isVisibleForAlumni: true,
    },
    {
      title: $localize`Team`,
      icon: 'ico-team.svg',
      route: 'team',
      isVisibleForInstructor: true,
      isVisibleForApplicant: false,
      isVisibleForRegistrant: false,
      isVisibleForAlumni: false,
    },
    {
      title: $localize`Contact Details`,
      icon: 'ico-contact.svg',
      route: 'contact-details',
      isVisibleForInstructor: false,
      isVisibleForApplicant: true,
      isVisibleForRegistrant: true,
      isVisibleForAlumni: true,
    },
    {
      route: 'identity',
      icon: 'ico-document.svg',
      title: $localize`Identity`,
      isVisibleForInstructor: false,
      isVisibleForApplicant: true,
      isVisibleForRegistrant: true,
      isVisibleForAlumni: true,
    },
    {
      route: 'professional-experience',
      icon: 'ico-professional.svg',
      title: $localize`Professional Experience`,
      isVisibleForInstructor: false,
      isVisibleForApplicant: true,
      isVisibleForRegistrant: true,
      isVisibleForAlumni: true,
    },
    {
      route: 'education',
      icon: 'ico-education.svg',
      title: $localize`Education`,
      isVisibleForInstructor: false,
      isVisibleForApplicant: true,
      isVisibleForRegistrant: true,
      isVisibleForAlumni: true,
    },
    {
      route: 'training',
      icon: 'ico-training.svg',
      title: $localize`Training Courses`,
      isVisibleForInstructor: false,
      isVisibleForApplicant: true,
      isVisibleForRegistrant: true,
      isVisibleForAlumni: true,
    },
    {
      route: 'memberships',
      icon: 'ico-membership.svg',
      title: $localize`Memberships`,
      isVisibleForInstructor: false,
      isVisibleForApplicant: true,
      isVisibleForRegistrant: true,
      isVisibleForAlumni: true,
    },
    {
      route: 'achievements',
      icon: 'ico-achievement.svg',
      title: $localize`Achievements`,
      isVisibleForInstructor: false,
      isVisibleForApplicant: true,
      isVisibleForRegistrant: true,
      isVisibleForAlumni: true,
    },
    {
      route: 'learning-preferences',
      icon: 'ico-learning.svg',
      title: $localize`Learning Preferences`,
      isVisibleForInstructor: false,
      isVisibleForApplicant: true,
      isVisibleForRegistrant: true,
      isVisibleForAlumni: true,
    },
    {
      route: 'languages',
      icon: 'ico-language.svg',
      title: $localize`Languages`,
      isVisibleForInstructor: false,
      isVisibleForApplicant: true,
      isVisibleForRegistrant: true,
      isVisibleForAlumni: true,
    },
    {
      route: 'skills',
      icon: 'ico-skills.svg',
      title: $localize`Skills & Hobbies`,
      isVisibleForInstructor: false,
      isVisibleForApplicant: true,
      isVisibleForRegistrant: true,
      isVisibleForAlumni: true,
    },
  ]

  private sectionStatuses: KeyValuePair[] = []
  private destroy$ = new Subject<boolean>()

  private _visibleOnMobile = false

  get visibleOnMobile(): boolean {
    return this._visibleOnMobile
  }

  constructor(
    router: Router,
    private navBarService: NavBarService,
    private profileFacade: ProfileFacade,
    private elitclubService: EliteClubClient
  ) {
    router.events.pipe(takeUntil(this.destroy$)).subscribe((event) => {
      if (event instanceof NavigationEnd) {
        this.urlFragments = event.url.split('/').filter((x) => !!x)
      }
    })

    navBarService.visibilityChange.pipe(takeUntil(this.destroy$)).subscribe((visible) => {
      this._visibleOnMobile = visible
    })
  }

  ngOnInit() {
    this.profileFacade.loadFormProgress()
    this.profileFacade.formChanges.pipe(takeUntil(this.destroy$)).subscribe((e) => {
      this.sectionStatuses = e.sections
    })
    const { role } = JSON.parse(localStorage.getItem('profile_info'))
    this.role = role

    if (this.role == 4) {
      this.navLinks = this.navLinks.filter((item) => item.isVisibleForInstructor)
    } else if (this.role == 3) {
      this.navLinks = this.navLinks.filter((item) => item.isVisibleForAlumni)
    } else if (this.role == 2) {
      this.navLinks = this.navLinks.filter((item) => item.isVisibleForApplicant)
    } else if (this.role == 1) {
      this.navLinks = this.navLinks.filter((item) => item.isVisibleForRegistrant)
    }

    this.elitclubService.eliteclub().subscribe((data) => {
      if (data) {
        const { id, isActiveMember, name } = data
        if (id && isActiveMember && name) {
          this.isActiveEliteclub = true
          localStorage.setItem('eliteclubId', id)
        }
      }
    })
  }

  ngOnDestroy() {
    this.destroy$.next(true)
  }

  isSectionIncomplete(sectionIdentifier): boolean {
    const sectionStatus = this.sectionStatuses.find((x) => x.key === sectionIdentifier)
    return sectionStatus && sectionStatus.value === false
  }

  close() {
    this.navBarService.close()
  }
}
