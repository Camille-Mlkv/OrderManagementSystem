import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class AccountService {
  constructor(private httpClient: HttpClient) { }
  baseURL = 'https://localhost:5000/account';

  sendConfirmationEmail(userName: string): Observable<void> {
    const url = `${this.baseURL}/${userName}/confirmation-email`;
    return this.httpClient.post<void>(url, null);
  }
}
