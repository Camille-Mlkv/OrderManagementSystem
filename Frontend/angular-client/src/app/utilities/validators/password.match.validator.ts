import { AbstractControl, ValidatorFn } from '@angular/forms';

export const passwordMatchValidator: ValidatorFn = (control: AbstractControl): null => {
  const password = control.get('password');
  const confirmPassword = control.get('confirmPassword');

  if (!password || !confirmPassword) return null;

  const mismatch = password.value !== confirmPassword.value;

  if (mismatch) {
    confirmPassword.setErrors({ passwordMismatch: true });
  } else {
    confirmPassword.setErrors(null)
  }

  return null;
};
