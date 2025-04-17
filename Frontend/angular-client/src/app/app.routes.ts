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
import { CartPageComponent } from './pages/client/cart-page/cart-page.component';
import { CreateOrderPageComponent } from './pages/client/create-order-page/create-order-page.component';
import { PayOrCancelOrderPageComponent } from './pages/client/pay-or-cancel-order-page/pay-or-cancel-order-page.component';
import { PaymentConfirmationPageComponent } from './pages/client/payment-confirmation-page/payment-confirmation-page.component';
import { OrderListComponent } from './pages/client/orders-list-page/orders-list-page.component';
import { OpenedOrdersPageComponent } from './pages/courier/opened-orders-page/opened-orders-page.component';
import { CourierOrdersPageComponent } from './pages/courier/courier-orders-page/courier-orders-page.component';
import { OrderInfoComponent } from './components/order-info/order-info.component';

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
    {path: "cart", component: CartPageComponent, canActivate: [RoleGuard], data: { roles: [Role.Client]}},
    {path: "create-order", component: CreateOrderPageComponent, canActivate: [RoleGuard], data: { roles: [Role.Client]}},
    {path: "pay-or-cancel/:id", component: PayOrCancelOrderPageComponent, canActivate: [RoleGuard], data: { roles: [Role.Client]}},
    {path: "payment-confirmation", component: PaymentConfirmationPageComponent, canActivate: [RoleGuard], data: { roles: [Role.Client]}},
    {path: "client-orders", component: OrderListComponent, canActivate: [RoleGuard], data: { roles: [Role.Client]}},
    {path: "opened-orders", component: OpenedOrdersPageComponent, canActivate: [RoleGuard], data: { roles: [Role.Courier]}},
    {path: "courier-orders", component: CourierOrdersPageComponent, canActivate: [RoleGuard], data: { roles: [Role.Courier]}},
    {path: "order-info/:id", component: OrderInfoComponent, canActivate: [RoleGuard], data: { roles: [Role.Courier, Role.Client, Role.Admin]}},
];
