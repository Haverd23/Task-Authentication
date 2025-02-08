import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { AuthService } from '../../../Services/auth.service';
import { UserStoreService } from '../../../Services/user-store.service';
import { Router } from '@angular/router';
import { catchError, of } from 'rxjs';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrl: './login.component.css'
})
export class LoginComponent implements OnInit {

  loginForm!: FormGroup;
  loginStatus: { success: boolean; message: string } | null = null;
  errorEmail: string | null = null; 
  errorPassword: string | null = null;
  

  constructor(private fb: FormBuilder, private auth: AuthService, private userStore: UserStoreService,
     private router: Router){}

  ngOnInit(): void {
    this.loginForm = this.fb.group({
      email: ['', [Validators.required, Validators.email]],
      password: ['', Validators.required]
    })
  }

  onLogin() {
    if (this.loginForm.valid) {
      this.errorEmail = null;
      this.errorPassword = null;
      this.auth.login(this.loginForm.value).pipe(
        catchError(error => {
          if (error.status === 404) {
            this.errorEmail = error.error.message;
          } else if (error.status === 401) {
            this.errorPassword = error.error.message;
          } else {
            this.showStatusMessage(false, 'Erro desconhecido, tente novamente mais tarde.');
          }
          return of(null);
        })
      ).subscribe(response => {
        if (response) {
          console.log('Resposta do servidor:', response);
          const token = response.token;
          const refreshToken = response.refreshToken;
          this.auth.storeToken(token);
          this.auth.storeRefreshToken(refreshToken);
          var tokenPayLoad = this.auth.decodedToken();
          
          localStorage.setItem('unique_name', tokenPayLoad.unique_name);
          localStorage.setItem('role', tokenPayLoad.role);
          
          this.userStore.setFullNameForStore(tokenPayLoad.unique_name);
          this.userStore.setRoleForStore(tokenPayLoad.role);
  
          this.showStatusMessage(true, 'Login realizado com sucesso!');
        }
      });
    } else {
      console.log("Formulário Inválido");
    }
  
  
  }

  showStatusMessage(success: boolean, message: string) {
    this.loginStatus = { success, message };
    
    setTimeout(() => {
      
      this.loginStatus = null;
      this.router.navigate(['dashboard'])


    }, 3000);
  }

}
