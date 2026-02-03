import { Directive, Input, Host, Self, OnInit } from '@angular/core'
import { Calendar } from 'primeng/calendar'
import * as moment from 'moment'

const USE_CASES: UseCases = {
  pastDate: () => {
    const thisYear = moment().startOf('year').utc(true)
    const thisDay = moment().startOf('day').utc(true).toDate()
    const thisYearSubtract50 = moment().subtract(50, 'year').toDate().getFullYear()

    return { yearRange: `${thisYearSubtract50}:${thisYear.year()}`, maxDate: thisDay }
  },
  dateOfBirth: () => {
    const thisYearSubtractt18 = moment().subtract(18, 'year').toDate().getFullYear()

    return { yearRange: `${1950}:${thisYearSubtractt18}` }
  },
  futureDate: () => {
    const thisYear = moment().startOf('year').utc(true).toDate().getFullYear()
    const thisYearAdd50 = moment().add(50, 'year').toDate().getFullYear()

    return { yearRange: `${thisYear}:${thisYearAdd50}` }
  },
  freeDate: () => {
    const thisYearAdd50 = moment().add(50, 'year').toDate().getFullYear()

    return { yearRange: `${1950}:${thisYearAdd50}` }
  },
}

@Directive({ selector: '[appUseDateRange]' })
export class UseDateRangeDirective implements OnInit {
  @Input() appUseDateRange: DateUseCase

  constructor(@Host() @Self() private calendar: Calendar) {}

  ngOnInit(): void {
    const useCase = USE_CASES[this.appUseDateRange || 'unknown']
    if (useCase) {
      const useCaseData = useCase()

      if (useCaseData.yearRange) {
        this.calendar.yearRange = useCaseData.yearRange
        this.calendar.readonlyInput = true
      }

      if (useCaseData.maxDate) {
        this.calendar.maxDate = useCaseData.maxDate
        this.calendar.readonlyInput = true
      }

      if (useCaseData.minDate) {
        this.calendar.minDate = useCaseData.minDate
        this.calendar.readonlyInput = true
      }
    }
  }
}

type DateUseCase = 'pastDate' | 'dateOfBirth' | 'futureDate' | 'freeDate'
type UseCases = { [key in DateUseCase]: UseCaseType }
type UseCaseType = () => { yearRange?: string; maxDate?: Date }
