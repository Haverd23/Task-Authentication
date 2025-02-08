import { Component, OnInit } from "@angular/core";
import { TaskService } from "../../../Services/task.service";
import { UserStoreService } from "../../../Services/user-store.service";
import { AuthService } from "../../../Services/auth.service";
import { FormBuilder, FormGroup, Validators } from "@angular/forms";
import { PrivateTask } from "../../../Interfaces/privateTasks-interface";

@Component({
  selector: 'app-dashboard',
  templateUrl: './dashboard.component.html',
  styleUrls: ['./dashboard.component.css']
})
export class DashboardComponent implements OnInit {
  public tasks: PrivateTask[] = [];
  public taskForm: FormGroup;
  public unique_name: string = '';
  public role: string  = '';
  public isCreateModalOpen: boolean = false; 
  public isViewEditModalOpen: boolean = false; 
  public searchQuery: string = '';
  public selectedTask: PrivateTask | null = null;
  public isEditing: boolean = false;  
  public isDeleteModalOpen: boolean = false;


  constructor(
    private taskService: TaskService,
    private userStore: UserStoreService,
    private auth: AuthService,
    private fb: FormBuilder
  ) {
    this.taskForm = this.fb.group({
      name: ['', [Validators.required, Validators.minLength(3)]],
      description: ['', [Validators.required, Validators.minLength(5)]],
      isPublic: [false]
    });
  }

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
  
    this.loadPrivateTasks();
  }
  
  

  loadPrivateTasks() {
    this.taskService.getPrivateTasks().subscribe(
      (res) => {
        this.tasks = res;
      },
      (error) => {
        console.error('Erro ao carregar as tarefas privadas:', error);
      }
    );
  }

  filterTasks() {
    if (this.searchQuery) {
      this.tasks = this.tasks.filter(task => task.name.toLowerCase().includes(this.searchQuery.toLowerCase()));
    } else {
      this.loadPrivateTasks();
    }
  }

  openCreateTaskModal() {
    this.isCreateModalOpen = true;
    this.taskForm.reset({
      name: '',
      description: '',
      isPublic: false 
    });
  }

  closeCreateTaskModal() {
    this.isCreateModalOpen = false;
    this.taskForm.reset();
  }

  openViewEditModal(task: PrivateTask) {
    this.selectedTask = task;
    this.isViewEditModalOpen = true;
    this.isEditing = false; 
    this.taskForm.patchValue(task); 
  }

  closeViewEditModal() {
    this.isViewEditModalOpen = false;
    this.selectedTask = null;
    this.isEditing = false;
  }

  openEditMode() {
    this.isEditing = true;
  }

  onSubmit() {
    console.log('Form value before submit:', this.taskForm.value);
  
    if (this.isEditing && this.selectedTask) {
      const updatedTask = this.taskForm.value;
      this.taskService.update(this.selectedTask.id, updatedTask).subscribe(
        () => {
          this.loadPrivateTasks(); 
          this.closeViewEditModal(); 
        },
        (error) => {
          console.error('Erro ao atualizar a tarefa', error);
        }
      );
    } else {
      const taskData = this.taskForm.value;
      console.log("Task Data", taskData)
      this.taskService.create(taskData).subscribe(
        () => {
          this.loadPrivateTasks(); 
          this.closeCreateTaskModal(); 
        },
        (error) => {
          console.error('Erro ao criar a tarefa', error);
        }
      );
    }
  }
  onToggleChange(event: Event): void {
    const inputElement = event.target as HTMLInputElement;
    this.taskForm.get('isPublic')?.setValue(inputElement.checked);
  }
  

  deleteTask() {
    if (this.selectedTask) {
      this.taskService.delete(this.selectedTask.id).subscribe(
        () => {
          this.loadPrivateTasks();  
          this.closeDeleteConfirmationModal(); 
          this.closeViewEditModal(); 
        },
        (error) => {
          console.error('Erro ao excluir a tarefa', error);
        }
      );
    }
  }

  openDeleteConfirmationModal(): void {
    this.isDeleteModalOpen = true;
  }

  closeDeleteConfirmationModal(): void {
    this.isDeleteModalOpen = false;
  }

  confirmDeletion() {
    this.deleteTask();  
    this.closeDeleteConfirmationModal();  
  }

  logout() {
    this.auth.logout();
  }
}