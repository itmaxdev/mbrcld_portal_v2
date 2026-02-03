import { async, ComponentFixture, TestBed } from '@angular/core/testing'

import { LearningPreferencesComponent } from './learning-preferences.component'

describe('LearningPreferencesComponent', () => {
  let component: LearningPreferencesComponent
  let fixture: ComponentFixture<LearningPreferencesComponent>

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [LearningPreferencesComponent],
    }).compileComponents()
  }))

  beforeEach(() => {
    fixture = TestBed.createComponent(LearningPreferencesComponent)
    component = fixture.componentInstance
    fixture.detectChanges()
  })

  it('should create', () => {
    expect(component).toBeTruthy()
  })
})
