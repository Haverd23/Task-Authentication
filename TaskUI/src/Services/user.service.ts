import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { RegisterInterface } from '../Interfaces/register-interface';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class UserService {

  private url = "http://localhost:5125/api/User"

  constructor(private http: HttpClient) { }

  register(obj: RegisterInterface): Observable<RegisterInterface>{
    return this.http.post<RegisterInterface>(this.url, obj)
  }


}
