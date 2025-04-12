import { isMainModule } from '@angular/ssr/node';
import { Component, OnInit } from '@angular/core';
import { AuthService } from '../../services/auth.service';
import { CommonModule } from '@angular/common';
import { Router, RouterModule } from '@angular/router';
import { Role } from '../../models/role';
import { AuthContext } from '../../auth/auth-context';

@Component({
  selector: 'app-header',
  imports: [CommonModule, RouterModule],
  templateUrl: './header.component.html',
  styleUrl: './header.component.css'
})
export class HeaderComponent{
  userName: string | null = null;

  constructor(
    public authContext: AuthContext,
    private authService: AuthService,
    private router: Router) { }

  onLogout(): void {
    this.authService.logout(); 
    this.authService.sendAuthStateChangeNotification(false);
    this.router.navigate(['/sign-in']);
  }
}
