import { ComponentFixture, TestBed } from '@angular/core/testing'

import { ProgramsNewContentComponent } from './programs-new-content.component'

describe('ProgramsNewContentComponent', () => {
  let component: ProgramsNewContentComponent
  let fixture: ComponentFixture<ProgramsNewContentComponent>

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ProgramsNewContentComponent],
    }).compileComponents()
  })

  beforeEach(() => {
    fixture = TestBed.createComponent(ProgramsNewContentComponent)
    component = fixture.componentInstance
    fixture.detectChanges()
  })

  it('should create', () => {
    expect(component).toBeTruthy()
  })
})
