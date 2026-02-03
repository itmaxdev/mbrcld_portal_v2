import { ComponentFixture, TestBed } from '@angular/core/testing'

import { ProgramsMaterialsComponent } from './programs-materials.component'

describe('ProgramsMaterialsComponent', () => {
  let component: ProgramsMaterialsComponent
  let fixture: ComponentFixture<ProgramsMaterialsComponent>

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ProgramsMaterialsComponent],
    }).compileComponents()
  })

  beforeEach(() => {
    fixture = TestBed.createComponent(ProgramsMaterialsComponent)
    component = fixture.componentInstance
    fixture.detectChanges()
  })

  it('should create', () => {
    expect(component).toBeTruthy()
  })
})
