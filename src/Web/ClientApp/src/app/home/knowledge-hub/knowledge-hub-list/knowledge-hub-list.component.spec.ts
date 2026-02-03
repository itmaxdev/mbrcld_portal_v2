import { ComponentFixture, TestBed } from '@angular/core/testing'

import { KnowledgeHubListComponent } from './knowledge-hub-list.component'

describe('KnowledgeHubListComponent', () => {
  let component: KnowledgeHubListComponent
  let fixture: ComponentFixture<KnowledgeHubListComponent>

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [KnowledgeHubListComponent],
    }).compileComponents()
  })

  beforeEach(() => {
    fixture = TestBed.createComponent(KnowledgeHubListComponent)
    component = fixture.componentInstance
    fixture.detectChanges()
  })

  it('should create', () => {
    expect(component).toBeTruthy()
  })
})
