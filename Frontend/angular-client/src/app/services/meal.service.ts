import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Meal } from '../models/meal/meal';
import { Observable } from 'rxjs';
import { PagedList } from '../models/paged-list';
import { MealFilterDto } from '../models/meal/meal-filter';
import { environment } from '../../environments/environment';
import { MealRequest } from '../models/meal/meal-request';

@Injectable({
  providedIn: 'root'
})
export class MealService {
  private baseURL = `${environment.apiUrl}/meals`;

  constructor(private httpClient: HttpClient) {}

  getFilteredMeals(
    pageNo = 1,
    pageSize = 3,
    filter: MealFilterDto){

    let params = new HttpParams()
    .set('pageNo', pageNo.toString())
    .set('pageSize', pageSize.toString());

    if(filter.IsAvailable !== null){
      params = params.set('IsAvailable', filter.IsAvailable);
    }
    if (filter.CuisineId) {
      params = params.set('CuisineId', filter.CuisineId);
    }
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
