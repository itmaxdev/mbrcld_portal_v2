import { ComponentFixture, TestBed } from '@angular/core/testing'

import { ProgramsModulesInstructorComponent } from './programs-modules-instructor.component'

describe('ProgramsModulesInstructorComponent', () => {
  let component: ProgramsModulesInstructorComponent
  let fixture: ComponentFixture<ProgramsModulesInstructorComponent>

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ProgramsModulesInstructorComponent],
    }).compileComponents()
  })

  beforeEach(() => {
    fixture = TestBed.createComponent(ProgramsModulesInstructorComponent)
    component = fixture.componentInstance
    fixture.detectChanges()
  })

  it('should create', () => {
    expect(component).toBeTruthy()
  })
})
