import { ComponentFixture, TestBed } from '@angular/core/testing'

import { ProgramsModulesAdminComponent } from './programs-modules-admin.component'

describe('ProgramsModulesAdminComponent', () => {
  let component: ProgramsModulesAdminComponent
  let fixture: ComponentFixture<ProgramsModulesAdminComponent>

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ProgramsModulesAdminComponent],
    }).compileComponents()
  })

  beforeEach(() => {
    fixture = TestBed.createComponent(ProgramsModulesAdminComponent)
    component = fixture.componentInstance
    fixture.detectChanges()
  })

  it('should create', () => {
    expect(component).toBeTruthy()
  })
})
