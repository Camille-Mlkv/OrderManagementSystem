import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { AuthService } from '../../services/auth.service';
import { ToastrService } from 'ngx-toastr';
import { CommonModule } from '@angular/common';
import { AccountService } from '../../services/account.service';
import { emailValidator } from '../../utilities/validators/email.validator';
import { phoneValidator } from '../../utilities/validators/phone.validator';
import { SignUpRequest } from '../../models/signup-request.model';

@Component({
  selector: 'app-sign-up-page',
  imports: [ReactiveFormsModule, CommonModule],
  templateUrl: './sign-up-page.component.html',
  styles: ``
})
export class SignUpPageComponent implements OnInit {
   form: FormGroup;
   roles: any = [];
   isSubmitted: boolean = false;
   showPassword: boolean = false;

  ngOnInit(): void {
    this.loadRoles();
  }
    
  loadRoles() {
    this.authService.retrieveRoles().subscribe(roles => {
      this.roles = roles;
    });
  }

  constructor(
    private formBuilder: FormBuilder, 
    private authService: AuthService,
    private accountService: AccountService, 
    private toastr: ToastrService)
    {
      this.form = this.buildForm();
    }

  private buildForm(): FormGroup {
    return this.formBuilder.group({
      name: ['', Validators.required],
      email: ['', [Validators.required, emailValidator]],
      phoneNumber: ['', [Validators.required, phoneValidator]],
      password: ['', [Validators.required, Validators.minLength(8)]],
      role: ['', Validators.required]
    });
  }

  onSubmit() {
    this.isSubmitted = true;
    if (this.form.valid) {
      const userData = this.form.value;
      this.registerUser(userData);
    }
  }

  private registerUser(userData: SignUpRequest): void {
    this.authService.createUser(userData).subscribe({
      next: () => {
        this.toastr.success('Registration successful');
        this.sendConfirmationEmail(userData.email);
      },
      error: (error) => {
        const errorMessage = error.error?.Message || 'An unexpected error occurred';
        const errorDetails = error.error?.Details || 'No additional details available'; 
        this.toastr.error(`${errorMessage}: ${errorDetails}`, 'Registration Failed');
      },
      complete: () => {
        console.log('Registration process completed');
      }
    });
  }

  private sendConfirmationEmail(email: string): void {
    this.accountService.sendConfirmationEmail(email).subscribe({
      next: () => {
        this.toastr.success('Confirmation email sent to your email address');
      },
      error: () => {
        this.toastr.error('Failed to send confirmation email', 'Email Error');
      }
    });
  }

  hasDisplayableError(controlName: string): Boolean {
    const control = this.form.get(controlName);
    return !!control && control.invalid && this.isSubmitted;
  }
  
}
