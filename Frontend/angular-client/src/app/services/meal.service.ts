import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Meal } from '../models/meal';
import { Observable } from 'rxjs';
import { PagedList } from '../models/paged-list';
import { MealFilterDto } from '../models/meal-filter';
import { environment } from '../../environments/environment';

@Injectable({
  providedIn: 'root'
})
export class MealService {
  private baseURL = `${environment.apiUrl}/meals`;

  constructor(private http: HttpClient) {}

  getMealsByCuisine(
    cuisineId: string,
    pageNo: number,
    pageSize: number,
  ): Observable<PagedList<Meal>> 
  {
    let params = new HttpParams()
      .set('pageNo', pageNo)
      .set('pageSize', pageSize);

    return this.http.get<PagedList<Meal>>(`${this.baseURL}/cuisine/${cuisineId}`, { params });
  }

  getFilteredMealsByCuisine(
    cuisineId: string,
    pageNo = 1,
    pageSize = 3,
    filter: MealFilterDto){

    let params = new HttpParams()
    .set('pageNo', pageNo.toString())
    .set('pageSize', pageSize.toString());

    if (filter.CategoryId) {
      params = params.set('CategoryId', filter.CategoryId);
    }
    if (filter.TagIds.length > 0) {
      params = params.set('TagIds', filter.TagIds.join(','));
    }
    if (filter.MinPrice !== null) {
      params = params.set('MinPrice', filter.MinPrice.toString());
    }
    if (filter.MaxPrice !== null) {
      params = params.set('MaxPrice', filter.MaxPrice.toString());
    }
    if (filter.MinCalories !== null) {
      params = params.set('MinCalories', filter.MinCalories.toString());
    }
    if (filter.MaxCalories !== null) {
      params = params.set('MaxCalories', filter.MaxCalories.toString());
    }

    return this.http.get<PagedList<Meal>>(`${this.baseURL}/cuisine/${cuisineId}/filtered`, { params });
  }
}
