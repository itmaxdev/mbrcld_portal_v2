import { ComponentFixture, TestBed } from '@angular/core/testing'

import { DmAttendanceListComponent } from './dm-attendance-list.component'

describe('DmAttendanceComponent', () => {
  let component: DmAttendanceListComponent
  let fixture: ComponentFixture<DmAttendanceListComponent>

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [DmAttendanceListComponent],
    }).compileComponents()
  })

  beforeEach(() => {
    fixture = TestBed.createComponent(DmAttendanceListComponent)
    component = fixture.componentInstance
    fixture.detectChanges()
  })

  it('should create', () => {
    expect(component).toBeTruthy()
  })
})
