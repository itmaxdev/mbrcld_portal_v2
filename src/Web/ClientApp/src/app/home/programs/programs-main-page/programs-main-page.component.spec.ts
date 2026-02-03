import { ComponentFixture, TestBed } from '@angular/core/testing'

import { ProgramsMainPageComponent } from './programs-main-page.component'

describe('ProgramsMainPageComponent', () => {
  let component: ProgramsMainPageComponent
  let fixture: ComponentFixture<ProgramsMainPageComponent>

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ProgramsMainPageComponent],
    }).compileComponents()
  })

  beforeEach(() => {
    fixture = TestBed.createComponent(ProgramsMainPageComponent)
    component = fixture.componentInstance
    fixture.detectChanges()
  })

  it('should create', () => {
    expect(component).toBeTruthy()
  })
})
