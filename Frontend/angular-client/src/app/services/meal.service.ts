import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Meal } from '../models/meal/meal';
import { Observable } from 'rxjs';
import { PagedList } from '../models/paged-list';
import { MealFilterDto } from '../models/meal/meal-filter';
import { environment } from '../../environments/environment';
import { MealRequest } from '../models/meal/meal-request';
import { MealQueryParams } from '../models/meal/meal-query-params';

@Injectable({
  providedIn: 'root'
})
export class MealService {
  private baseURL = `${environment.apiUrl}/meals`;

  constructor(private httpClient: HttpClient) {}

  getFilteredMeals(query: MealQueryParams){
    let params = new HttpParams();

    Object.entries(query).forEach(([key, value]) => {
      if (value !== null && value !== undefined) {
        if (Array.isArray(value)) {
          if (value.length > 0) {
            params = params.set(key, value.join(','));
          }
        } else {
          params = params.set(key, value.toString());
        }
      }
    });

    return this.httpClient.get<PagedList<Meal>>(`${this.baseURL}`, { params });
  }

  getMealInfoById(id: string): Observable<Meal>{
    return this.httpClient.get<Meal>(`${this.baseURL}/${id}`);
  }

  createMeal(meal: MealRequest): Observable<any>{
    return this.httpClient.post<Meal>(this.baseURL, meal);
  }

  updateMeal(id: string, meal: MealRequest):Observable<any>{
    return this.httpClient.put<Meal>(`${this.baseURL}/${id}`, meal);
  }

}
