import { ComponentFixture, TestBed } from '@angular/core/testing'

import { ProgramCohortViewComponent } from './program-cohort-view.component'

describe('ProgramCohortViewComponent', () => {
  let component: ProgramCohortViewComponent
  let fixture: ComponentFixture<ProgramCohortViewComponent>

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ProgramCohortViewComponent],
    }).compileComponents()
  })

  beforeEach(() => {
    fixture = TestBed.createComponent(ProgramCohortViewComponent)
    component = fixture.componentInstance
    fixture.detectChanges()
  })

  it('should create', () => {
    expect(component).toBeTruthy()
  })
})
