import { ComponentFixture, TestBed } from '@angular/core/testing'

import { ProjectIdeasListComponent } from './project-ideas-list.component'

describe('ProjectIdeasListComponent', () => {
  let component: ProjectIdeasListComponent
  let fixture: ComponentFixture<ProjectIdeasListComponent>

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ProjectIdeasListComponent],
    }).compileComponents()
  })

  beforeEach(() => {
    fixture = TestBed.createComponent(ProjectIdeasListComponent)
    component = fixture.componentInstance
    fixture.detectChanges()
  })

  it('should create', () => {
    expect(component).toBeTruthy()
  })
})
