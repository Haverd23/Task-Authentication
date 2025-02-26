import { Component, OnInit } from '@angular/core';
import { ShowUser } from '../../../Interfaces/show-users';
import { UserService } from '../../../Services/user.service';
import { AuthService } from '../../../Services/auth.service';  
import { UserStoreService } from '../../../Services/user-store.service';  
@Component({
  selector: 'app-usuarios',
  templateUrl: './usuarios.component.html',
  styleUrls: ['./usuarios.component.css']
})
export class UsuariosComponent implements OnInit {
  public users: ShowUser[] = [];
  public selectedUser: ShowUser | null = null;
  public isDeleteModalOpen: boolean = false;
  public unique_name: string = '';  
  public role: string = '';  

  constructor(
    private userService: UserService,
    private authService: AuthService,
    private userStore: UserStoreService  
  ) {}

  ngOnInit() {
    const storedName = localStorage.getItem('unique_name');
    const storedRole = localStorage.getItem('role');
  
    if (storedName && storedRole) {
      this.unique_name = storedName;
      this.role = storedRole;
    } else {
      this.userStore.getFullNameFromStore().subscribe(val => {
        this.unique_name = val;
      });
      this.userStore.getRoleFromStore().subscribe(val => {
        this.role = val;
      });
    }

    this.loadUsers();
  }

  loadUsers() {
    this.userService.getAllUsers().subscribe(
      (res) => {
        this.users = res;
      },
      (error) => {
        console.error('Erro ao carregar usuários', error);
      }
    );
  }

  openDeleteConfirmationModal(user: ShowUser) {
    this.selectedUser = user;
    this.isDeleteModalOpen = true;
  }

  closeDeleteConfirmationModal() {
    this.isDeleteModalOpen = false;
    this.selectedUser = null;
  }

  confirmDeletion() {
    if (this.selectedUser) {
      this.userService.remove(this.selectedUser.id).subscribe(
        () => {
          this.loadUsers();
          this.closeDeleteConfirmationModal();
        },
        (error) => {
          console.error('Erro ao excluir o usuário', error);
        }
      );
    }
  }

  logout() {
    this.authService.logout();
  }
}
