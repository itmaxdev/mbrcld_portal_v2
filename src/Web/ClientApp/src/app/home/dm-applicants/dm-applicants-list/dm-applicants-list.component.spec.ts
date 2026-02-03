import { ComponentFixture, TestBed } from '@angular/core/testing'

import { DmApplicantsListComponent } from './dm-applicants-list.component'

describe('DmApplicantsComponent', () => {
  let component: DmApplicantsListComponent
  let fixture: ComponentFixture<DmApplicantsListComponent>

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [DmApplicantsListComponent],
    }).compileComponents()
  })

  beforeEach(() => {
    fixture = TestBed.createComponent(DmApplicantsListComponent)
    component = fixture.componentInstance
    fixture.detectChanges()
  })

  it('should create', () => {
    expect(component).toBeTruthy()
  })
})
