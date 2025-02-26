import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { UserService } from '../../../Services/user.service';
import { catchError } from 'rxjs/operators';
import { of } from 'rxjs';
import { Router } from '@angular/router';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrl: './register.component.css'
})
export class RegisterComponent implements OnInit {

  loginForm!: FormGroup;
  registrationStatus: { success: boolean; message: string } | null = null;
  emailError: string | null = null; 

  constructor(private fb: FormBuilder, private userService: UserService, private router: Router) {}

  ngOnInit(): void {
    this.loginForm = this.fb.group({
      name: ['', Validators.required],
      email: ['', [Validators.required, Validators.email]],
      password: ['', Validators.required]
    });
  }

  onSubmit() {
    if (this.loginForm.valid) {
      console.log(`Formulário válido!`, this.loginForm.value);

      this.emailError = null;

      this.userService.register(this.loginForm.value).pipe(
        catchError(error => {
          if (error.status === 409) {
            this.emailError = error.error.message;  
          } else {
            this.showStatusMessage(false, 'Erro desconhecido, tente novamente mais tarde.');
          }
          return of(null); 
        })
      ).subscribe(response => {
        if (response) {
          this.showStatusMessage(true, 'Registro realizado com sucesso!');
        }
      });
    } else {
      console.log("Formulário Inválido");
    }
  }

  showStatusMessage(success: boolean, message: string) {
    this.registrationStatus = { success, message };

    setTimeout(() => {
      this.registrationStatus = null;
      this.router.navigate(['login'])
    }, 3000);
  }
}
