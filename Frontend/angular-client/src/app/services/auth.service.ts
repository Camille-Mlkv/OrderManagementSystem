import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { SignUpRequest } from '../models/signup-request';
import { SignInRequest } from '../models/signin-request';
import { catchError, map, Observable, Subject, throwError } from 'rxjs';
import { SignInResponse } from '../models/sign-in-response';
import {CookieService} from 'ngx-cookie-service';
import {jwtDecode} from 'jwt-decode';
import { environment } from '../../environments/environment';

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  private baseURL = `${environment.apiUrl}/auth`;
  private authChangeSub = new Subject<boolean>()
  authChanged = this.authChangeSub.asObservable();
  
  constructor(private httpClient: HttpClient, private cookieService: CookieService) { }

  sendAuthStateChangeNotification = (isAuthenticated: boolean) => {
    this.authChangeSub.next(isAuthenticated);
  }

  signUp(credentials: SignUpRequest){
    const url = this.baseURL+'/signup';

    return this.httpClient.post(url, credentials);
  }

  signIn(credentials: SignInRequest): Observable<SignInResponse>{
    const url = this.baseURL+'/signin';

    return this.httpClient.post<SignInResponse>(url, credentials).pipe(
      map((response) => {
        if (response?.accessToken && response?.refreshToken) {
          this.cookieService.set('accessToken', response.accessToken, { expires: 7, path: '/' });
          this.cookieService.set('refreshToken', response.refreshToken, { expires: 7, path: '/' });
        }
        return response;
      })
    );
  }

  refreshAccessToken(): Observable<SignInResponse> {
    const url = this.baseURL + '/refresh';

    const refreshToken = this.cookieService.get('refreshToken');
    const accessToken = this.cookieService.get('accessToken');
  
    return this.httpClient.post<SignInResponse>(url, {
      accessToken: accessToken,
      refreshToken: refreshToken,
    })
    .pipe(
      map(response => {
        this.cookieService.set('accessToken', response.accessToken, {expires: 7, path: '/' });
        this.cookieService.set('refreshToken', response.refreshToken, { expires: 7, path: '/' });
        return response;
      }),
      catchError(error => {
        console.error('Error during token refresh:', error);
        return throwError(() => error);
      })
    );
  }

  logout(){
    this.cookieService.delete('accessToken', '/');
    this.cookieService.delete('refreshToken', '/');
  }

  retrieveRoles(){
    return this.httpClient.get<string[]>(this.baseURL + '/roles');
  }

  getClaim<T>(claimName: string): T | null {
    const token = this.cookieService.get('accessToken');
    if (!token) return null;
  
    try {
      const decoded: any = jwtDecode(token);
      return decoded[claimName] || null;
    } catch {
      return null;
    }
  }

  getTokens(): { accessToken: string | null, refreshToken: string | null } {
    const accessToken = this.cookieService.get('accessToken');
    const refreshToken = this.cookieService.get('refreshToken');
    return { accessToken, refreshToken };
  }

  isAuthenticated(): boolean {
    const { accessToken, refreshToken } = this.getTokens();
    return !!accessToken && !!refreshToken;
  }
}
