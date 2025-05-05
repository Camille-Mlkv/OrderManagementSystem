import { AbstractControl, ValidationErrors } from '@angular/forms';

export function phoneValidator(control: AbstractControl): ValidationErrors | null {
  const phoneRegex = /^\+?\d{10,15}$/;
  return phoneRegex.test(control.value) ? null : { invalidPhone: true };
}