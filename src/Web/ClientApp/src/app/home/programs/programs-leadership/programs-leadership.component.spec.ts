import { ComponentFixture, TestBed } from '@angular/core/testing'

import { ProgramsLeadershipComponent } from './programs-leadership.component'

describe('ProgramsLeadershipComponent', () => {
  let component: ProgramsLeadershipComponent
  let fixture: ComponentFixture<ProgramsLeadershipComponent>

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ProgramsLeadershipComponent],
    }).compileComponents()
  })

  beforeEach(() => {
    fixture = TestBed.createComponent(ProgramsLeadershipComponent)
    component = fixture.componentInstance
    fixture.detectChanges()
  })

  it('should create', () => {
    expect(component).toBeTruthy()
  })
})
