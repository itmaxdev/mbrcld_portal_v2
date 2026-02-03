import { ComponentFixture, TestBed } from '@angular/core/testing'

import { ProgramsSubmittedProjectComponent } from './programs-submitted-project.component'

describe('ProgramsSubmittedProjectComponent', () => {
  let component: ProgramsSubmittedProjectComponent
  let fixture: ComponentFixture<ProgramsSubmittedProjectComponent>

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ProgramsSubmittedProjectComponent],
    }).compileComponents()
  })

  beforeEach(() => {
    fixture = TestBed.createComponent(ProgramsSubmittedProjectComponent)
    component = fixture.componentInstance
    fixture.detectChanges()
  })

  it('should create', () => {
    expect(component).toBeTruthy()
  })
})
