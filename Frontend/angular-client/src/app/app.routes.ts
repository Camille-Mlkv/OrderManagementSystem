import { Routes } from '@angular/router';
import { SignUpPageComponent } from './pages/sign-up-page/sign-up-page.component';
import { HomePageComponent } from './pages/home-page/home-page.component';
import { SignInPageComponent } from './pages/sign-in-page/sign-in-page.component';
import { EmailConfirmationPageComponent } from './pages/email-confirmation-page/email-confirmation-page.component';
import { ForgotPasswordPageComponent } from './pages/forgot-password-page/forgot-password-page.component';
import { PasswordResetConfirmationPageComponent } from './pages/password-reset-confirmation-page/password-reset-confirmation-page.component';
import { ResetPasswordPageComponent } from './pages/reset-password-page/reset-password-page.component';
import { RoleGuard } from './auth/role.guard';
import { Role } from './models/role';
import { CuisinesPageComponent } from './pages/cuisines-page/cuisines-page.component';
import { MealsPageComponent } from './pages/meals-page/meals-page.component';
import { MealInfoPageComponent } from './pages/meal-info-page/meal-info-page.component';
import { CreateMealPageComponent } from './pages/admin/create-meal-page/create-meal-page.component';
import { MealsTablePageComponent } from './pages/admin/meals-table-page/meals-table-page.component';
import { UpdateMealPageComponent } from './pages/admin/update-meal-page/update-meal-page.component';

export const routes: Routes = [
    {path: "", component: HomePageComponent, pathMatch:"full"},
    {path: "sign-up", component: SignUpPageComponent},
    {path: "sign-in", component: SignInPageComponent},
    {path: "email-confirmed", component: EmailConfirmationPageComponent},
    {path: "forgot-password", component: ForgotPasswordPageComponent},
    {path: "reset-password", component: ResetPasswordPageComponent},
    {path: "password-reset-confirmation", component: PasswordResetConfirmationPageComponent},
    {path: "cuisines", component: CuisinesPageComponent},
    {path: "meals/:cuisineId",component: MealsPageComponent},
    {path: "meal/:id", component: MealInfoPageComponent},
    {path: "meals-table", component: MealsTablePageComponent, canActivate: [RoleGuard], data: { roles: [Role.Admin]}},
    {path: "create-meal", component: CreateMealPageComponent, canActivate: [RoleGuard], data: { roles: [Role.Admin]}},
    {path: "update-meal/:id", component: UpdateMealPageComponent, canActivate: [RoleGuard], data: { roles: [Role.Admin]}},
];
