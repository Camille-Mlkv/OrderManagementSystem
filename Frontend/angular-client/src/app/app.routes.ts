import { Routes } from '@angular/router';

export const routes: Routes = [
    {
        path: '',
        pathMatch: 'full',
        loadComponent: () =>{
            return import('./pages/home-page/home-page.component').then(
                m => m.HomePageComponent
            )
        },
    },
    {
        path: 'sign-up',
        loadComponent: () =>{
            return import('./pages/sign-up-page/sign-up-page.component').then(
                m => m.SignUpPageComponent
            )
        },
    },
    {
        path: 'sign-in',
        loadComponent: () =>{
            return import('./pages/sign-in-page/sign-in-page.component').then(
                m => m.SignInPageComponent
            )
        },
    },
    {
        path: 'email-confirmed',
        loadComponent: () =>{
            return import('./pages/email-confirmation-page/email-confirmation-page.component').then(
                m => m.EmailConfirmationPageComponent
            )
        },
    },

];
