import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { RegisterComponent } from './Components/register/register.component';
import { LoginComponent } from './Components/login/login.component';
import { DashboardComponent } from './Components/dashboard/dashboard.component';
import { AuthGuard } from '../Guards/auth.guard';
import { TaskPublicComponent } from './Components/task-public/task-public.component';
import { UsuariosComponent } from './Components/usuarios/usuarios.component';
import { EstatisticasComponent } from './Components/estatisticas/estatisticas.component';

const routes: Routes = [
  {path: 'register', component: RegisterComponent},
  {path: 'login', component: LoginComponent},
  {path: 'dashboard', component: DashboardComponent,canActivate:[AuthGuard]},
  {path: '', component: LoginComponent},
  {path: 'community', component: TaskPublicComponent, canActivate:[AuthGuard]},
  {path: 'usuarios', component: UsuariosComponent, canActivate:[AuthGuard]},
  {path: 'statistics', component: EstatisticasComponent, canActivate:[AuthGuard]},


];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
