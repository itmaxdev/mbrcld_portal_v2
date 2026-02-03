import { async, ComponentFixture, TestBed } from '@angular/core/testing'

import { SkillsAndInterestsComponent } from './skills-and-interests.component'

describe('SkillsAndInterestsComponent', () => {
  let component: SkillsAndInterestsComponent
  let fixture: ComponentFixture<SkillsAndInterestsComponent>

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [SkillsAndInterestsComponent],
    }).compileComponents()
  }))

  beforeEach(() => {
    fixture = TestBed.createComponent(SkillsAndInterestsComponent)
    component = fixture.componentInstance
    fixture.detectChanges()
  })

  it('should create', () => {
    expect(component).toBeTruthy()
  })
})
