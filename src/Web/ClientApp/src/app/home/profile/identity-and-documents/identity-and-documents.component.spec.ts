import { async, ComponentFixture, TestBed } from '@angular/core/testing'

import { IdentityAndDocumentsComponent } from './identity-and-documents.component'

describe('IdentityAndDocumentsComponent', () => {
  let component: IdentityAndDocumentsComponent
  let fixture: ComponentFixture<IdentityAndDocumentsComponent>

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [IdentityAndDocumentsComponent],
    }).compileComponents()
  }))

  beforeEach(() => {
    fixture = TestBed.createComponent(IdentityAndDocumentsComponent)
    component = fixture.componentInstance
    fixture.detectChanges()
  })

  it('should create', () => {
    expect(component).toBeTruthy()
  })
})
