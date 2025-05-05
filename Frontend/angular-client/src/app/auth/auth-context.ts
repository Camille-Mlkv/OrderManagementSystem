import { Injectable } from "@angular/core";
import { Role } from "../models/auth/role";
import { AuthService } from "../services/auth.service";

@Injectable({ providedIn: 'root' })
export class AuthContext {
  isAuthenticated: boolean = false;
  userName: string | null = null;
  userId: string | null = null;
  role: Role | null = null;

  constructor(private authService: AuthService) {
    this.updateState();

    this.authService.authChanged.subscribe(isAuth => {
      this.updateState();
    });
  }

  private updateState(): void {
    this.isAuthenticated = this.authService.isAuthenticated();
    if (this.isAuthenticated) {
      this.userName = this.authService.getClaim<string>('name');
      this.role = this.authService.getClaim<Role>('Role');
      this.userId = this.authService.getClaim<string>('sub');
    } else {
      this.userName = null;
      this.role = null;
      this.userId = null;
    }
  }

  get isAdmin(): boolean {
    return this.role === Role.Admin;
  }

  get isClient(): boolean {
    return this.role === Role.Client;
  }

  get isCourier(): boolean {
    return this.role === Role.Courier;
  }
}
