import { Component } from '@angular/core';
import { AccountService } from '../../services/account.service';
import { ToastrService } from 'ngx-toastr';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { emailValidator } from '../../utilities/validators/email.validator';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-forgot-password-page',
  imports: [ReactiveFormsModule, CommonModule],
  templateUrl: './forgot-password-page.component.html',
  styles: ``
})
export class ForgotPasswordPageComponent {
  form: FormGroup;
  isSubmitted: boolean = false;

  constructor(
    private formBuilder: FormBuilder,
    private accountService: AccountService, 
    private toastrService: ToastrService)
    {
    this.form = this.formBuilder.group({
      email: ['', [Validators.required, emailValidator]]
    });
  }

  onSubmit(){
    this.isSubmitted = true;
    if (this.form.valid) {
      const email = this.form.value.email;
      this.accountService.sendPasswordResetEmail(email).subscribe({
      next: (response) => {
        this.toastrService.success('Reset code was sent to your account.');
        console.log(response);
      },
      error: () => {
        this.toastrService.error('Failed to send password reset email', 'Email Error');
      }
    });
    }
  }

  hasDisplayableError(controlName: string): Boolean {
    const control = this.form.get(controlName);
    return !!control && control.invalid && this.isSubmitted;
  }
}
