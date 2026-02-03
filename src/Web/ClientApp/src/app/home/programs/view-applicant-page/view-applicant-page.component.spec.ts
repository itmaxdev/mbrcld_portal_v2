import { ComponentFixture, TestBed } from '@angular/core/testing'

import { ViewApplicantPageComponent } from './view-applicant-page.component'

describe('ViewApplicantPageComponent', () => {
  let component: ViewApplicantPageComponent
  let fixture: ComponentFixture<ViewApplicantPageComponent>

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ViewApplicantPageComponent],
    }).compileComponents()
  })

  beforeEach(() => {
    fixture = TestBed.createComponent(ViewApplicantPageComponent)
    component = fixture.componentInstance
    fixture.detectChanges()
  })

  it('should create', () => {
    expect(component).toBeTruthy()
  })
})
