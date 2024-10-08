import { Routes } from '@angular/router';
import { LoginComponent } from './components/login/login.component';
import { guestGuard } from './guards/guest.guard';
import { TaskListComponent } from './components/task-list/task-list.component';
import { authGuard } from './guards/auth.guard';
import { SingupComponent } from './components/singup/singup.component';

export const routes: Routes = [
    { path: '', component: LoginComponent, canActivate: [guestGuard] },
    { path: 'register', component: SingupComponent, canActivate: [guestGuard] },
    { path: 'tasks', component: TaskListComponent,  }
];
