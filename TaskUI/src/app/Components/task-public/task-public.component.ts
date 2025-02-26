import { Component, OnInit } from "@angular/core";
import { TaskService } from "../../../Services/task.service";
import { UserStoreService } from "../../../Services/user-store.service";
import { AuthService } from "../../../Services/auth.service";
import { FormBuilder, FormGroup, Validators } from "@angular/forms";
import { PrivateTask } from "../../../Interfaces/privateTasks-interface";
@Component({
  selector: 'app-task-public',
  templateUrl: './task-public.component.html',
  styleUrl: './task-public.component.css'
})
export class TaskPublicComponent implements OnInit{
  public publicTasks: PrivateTask[] = [];
  public unique_name: string = '';
  public role: string = '';
  public searchQuery: string = '';
  public selectedTask: PrivateTask | null = null;
  public isViewEditModalOpen: boolean = false;  
  public showFullDescription: boolean = false;
  public characterLimit: number = 20;
  public isDeleteModalOpen: boolean = false;
  constructor(
    private taskService: TaskService,
    private userStore: UserStoreService,
    private auth: AuthService
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
  
    this.loadPublicTasks();
  }

  loadPublicTasks() {
    this.taskService.getPublicTasks().subscribe(
      (res) => {
        this.publicTasks = res;
      },
      (error) => {
        console.error('Erro ao carregar as tarefas públicas:', error);
      }
    );
  }

  toggleDescription(task: PrivateTask, event: MouseEvent): void {
    event.stopPropagation();  
    task.showFullDescription = !task.showFullDescription;
  }

  isDescriptionLong(task: PrivateTask): boolean {
    return task.description.length > this.characterLimit;
  }

  filterPublicTasks() {
    if (this.searchQuery) {
      this.publicTasks = this.publicTasks.filter(task => task.name.toLowerCase().includes(this.searchQuery.toLowerCase()));
    } else {
      this.loadPublicTasks();
    }
  }

  openViewEditModal(task: PrivateTask) {
    this.selectedTask = task;
    this.isViewEditModalOpen = true;
  }

  closeViewEditModal() {
    this.isViewEditModalOpen = false;
    this.selectedTask = null;
  }

  openDeleteModal(task: PrivateTask) {
    this.selectedTask = task;
    this.isDeleteModalOpen = true;
  }

  closeDeleteModal() {
    this.isDeleteModalOpen = false;
    this.selectedTask = null;
  }

  deleteTask() {
    if (this.selectedTask) {
      this.taskService.delete(this.selectedTask.id).subscribe(
        () => {
         
          this.closeDeleteModal();
          this.loadPublicTasks();
        },
        (error) => {
          console.error('Erro ao excluir a tarefa:', error);
        }
      );
    }
  }

  logout() {
    this.auth.logout();
  }
}