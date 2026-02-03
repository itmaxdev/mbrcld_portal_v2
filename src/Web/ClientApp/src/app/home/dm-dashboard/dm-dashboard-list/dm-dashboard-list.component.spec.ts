import { ComponentFixture, TestBed } from '@angular/core/testing'

import { DmDashboardListComponent } from './dm-dashboard-list.component'

describe('DmDashboardListComponent', () => {
  let component: DmDashboardListComponent
  let fixture: ComponentFixture<DmDashboardListComponent>

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [DmDashboardListComponent],
    }).compileComponents()
  })

  beforeEach(() => {
    fixture = TestBed.createComponent(DmDashboardListComponent)
    component = fixture.componentInstance
    fixture.detectChanges()
  })

  it('should create', () => {
    expect(component).toBeTruthy()
  })
})
