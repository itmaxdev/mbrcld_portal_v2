import { Component, OnInit } from '@angular/core'
import { FormGroup, FormControl, Validators } from '@angular/forms'
import { Router, ActivatedRoute } from '@angular/router'
import { email, inputLanguage } from 'src/app/shared/validators'
import { IReference } from './references.interface'
import { ConfirmationService } from 'primeng/api'
import { ReferencesFacadeService } from './references-facade.service'

@Component({
  selector: 'app-references',
  templateUrl: './references.component.html',
  styleUrls: ['./references.component.scss'],
  providers: [ConfirmationService],
})
export class ReferencesComponent implements OnInit {
  referenceForm: FormGroup
  references: IReference[] = []
  editingReference: IReference = {}
  isFormSubmitting = false
  activeIndex = 1
  ready = false

  private _showReferenceDialog = false

  get showReferenceDialog() {
    return this._showReferenceDialog
  }

  set showReferenceDialog(show: boolean) {
    this._showReferenceDialog = show
    if (!show) {
      this.editingReference = {}
      this.referenceForm.reset()
    }
  }

  constructor(
    private router: Router,
    private activatedRoute: ActivatedRoute,
    private facade: ReferencesFacadeService,
    private confirmationService: ConfirmationService
  ) {}

  async ngOnInit() {
    this.buildForm()
    await this.facade.loadReferences().then((data) => (this.references = data))
    this.ready = true
  }

  private buildForm(): void {
    this.referenceForm = new FormGroup({
      fullName: new FormControl('', [Validators.required, inputLanguage('en')]),
      fullName_AR: new FormControl('', [Validators.required, inputLanguage('ar')]),
      jobTitle: new FormControl('', [Validators.required, inputLanguage('en')]),
      jobTitle_AR: new FormControl('', [Validators.required, inputLanguage('ar')]),
      organizationName: new FormControl('', [Validators.required, inputLanguage('en')]),
      organizationName_AR: new FormControl('', [Validators.required]),
      mobile: new FormControl('', [Validators.required]),
      email: new FormControl('', [Validators.required, email()]),
    } as FormModel)
  }

  async addReference() {
    //this.references.push(this.referenceForm.value)
    if (!this.referenceForm.valid || this.isFormSubmitting) {
      return
    }

    const formValue = this.referenceForm.value as FormModel

    this.isFormSubmitting = true
    try {
      const id = await this.facade.addReference(formValue)
      this.references.push({ id, ...formValue })
      this.showReferenceDialog = false
    } catch (error) {
      // EMPTY CATCH
    } finally {
      this.isFormSubmitting = false
    }
  }

  removeReference(reference: IReference): void {
    this.confirmationService.confirm({
      message: $localize`Are you sure?`,
      accept: async () => {
        await this.facade.removeReference(reference.id)
        this.references.splice(this.references.indexOf(reference), 1)
      },
    })
  }

  async updateReference() {
    this.isFormSubmitting = true
    const formValues = this.referenceForm.value

    await this.facade.updateReference(this.editingReference.id, formValues)
    Object.assign(this.editingReference, formValues)

    this.showReferenceDialog = false
    this.isFormSubmitting = false
  }

  openAddReferenceDialog() {
    this._showReferenceDialog = true
  }

  openReferenceDialogForEditing(reference: IReference) {
    this.editingReference = reference
    this.referenceForm.patchValue(reference)
    this.showReferenceDialog = true
  }

  goToNextForm(): void {
    this.router.navigate(['../questions'], { relativeTo: this.activatedRoute })
  }

  goToBackForm(): void {
    this.router.navigate(['../achievements'], { relativeTo: this.activatedRoute })
  }
}

type FormModel = { [key in keyof IReference]: any }
