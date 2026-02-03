import { ComponentFixture, TestBed } from '@angular/core/testing'

import { ProgramsModulesComponent } from './programs-modules.component'

describe('ProgramsModulesComponent', () => {
  let component: ProgramsModulesComponent
  let fixture: ComponentFixture<ProgramsModulesComponent>

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ProgramsModulesComponent],
    }).compileComponents()
  })

  beforeEach(() => {
    fixture = TestBed.createComponent(ProgramsModulesComponent)
    component = fixture.componentInstance
    fixture.detectChanges()
  })

  it('should create', () => {
    expect(component).toBeTruthy()
  })
})
