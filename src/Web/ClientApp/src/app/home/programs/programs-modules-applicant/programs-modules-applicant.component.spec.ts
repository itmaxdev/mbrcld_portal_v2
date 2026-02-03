import { ComponentFixture, TestBed } from '@angular/core/testing'

import { ProgramsModulesApplicantComponent } from './programs-modules-applicant.component'

describe('ProgramsModulesApplicantComponent', () => {
  let component: ProgramsModulesApplicantComponent
  let fixture: ComponentFixture<ProgramsModulesApplicantComponent>

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ProgramsModulesApplicantComponent],
    }).compileComponents()
  })

  beforeEach(() => {
    fixture = TestBed.createComponent(ProgramsModulesApplicantComponent)
    component = fixture.componentInstance
    fixture.detectChanges()
  })

  it('should create', () => {
    expect(component).toBeTruthy()
  })
})
