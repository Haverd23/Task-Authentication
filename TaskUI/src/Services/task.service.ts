import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { PrivateTask } from '../Interfaces/privateTasks-interface';

@Injectable({
  providedIn: 'root'
})
export class TaskService {
  private apiUrl = "http://localhost:5125/api/Task";
  constructor(private http: HttpClient) { }

  create(obj: any): Observable<any> {
    return this.http.post<any>(`${this.apiUrl}`, obj);
    }

  getPrivateTasks(): Observable<PrivateTask[]> {
    return this.http.get<PrivateTask[]>(`${this.apiUrl}/private`);
    }

    getPublicTasks(): Observable<PrivateTask[]> {
      return this.http.get<PrivateTask[]>(`${this.apiUrl}/public`);
      }

  delete(id: number): Observable<any>{
    return this.http.delete<any>(`${this.apiUrl}/delete/${id}`);
  }

  update(id: number, update: PrivateTask): Observable<PrivateTask> {
    return this.http.put<any>(`${this.apiUrl}/update/${id}`, update); 
  }
}
