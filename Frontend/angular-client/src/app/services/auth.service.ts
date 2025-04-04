import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  constructor(private httpClient: HttpClient) { }
  baseURL = 'https://localhost:5000/auth';

  createUser(formData:any){
    return this.httpClient.post(this.baseURL+'/signup',formData);
  }

  retrieveRoles(){
    return this.httpClient.get<string[]>(this.baseURL + '/roles');
  }
}
