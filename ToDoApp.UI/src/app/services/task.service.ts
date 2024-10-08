import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { Task } from '../models/task';
import { apiEndpoint } from '../constants/api';

@Injectable({
  providedIn: 'root'
})
export class TaskService {

  constructor(
    private http: HttpClient
  ) {}

  getTasks(pageNumber: number = 1, pageSize: number = 10, searchTerm: string, categoryId?: number): Observable<Task[]> {
    let params = new HttpParams()
      .set('pageNumber', pageNumber)
      .set('pageSize', pageSize.toString());

    if (searchTerm && searchTerm.trim() !== '') {
      params = params.set('searchTerm', searchTerm.trim());
    }

    if (categoryId !== undefined) {
      params = params.set('categoryId', categoryId.toString());
    }

    return this.http.get<Task[]>(apiEndpoint.TodoEndpoint.Tasks, { params });
  }

  getTaskById(id: string): Observable<Task> {
    return this.http.get<Task>(`${apiEndpoint.TodoEndpoint.Tasks}/${id}`);
  }

  addTask(task: Task): Observable<Task> {
    return this.http.post<Task>(apiEndpoint.TodoEndpoint.Tasks, task);
  }

  updateTask(task: Task): Observable<void> {
    return this.http.put<void>(`${apiEndpoint.TodoEndpoint.Tasks}`, task);
  }

  deleteTask(id: string): Observable<void> {
    return this.http.delete<void>(`${apiEndpoint.TodoEndpoint.Tasks}/${id}`);
  }
}
