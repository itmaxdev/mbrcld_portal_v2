import { ComponentFixture, TestBed } from '@angular/core/testing'

import { ProgramsList3Component } from './programs-list3.component'

describe('ProgramsList3Component', () => {
  let component: ProgramsList3Component
  let fixture: ComponentFixture<ProgramsList3Component>

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ProgramsList3Component],
    }).compileComponents()
  })

  beforeEach(() => {
    fixture = TestBed.createComponent(ProgramsList3Component)
    component = fixture.componentInstance
    fixture.detectChanges()
  })

  it('should create', () => {
    expect(component).toBeTruthy()
  })
})
