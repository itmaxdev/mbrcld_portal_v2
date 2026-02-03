import { Router } from '@angular/router'
import { Injectable, LOCALE_ID, Inject } from '@angular/core'

@Injectable({
  providedIn: 'root',
})
export class SectionDataService {
  editData: any
  public id: number

  public completedStatus = [
    {
      color: 'text-yellow-600',
    },
    {
      color: 'text-red-400',
    },
    {
      color: 'text-green-400',
    },
    {
      text: 'Status Is Invalid',
      color: 'text-gray-500',
    },
  ]

  constructor(private router: Router, @Inject(LOCALE_ID) public locale: string) {
    this.id = 1
  }

  checkStatus(status: number) {
    if (status == 1) {
      if (this.locale == 'en') this.completedStatus[0].text = 'Unread'
      else this.completedStatus[0].text = 'لم أبدأ بعد'
      return this.completedStatus[0]
    } else if (status == 936510000) {
      if (this.locale == 'en') this.completedStatus[1].text = 'Review'
      else this.completedStatus[1].text = 'قيد المراجعة'
      return this.completedStatus[1]
    } else if (status == 936510001) {
      if (this.locale == 'en') this.completedStatus[2].text = 'Done'
      else this.completedStatus[2].text = 'مكتمل'
      return this.completedStatus[2]
    } else {
      return this.completedStatus[3]
    }
  }

  redirectBack(backStep: number, getURL = false) {
    const routeValues = this.router.url.split('/')
    routeValues.splice(-backStep, backStep)
    const newRoute = routeValues.join('/')
    if (getURL) {
      return newRoute
    }
    this.router.navigateByUrl(newRoute)
  }

  checkInEliteClub() {
    const routeValues = this.router.url.split('/')
    let exist = false
    routeValues.forEach((element) => {
      if (element === 'eliteclub') exist = true
    })
    return exist
  }

  convertMinuteToHours(minutes: string) {
    const hours = Math.floor(parseInt(minutes) / 60)
    const mins = parseInt(minutes) % 60
    return `${hours} hours ${mins} minutes`
  }
}
