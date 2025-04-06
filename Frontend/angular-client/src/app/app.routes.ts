import { Routes } from '@angular/router';
import { SignUpPageComponent } from './pages/sign-up-page/sign-up-page.component';
import { HomePageComponent } from './pages/home-page/home-page.component';
import { SignInPageComponent } from './pages/sign-in-page/sign-in-page.component';
import { EmailConfirmationPageComponent } from './pages/email-confirmation-page/email-confirmation-page.component';
import { ForgotPasswordPageComponent } from './pages/forgot-password-page/forgot-password-page.component';
import { PasswordResetConfirmationPageComponent } from './pages/password-reset-confirmation-page/password-reset-confirmation-page.component';
import { ResetPasswordPageComponent } from './pages/reset-password-page/reset-password-page.component';
import { RoleGuard } from './utilities/auth/role.guard';
import { Role } from './models/role';

export const routes: Routes = [
    {path:"", component: HomePageComponent, pathMatch:"full"},
    {path:"sign-up", component: SignUpPageComponent},
    {path:"sign-in", component: SignInPageComponent},
    {path:"email-confirmed", component: EmailConfirmationPageComponent},
    {path: "forgot-password", component: ForgotPasswordPageComponent},
    {path: "reset-password", component: ResetPasswordPageComponent},
    {path:"password-reset-confirmation", component: PasswordResetConfirmationPageComponent}
];
