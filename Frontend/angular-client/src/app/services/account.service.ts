import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { ResetPasswordRequest } from '../models/reset-password-request';

@Injectable({
  providedIn: 'root'
})
export class AccountService {
  constructor(private httpClient: HttpClient) { }
  baseURL = 'http://localhost:5010/account';

  sendConfirmationEmail(userName: string): Observable<void> {
    const url = `${this.baseURL}/${userName}/confirmation-email`;
    return this.httpClient.post<void>(url, null);
  }

  sendPasswordResetEmail(userName: string): Observable<void>{
    const url = `${this.baseURL}/${userName}/password-email`;
    return this.httpClient.post<void>(url, null);
  }

  getPasswordResetCode(userName: string){
    const url = `${this.baseURL}/${userName}/reset-code`;
    var res = this.httpClient.get<string>(url, { responseType: 'text' as 'json' });
    return res;
  }

  resetPassword(request: ResetPasswordRequest): Observable<void> {
    const url = `${this.baseURL}/password/reset`;
    const body = {
      email: request.email,
      password: request.password,
      confirmPassword: request.confirmPassword,
      code: request.code,
    };
    return this.httpClient.post<void>(url, body);
  }
}
