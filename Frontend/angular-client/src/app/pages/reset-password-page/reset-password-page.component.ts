import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { AccountService } from '../../services/account.service';
import { CommonModule } from '@angular/common';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { passwordMatchValidator } from '../../utilities/validators/password.match.validator';
import { ResetPasswordRequest } from '../../models/reset-password-request';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-reset-password-page',
  imports: [CommonModule, ReactiveFormsModule],
  templateUrl: './reset-password-page.component.html',
  styles: ``
})
export class ResetPasswordPageComponent implements OnInit {
  form: FormGroup;
  showPassword: boolean = false;
  isSubmitted: boolean = false;

  userName!: string;
  resetCode!: string;
  newPassword: string = '';

  constructor(
    private formBuilder: FormBuilder,
    private route: ActivatedRoute,
    private accountService: AccountService,
    private toastr: ToastrService,
    private router: Router,
  ) 
  {
    this.form = this.formBuilder.group(
      {
        password: ['', [Validators.required, Validators.minLength(8)]],
        confirmPassword: ['', Validators.required]
      },
      {
        validators: [passwordMatchValidator]
      }
    );
  }

  ngOnInit(): void {
    this.route.queryParamMap.subscribe(params => {
      this.userName = params.get('userName') || '';
    });
    this.getResetCode();
  }

  getResetCode(): void {
    this.accountService.getPasswordResetCode(this.userName).subscribe({
      next: (data) => 
        {
          this.resetCode = data;
        },
      error: () =>console.log('Error retrieving reset code.')
    });
  }

  onSubmit(){
    this.isSubmitted = true;
    if (this.form.valid) {
      const resetRequest: ResetPasswordRequest = {
        email: this.userName,
        password: this.form.value.password,
        confirmPassword: this.form.value.confirmPassword,
        code: this.resetCode
      };

      this.sendResetRequest(resetRequest);
    }
  }

  sendResetRequest(data: ResetPasswordRequest) {
    this.accountService.resetPassword(data).subscribe({
      next: () => this.router.navigate(['/password-reset-confirmation']),
      error: () => this.toastr.error('Password reset failed')
    });
  }


  hasDisplayableError(controlName: string): Boolean {
    const control = this.form.get(controlName);
    return !!control && control.invalid && this.isSubmitted;
  }
}
