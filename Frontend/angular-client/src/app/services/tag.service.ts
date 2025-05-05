import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { Tag } from '../models/meal/tag';
import { environment } from '../../environments/environment';


@Injectable({
  providedIn: 'root'
})
export class TagService {
  private baseURL = `${environment.apiUrl}/tags`;

  constructor(private httpClient: HttpClient) { }

  getTags(): Observable<Tag[]> {
    return this.httpClient.get<Tag[]>(this.baseURL);
  }
}
