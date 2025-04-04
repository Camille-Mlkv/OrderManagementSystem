import { Component, OnInit } from '@angular/core';
import { AbstractControl, FormBuilder, FormGroup, ReactiveFormsModule, ValidatorFn, Validators } from '@angular/forms';
import { AuthService } from '../../services/auth.service';
import { ToastrService } from 'ngx-toastr';
import { CommonModule } from '@angular/common';
import { AccountService } from '../../services/account.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-sign-up-page',
  imports: [ReactiveFormsModule, CommonModule],
  templateUrl: './sign-up-page.component.html',
  styleUrl: './sign-up-page.component.css'
})
export class SignUpPageComponent implements OnInit {
   form: FormGroup;
   errors: any = {};
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

  constructor(private formBuilder: FormBuilder, 
    private authService: AuthService,
    private accountService: AccountService, 
    private toastr: ToastrService,
    private router: Router)
  {
    this.form = this.formBuilder.group({
      name: ['', Validators.required],
      email: ['', [Validators.required, Validators.email]],
      phoneNumber: ['', [Validators.required, Validators.pattern(/^\+?\d{10,15}$/)]],
      password: ['', [Validators.required]],
      role: ['', Validators.required]
    });
  }

  onSubmit() {
    if (this.form.valid) {
      const userData = this.form.value;
      this.registerUser(userData);
    }
  }

  registerUser(userData: any): void {
    this.authService.createUser(userData).subscribe({
      next: (result) => {
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

  sendConfirmationEmail(email: string): void {
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
    return Boolean(control?.invalid) &&
      (this.isSubmitted || Boolean(control?.touched)|| Boolean(control?.dirty))
  }
  
}
