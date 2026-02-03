import { ComponentFixture, TestBed } from '@angular/core/testing'

import { ProgramsCoursesComponent } from './programs-courses.component'

describe('ProgramsCoursesComponent', () => {
  let component: ProgramsCoursesComponent
  let fixture: ComponentFixture<ProgramsCoursesComponent>

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ProgramsCoursesComponent],
    }).compileComponents()
  })

  beforeEach(() => {
    fixture = TestBed.createComponent(ProgramsCoursesComponent)
    component = fixture.componentInstance
    fixture.detectChanges()
  })

  it('should create', () => {
    expect(component).toBeTruthy()
  })
})
