import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { TaskService } from '../../services/task.service';  // You would need a service to handle task CRUD operations
import { CategoryService } from '../../services/category.service';  // You would need a service to handle category CRUD operations
import { Task } from '../../models/task';  // Define a Task interface/model
import { Category } from '../../models/category';  // Define a Category interface/model
import { HttpClientModule } from '@angular/common/http';
import { CommonModule } from '@angular/common';
import { TokenService } from '../../services/token.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-task-list',
  templateUrl: './task-list.component.html',
  styleUrls: ['./task-list.component.scss'],
  imports: [ReactiveFormsModule, HttpClientModule, CommonModule],
  standalone: true
})
export class TaskListComponent implements OnInit {
  tasks: Task[] = [];
  categories: Category[] = [];
  taskForm: FormGroup;
  categoryForm: { [key: number]: FormGroup } = {};
  editedTaskIndex: number | null = null;
  addingNewTask: boolean = false;
  selectedCategory: number | null = null;
  currentPage: number = 1;
  totalPages: number = 1;
  tasksPerPageControl = this.fb.control(10);
  categoryEditState: {[key: number]: boolean} = {};
  categoryAddState: boolean = false;

  constructor(
    private fb: FormBuilder,
    private taskService: TaskService,
    private categoryService: CategoryService,
    private tokenService: TokenService,
    private router: Router
  ) {
    this.taskForm = this.fb.group({
      id: [''],
      title: ['', Validators.required],
      description: [''],
      dueDate: ['', Validators.required],
      categoryName: ['', Validators.required],
      categoryId:[''],
      isCompleted: [false]
    });
  }

  ngOnInit(): void {
    this.loadCategories();
    this.loadTasks();
    this.tasksPerPageControl.valueChanges.subscribe(() => {
      this.currentPage = 1;  // Reset to the first page when changing the number of tasks per page
      this.loadTasks();
    });
  }

  /**
   * Load tasks dynamically with pagination and filtering by category.
   */
  loadTasks(pageNumber: number = 1, pageSize: number = 10, searchTerm: string = ""): void {
    const categoryId = this.selectedCategory ? this.selectedCategory : undefined;
    
    this.taskService.getTasks(pageNumber, pageSize, searchTerm, categoryId).subscribe({
      next: (response: Task[]) => {
        this.tasks = response;

        this.tasks.forEach(task => {
          this.taskForm.patchValue({
            id: task.id,
            title: task.title,
            description: task.description,
            dueDate: new Date(task.dueDate).toISOString().split('T')[0],
            categoryId: task.category.id,
            categoryName: task.category.name,
            isCompleted: task.isCompleted
          });
        });
        
        this.totalPages = Math.ceil(this.tasks.length / (this.tasksPerPageControl.value ?? 10));  
      },
      error: (err) => console.error('Failed to load tasks', err)
    });
  }
  

  /**
   * Load all categories for filtering and displaying.
   */
  loadCategories(): void {
    this.categoryService.getCategories().subscribe({
      next: (categories) => {
        this.categories = categories;
  
        // Only run the forEach after categories are loaded
        this.categories.forEach(category => {
          this.categoryEditState[category.id] = false;
          console.log(this.categoryEditState[category.id]);

        this.categoryForm[category.id]=this.fb.group({
          name: [category.name, Validators.required]
        });
        });
      },
      error: (err) => console.error('Failed to load categories', err)
    });
  }
  

  /**
   * Filter tasks by the selected category.
   */
  filterTasksByCategory(categoryId: number): void {
    if(categoryId === this.selectedCategory){
      this.selectedCategory = null;
    }
    else{
      this.selectedCategory = categoryId;
    }
    this.loadTasks();
  }

  /**
   * Start editing a task.
   */
  onEditTask(index: number): void {
    this.editedTaskIndex = index;
  }

/**
   * Save the edited task.
   */
onSaveTask(): void {
  if (this.taskForm.valid) {
    const updatedTask = {
      id: this.taskForm.value.id,
      title: this.taskForm.value.title,
      description: this.taskForm.value.description,
      isCompleted: this.taskForm.value.isCompleted,
      dueDate: this.taskForm.value.dueDate,
      category: this.getCategoryById(this.taskForm.value.categoryId)
      }
    this.taskService.updateTask(updatedTask).subscribe({
      next: () => {
        this.tasks[this.editedTaskIndex!] = updatedTask;
        this.editedTaskIndex = null;  // Exit editing state
      },
      error: (err) => console.error('Failed to update task', err)
    });
  }

}


/**
 * Cancel editing a task.
 */
onCancelTaskEdit(): void {
  this.editedTaskIndex = null;
  this.taskForm.reset();  // Reset form after canceling the edit
}

  /**
   * Delete a task.
   */
  onDeleteTask(task: Task): void {
    if (confirm('Are you sure you want to delete this task?')) {
      this.taskService.deleteTask(task.id).subscribe({
        next: () => {
          this.tasks = this.tasks.filter((t) => t.id !== task.id);
        },
        error: (err) => console.error('Failed to delete task', err)
      });
    }
  }

  /**
   * Start adding a new task.
   */
  onAddTask(): void {
    this.addingNewTask = true;
    this.taskForm.reset();  // Clear the form for a new task
  }

  /**
   * Save the newly created task.
   */
  onSaveNewTask(): void {
    if (this.taskForm.valid) {
      const newTask = this.taskForm.value;
      this.taskService.addTask(newTask).subscribe({
        next: (task) => {
          this.tasks.push(task);
          this.addingNewTask = false;
          this.taskForm.reset();
        },
        error: (err) => console.error('Failed to add task', err)
      });
    }
  }

    /**
   * Add a new category.
   */
    onAddCategory(): void {
      this.categoryAddState = true;
      this.categoryForm[0] = this.fb.group({
        name: ['', Validators.required]  // Form control for new category name
      });
    }

  /**
   * Save the new category
   */
  onSaveNewCategory(): void {
    if (this.categoryForm[0] && this.categoryForm[0].valid) {
      const newCategory = {
        id: 0,
        name: this.categoryForm[0].value.name
      };

      this.categoryService.addCategory(this.categoryForm[0].value).subscribe({
        next: (category) => {
          this.categories.push(category);  // Add the new category to the list with backend-generated id
          this.categoryAddState = false;   // Hide the input field
          delete this.categoryForm[0];     // Remove the temporary form for new category
        },
        error: (err) => console.error('Failed to add category', err)
      });
    }
    this.loadCategories();
  }



  /**
   * Cancel adding a new category
   */
  onCancelNewCategory(): void {
    this.categoryAddState = false;  // Hide input and buttons
    delete this.categoryForm[0]; // Remove the temporary form for new category
  }

  /**
   * Edit a category.
   */
  onEditCategory(category: Category): void {

    this.categoryEditState[category.id]=true;
    // const updatedCategoryName = prompt('Edit category name:', category.name);
    // if (updatedCategoryName) {
    //   const updatedCategory = { ...category, name: updatedCategoryName };
    //   this.categoryService.updateCategory(updatedCategory).subscribe({
    //     next: () => {
    //       const index = this.categories.findIndex((c) => c.id === category.id);
    //       if (index !== -1) this.categories[index] = updatedCategory;
    //     },
    //     error: (err) => console.error('Failed to update category', err)
    //   });
    // }
  }



  /**
   * Save the edited category.
   */
  onSaveCategory(category: Category): void {
    if (this.categoryForm[category.id].valid) {
      // Create a new object with only the properties you want to update (id and name)
      const updatedCategory = {
        id: category.id,
        name: this.categoryForm[category.id].value.name
      };
      
      this.categoryService.updateCategory(updatedCategory).subscribe({
        next: () => {
          const index = this.categories.findIndex(c => c.id === category.id);
          if (index !== -1) {
            this.categories[index] = { ...this.categories[index], ...updatedCategory };  // Update only name locally
          }
          this.categoryEditState[category.id] = false;  // Exit edit mode
        },
        error: (err) => console.error('Failed to update category', err)
      });
    }
  }
  
  // onSaveCategory(category: Category): void {
  //   if (this.categoryForm[category.id].valid) {
  //     const updatedCategory = { ...category, name: this.categoryForm[category.id].value.name };
      
  //     this.categoryService.updateCategory(updatedCategory).subscribe({
  //       next: () => {
  //         const index = this.categories.findIndex(c => c.id === category.id);
  //         if (index !== -1) {
  //           this.categories[index] = updatedCategory;  // Update the local categories list
  //         }
  //         this.categoryEditState[category.id] = false;
  //       },
  //       error: (err) => console.error('Failed to update category', err)
  //     });
  //   }
  // }

  /**
   * Cancel category editing.
   */
  onCancelCategoryEdit(categoryId: number): void {
    this.categoryEditState[categoryId] = false;  // Exit edit mode without saving
    this.categoryForm[categoryId].reset();  // Clear the form control
  }

  /**
   * Delete a category.
   */
  onDeleteCategory(category: Category): void {
    if (confirm(`Are you sure you want to delete the category: ${category.name}?`)) {
      this.categoryService.deleteCategory(category.id).subscribe({
        next: () => {
          this.categories = this.categories.filter((c) => c.id !== category.id);
        },
        error: (err) => console.error('Failed to delete category', err)
      });
    }
  }

  /**
   * Get the category object by ID.
   */
  getCategoryById(id: number): Category {
    return this.categories.find((category) => category.id === id)!;
  }

  /**
   * Pagination: Go to the previous page.
   */
  prevPage(): void {
    if (this.currentPage > 1) {
      this.currentPage--;
      // Ensure the value is not null by providing a default (e.g., 10)
      const tasksPerPage = this.tasksPerPageControl.value ?? 10;
      this.loadTasks(this.currentPage, tasksPerPage);
    }
  }
  

  nextPage(): void {
    if (this.currentPage < this.totalPages) {
      this.currentPage++;
      const tasksPerPage = this.tasksPerPageControl.value ?? 10;
      this.loadTasks(this.currentPage, tasksPerPage);
    }
  }
  
  /**
   * Change the number of tasks per page.
   */
  onTasksPerPageChange(): void {
    this.currentPage = 1;  // Reset to the first page when changing the number of tasks per page
    this.loadTasks();
  }

  /**
   * Logout handler.
   */
  onLogout(): void {
    this.tokenService.removeToken();
    this.router.navigate(['']);
  }

  /**
   * Check if the task form is valid.
   */
  isFormValid(): boolean {
    return this.taskForm.valid;
  }
}
