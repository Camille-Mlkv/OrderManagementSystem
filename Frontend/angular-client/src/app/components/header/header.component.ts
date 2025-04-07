import { Component, OnInit } from '@angular/core';
import { AuthService } from '../../services/auth.service';
import { CommonModule } from '@angular/common';
import { Router, RouterModule } from '@angular/router';

@Component({
  selector: 'app-header',
  imports: [CommonModule, RouterModule],
  templateUrl: './header.component.html',
  styleUrl: './header.component.css'
})
export class HeaderComponent implements OnInit {

  public isUserAuthenticated: boolean = false;
  public userName: string | null = null;

  constructor(
    private authService: AuthService,
    private router: Router) { }

  ngOnInit(): void {
    this.isUserAuthenticated = this.authService.isAuthenticated();
    if (this.isUserAuthenticated) {
      this.userName = this.authService.getClaim<string>('name');
    }

    this.authService.authChanged.subscribe(res => {
      this.isUserAuthenticated = res;
      if (res) {
        this.userName = this.authService.getClaim<string>('name');
      } else {
        this.userName = null;
      }
    });
  }

  onLogout(): void {
    this.authService.logout(); 
    this.authService.sendAuthStateChangeNotification(false);
    this.router.navigate(['/sign-in']);
  }
}
