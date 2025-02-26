import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { RegisterInterface } from '../Interfaces/register-interface';
import { Observable } from 'rxjs';
import { ShowUser } from '../Interfaces/show-users';

@Injectable({
  providedIn: 'root'
})
export class UserService {

  private url = "http://localhost:5125/api/User"

  constructor(private http: HttpClient) { }

  register(obj: RegisterInterface): Observable<RegisterInterface>{
    return this.http.post<RegisterInterface>(this.url, obj)
  }
  getAllUsers(): Observable<ShowUser[]>{
    return this.http.get<ShowUser[]>(`${this.url}/All-Users`)
  }
  remove(userId: number): Observable<void> {
    return this.http.delete<void>(`${this.url}/${userId}`);
  }


}
