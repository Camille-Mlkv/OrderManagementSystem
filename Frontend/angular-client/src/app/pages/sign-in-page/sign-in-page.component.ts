import { SignInRequest } from '../../models/signin-request';
import { CommonModule } from '@angular/common';
import { Component } from '@angular/core';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { emailValidator } from '../../utilities/validators/email.validator';
import { AuthService } from '../../services/auth.service';
import { ToastrService } from 'ngx-toastr';
import { Router } from '@angular/router';

@Component({
  selector: 'app-sign-in-page',
  imports: [ReactiveFormsModule, CommonModule],
  templateUrl: './sign-in-page.component.html',
  styles: ``
})
export class SignInPageComponent {
  form: FormGroup;
  isSubmitted: boolean = false;
  showPassword: boolean = false;

  constructor(
    private fb: FormBuilder, 
    private authService: AuthService,
    private toastr: ToastrService,
    private router: Router) {
    this.form = this.fb.group({
      email: ['', [Validators.required, emailValidator]],
      password: ['', [Validators.required, Validators.minLength(8)]]
    });
  }

  onSubmit(): void {
    this.isSubmitted = true;
    if (this.form.valid) {
      const credentials: SignInRequest = this.form.value;
      this.signIn(credentials);
    }
  }

  private signIn(credentials: SignInRequest): void {
    this.authService.signIn(credentials).subscribe({
      next: () => {
        this.toastr.success('Sign in successful');
      },
      error: (error) => {
        const errorMessage = error.error?.Message || 'An unexpected error occurred';
        const errorDetails = error.error?.Details || 'No additional details available'; 
        this.toastr.error(`${errorMessage}: ${errorDetails}`, 'Registration Failed');
      },
      complete: () => {
        console.log('Sign in process completed');
      }
    });
  }

  hasDisplayableError(controlName: string): Boolean {
    const control = this.form.get(controlName);
    return !!control && control.invalid && this.isSubmitted;
  }
}
