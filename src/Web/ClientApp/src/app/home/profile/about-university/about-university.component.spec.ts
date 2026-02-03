import { ComponentFixture, TestBed } from '@angular/core/testing'

import { AboutUniversityComponent } from './about-university.component'

describe('AboutUniversityComponent', () => {
  let component: AboutUniversityComponent
  let fixture: ComponentFixture<AboutUniversityComponent>

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [AboutUniversityComponent],
    }).compileComponents()
  })

  beforeEach(() => {
    fixture = TestBed.createComponent(AboutUniversityComponent)
    component = fixture.componentInstance
    fixture.detectChanges()
  })

  it('should create', () => {
    expect(component).toBeTruthy()
  })
})
