import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { Category } from '../models/category';
import { apiEndpoint } from '../constants/api';

@Injectable({
  providedIn: 'root'
})
export class CategoryService {  
    constructor(private http: HttpClient) {}
  
    getCategories(): Observable<Category[]> {
      return this.http.get<Category[]>(apiEndpoint.TodoEndpoint.Categories);
    }
  
    addCategory(category: Category): Observable<Category> {
      return this.http.post<Category>(apiEndpoint.TodoEndpoint.Categories, category);
    }
  
    updateCategory(category: Category): Observable<Category> {
      return this.http.put<Category>(`${apiEndpoint.TodoEndpoint.Categories}`, category);
    }
  
    deleteCategory(categoryId: number): Observable<void> {
      return this.http.delete<void>(`${apiEndpoint.TodoEndpoint.Categories}/${categoryId}`);
    }
  }