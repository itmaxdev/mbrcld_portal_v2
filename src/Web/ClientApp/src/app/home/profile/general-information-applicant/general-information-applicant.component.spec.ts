import { ComponentFixture, TestBed } from '@angular/core/testing'

import { GeneralInformationApplicantComponent } from './general-information-applicant.component'

describe('GeneralInformationApplicantComponent', () => {
  let component: GeneralInformationApplicantComponent
  let fixture: ComponentFixture<GeneralInformationApplicantComponent>

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [GeneralInformationApplicantComponent],
    }).compileComponents()
  })

  beforeEach(() => {
    fixture = TestBed.createComponent(GeneralInformationApplicantComponent)
    component = fixture.componentInstance
    fixture.detectChanges()
  })

  it('should create', () => {
    expect(component).toBeTruthy()
  })
})
