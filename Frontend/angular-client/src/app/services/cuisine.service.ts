import { Observable } from 'rxjs';
import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Cuisine } from '../models/cuisine';

@Injectable({
  providedIn: 'root'
})
export class CuisineService {
  private baseURL = 'http://localhost:5010/cuisines';

  constructor(private httpClient: HttpClient) { }

  getCuisines(): Observable<Cuisine[]> {
    return this.httpClient.get<Cuisine[]>(this.baseURL);
  }

}
