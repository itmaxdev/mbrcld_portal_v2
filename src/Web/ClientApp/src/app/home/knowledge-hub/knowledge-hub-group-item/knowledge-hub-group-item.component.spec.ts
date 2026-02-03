import { ComponentFixture, TestBed } from '@angular/core/testing'

import { KnowledgeHubGroupItemComponent } from './knowledge-hub-group-item.component'

describe('KnowledgeHubGroupItemComponent', () => {
  let component: KnowledgeHubGroupItemComponent
  let fixture: ComponentFixture<KnowledgeHubGroupItemComponent>

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [KnowledgeHubGroupItemComponent],
    }).compileComponents()
  })

  beforeEach(() => {
    fixture = TestBed.createComponent(KnowledgeHubGroupItemComponent)
    component = fixture.componentInstance
    fixture.detectChanges()
  })

  it('should create', () => {
    expect(component).toBeTruthy()
  })
})
