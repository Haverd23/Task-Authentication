import { CanActivate, ActivatedRouteSnapshot, RouterStateSnapshot, Router } from '@angular/router';
import { Injectable } from '@angular/core';
import { AuthService } from '../Services/auth.service';
@Injectable({ providedIn: 'root' })  

export class AuthGuard implements CanActivate {
  constructor(private auth: AuthService, private router: Router) {}
  canActivate(route: ActivatedRouteSnapshot, state: RouterStateSnapshot): boolean {
    console.log('User is logged in:', this.auth.isLoggin()); 
    if (this.auth.isLoggin()) {
    
      return true;
    } else {
     
      this.router.navigate(['login']);
      return false;
    }
  }
  
    
  }

