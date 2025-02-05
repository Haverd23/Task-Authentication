import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { LoginInterface } from '../Interfaces/login-interface';
import { Observable } from 'rxjs';
import { Router } from '@angular/router';
import { JwtHelperService } from '@auth0/angular-jwt';

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  private url = "http://localhost:5125/api/Auth"
  private userPayload: any;

  constructor(private http: HttpClient, private router: Router) { }

  login(obj: LoginInterface): Observable<LoginInterface>{
    return this.http.post<LoginInterface>(`${this.url}/login`, obj)
  }

  storeToken(tokenValue: string): void{
    localStorage.setItem('token', tokenValue);
    this.userPayload = this.decodeToken(tokenValue);
  }

  getToken(): string|null{
    return localStorage.getItem('token');
  }

  isLoggin(): boolean{
    return  localStorage.getItem('token') ? true :  false;
  }

  logout(): void{
    localStorage.clear();
    this.userPayload = null;
    this.router.navigate(['/login'])
    
  }

  decodedToken(){
    const token = this.getToken();
    if(token){
      const jwtHelper = new JwtHelperService();
      return jwtHelper.decodeToken(token);
    }
    return null;
  }

  decodeToken(token:string): any{
    const jwtHelper = new JwtHelperService();
    return jwtHelper.decodeToken(token);

  }
  getFullNameFromToken(): string | null {
    return this.userPayload ? this.userPayload.unique_name : null; 
  }
  getRoleFromToken(): string | null {
    return this.userPayload ? this.userPayload.role : null;  
  }

  getNameIdFromToken(): string | null {
    return this.userPayload ? this.userPayload.nameid : null;
  }
  
}
