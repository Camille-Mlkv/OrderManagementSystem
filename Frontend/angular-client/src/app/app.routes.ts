import { Routes } from '@angular/router';
import { SignUpPageComponent } from './pages/common/sign-up-page/sign-up-page.component';
import { HomePageComponent } from './pages/common/home-page/home-page.component';
import { SignInPageComponent } from './pages/common/sign-in-page/sign-in-page.component';
import { EmailConfirmationPageComponent } from './pages/common/email-confirmation-page/email-confirmation-page.component';
import { ForgotPasswordPageComponent } from './pages/common/forgot-password-page/forgot-password-page.component';
import { PasswordResetConfirmationPageComponent } from './pages/common/password-reset-confirmation-page/password-reset-confirmation-page.component';
import { ResetPasswordPageComponent } from './pages/common/reset-password-page/reset-password-page.component';
import { RoleGuard } from './auth/role.guard';
import { Role } from './models/auth/role';
import { CuisinesPageComponent } from './pages/common/cuisines-page/cuisines-page.component';
import { MealsPageComponent } from './pages/common/meals-page/meals-page.component';
import { MealInfoPageComponent } from './pages/common/meal-info-page/meal-info-page.component';
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
import { AdminOrdersPageComponent } from './pages/admin/admin-orders-page/admin-orders-page.component';
import { CouriersPageComponent } from './pages/admin/couriers-page/couriers-page.component';
import { CourierReadyOrdersPageComponent } from './pages/admin/courier-ready-orders-page/courier-ready-orders-page.component';
import { DeliveredOrdersPageComponent } from './pages/admin/delivered-orders-page/delivered-orders-page.component';

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
    {path: "admin-orders", component: AdminOrdersPageComponent, canActivate: [RoleGuard], data: { roles: [Role.Admin]}},
    {path: "couriers", component: CouriersPageComponent, canActivate: [RoleGuard], data: { roles: [Role.Admin]}},
    {path: "courier-ready-orders/:id", component: CourierReadyOrdersPageComponent, canActivate: [RoleGuard], data: { roles: [Role.Admin]}},
    {path: "delivered-orders", component: DeliveredOrdersPageComponent, canActivate: [RoleGuard], data: { roles: [Role.Admin]}},
];
