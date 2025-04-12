import { Observable } from 'rxjs';
import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Cuisine } from '../models/cuisine';
import { environment } from '../../environments/environment';

@Injectable({
  providedIn: 'root'
})
export class CuisineService {
  private baseURL = `${environment.apiUrl}/cuisines`;

  constructor(private httpClient: HttpClient) { }

  getCuisines(): Observable<Cuisine[]> {
    return this.httpClient.get<Cuisine[]>(this.baseURL);
  }

  getCuisineById(id: string): Observable<Cuisine>{
    return this.httpClient.get<Cuisine>(`${this.baseURL}/${id}`)
  }
}
