import { ComponentFixture, TestBed } from '@angular/core/testing'

import { DmCalendarListComponent } from './dm-calendar-list.component'

describe('DmCalendarListComponent', () => {
  let component: DmCalendarListComponent
  let fixture: ComponentFixture<DmCalendarListComponent>

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [DmCalendarListComponent],
    }).compileComponents()
  })

  beforeEach(() => {
    fixture = TestBed.createComponent(DmCalendarListComponent)
    component = fixture.componentInstance
    fixture.detectChanges()
  })

  it('should create', () => {
    expect(component).toBeTruthy()
  })
})
