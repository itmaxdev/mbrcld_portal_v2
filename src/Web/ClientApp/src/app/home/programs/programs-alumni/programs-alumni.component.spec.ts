import { ComponentFixture, TestBed } from '@angular/core/testing'

import { ProgramsAlumniComponent } from './programs-alumni.component'

describe('ProgramsAlumniComponent', () => {
  let component: ProgramsAlumniComponent
  let fixture: ComponentFixture<ProgramsAlumniComponent>

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ProgramsAlumniComponent],
    }).compileComponents()
  })

  beforeEach(() => {
    fixture = TestBed.createComponent(ProgramsAlumniComponent)
    component = fixture.componentInstance
    fixture.detectChanges()
  })

  it('should create', () => {
    expect(component).toBeTruthy()
  })
})
