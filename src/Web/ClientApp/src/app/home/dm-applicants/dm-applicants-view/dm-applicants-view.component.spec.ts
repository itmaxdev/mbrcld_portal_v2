import { ComponentFixture, TestBed } from '@angular/core/testing'

import { DmApplicantsViewComponent } from './dm-applicants-view.component'

describe('DmApplicantsViewComponent', () => {
  let component: DmApplicantsViewComponent
  let fixture: ComponentFixture<DmApplicantsViewComponent>

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [DmApplicantsViewComponent],
    }).compileComponents()
  })

  beforeEach(() => {
    fixture = TestBed.createComponent(DmApplicantsViewComponent)
    component = fixture.componentInstance
    fixture.detectChanges()
  })

  it('should create', () => {
    expect(component).toBeTruthy()
  })
})
