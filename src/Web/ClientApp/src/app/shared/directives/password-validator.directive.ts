import {
  Directive,
  OnInit,
  ComponentFactoryResolver,
  ViewContainerRef,
  Input,
  HostListener,
  ComponentRef,
} from '@angular/core'
import { PasswordValidatorComponent } from '../components/password-validator.component'
import { FormGroupDirective } from '@angular/forms'

@Directive({
  selector: '[appPasswordValidator]',
})
export class PasswordValidatorDirective implements OnInit {
  @Input()
  formControlName: string

  private componentRef?: ComponentRef<PasswordValidatorComponent>

  constructor(
    private formGroupDirective: FormGroupDirective,
    private viewContainerRef: ViewContainerRef,
    private resolver: ComponentFactoryResolver
  ) {}

  ngOnInit(): void {
    const control = this.formGroupDirective.form.get(this.formControlName)
    const componentFactory = this.resolver.resolveComponentFactory(PasswordValidatorComponent)
    this.componentRef = this.viewContainerRef.createComponent(componentFactory)
    this.componentRef.instance.control = control
  }

  @HostListener('focus')
  onFocus() {
    if (this.componentRef) this.componentRef.instance.visible = true
  }

  @HostListener('blur')
  onBlur() {
    if (this.componentRef) this.componentRef.instance.visible = false
  }
}
