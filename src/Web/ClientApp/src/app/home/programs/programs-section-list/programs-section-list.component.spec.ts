import { ComponentFixture, TestBed } from '@angular/core/testing'

import { ProgramsSectionListComponent } from './programs-section-list.component'

describe('ProgramsSectionListComponent', () => {
  let component: ProgramsSectionListComponent
  let fixture: ComponentFixture<ProgramsSectionListComponent>

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ProgramsSectionListComponent],
    }).compileComponents()
  })

  beforeEach(() => {
    fixture = TestBed.createComponent(ProgramsSectionListComponent)
    component = fixture.componentInstance
    fixture.detectChanges()
  })

  it('should create', () => {
    expect(component).toBeTruthy()
  })
})
