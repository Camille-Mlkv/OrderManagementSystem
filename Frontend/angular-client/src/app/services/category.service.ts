import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Category } from '../models/meal/category';
import { Observable } from 'rxjs';
import { environment } from '../../environments/environment';

@Injectable({
  providedIn: 'root'
})
export class CategoryService {
  private baseURL = `${environment.apiUrl}/categories`;

  constructor(private httpClient: HttpClient) { }

  getCategories(): Observable<Category[]> {
    return this.httpClient.get<Category[]>(this.baseURL);
  }

  getCategoryById(id: string): Observable<Category>{
    return this.httpClient.get<Category>(`${this.baseURL}/${id}`)
  }
}
