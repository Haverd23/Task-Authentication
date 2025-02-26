import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { LoginInterface } from '../Interfaces/login-interface';
import { Observable, tap } from 'rxjs';
import { Router } from '@angular/router';
import { JwtHelperService } from '@auth0/angular-jwt';
import { LoginResponse } from '../Interfaces/loginResponse-interface';

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  private url = "http://localhost:5125/api/Auth"
  private userPayload: any;

  constructor(private http: HttpClient, private router: Router) { }

  login(obj: LoginInterface): Observable<LoginResponse> {
    console.log('Login attempt with:', obj); 

    return this.http.post<LoginResponse>(`${this.url}/login`, obj).pipe(
      tap((response: LoginResponse) => {
        console.log('Login response:', response); 
  
        this.storeToken(response.token);  
        localStorage.setItem('refreshToken', response.refreshToken);  
      })
    );
  }
  refreshToken(refreshToken: string): Observable<any> {
    return this.http.post<any>(`${this.url}/refresh-token`, { RefreshToken: refreshToken });
  }

  storeToken(tokenValue: string): void{
    localStorage.setItem('token', tokenValue);
    this.userPayload = this.decodeToken(tokenValue);
    
  }

  storeRefreshToken(refreshToken: string): void{
    localStorage.setItem('refreshToken', refreshToken);
  }

  getToken(): string|null{
    return localStorage.getItem('token');
  }

  getRefreshToken(): string|null{
    return localStorage.getItem('refreshToken')
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
