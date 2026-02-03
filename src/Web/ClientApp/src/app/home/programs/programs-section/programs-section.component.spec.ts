import { ComponentFixture, TestBed } from '@angular/core/testing'

import { ProgramsSectionComponent } from './programs-section.component'

describe('ProgramsSectionComponent', () => {
  let component: ProgramsSectionComponent
  let fixture: ComponentFixture<ProgramsSectionComponent>

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ProgramsSectionComponent],
    }).compileComponents()
  })

  beforeEach(() => {
    fixture = TestBed.createComponent(ProgramsSectionComponent)
    component = fixture.componentInstance
    fixture.detectChanges()
  })

  it('should create', () => {
    expect(component).toBeTruthy()
  })
})
