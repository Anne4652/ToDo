import { Component } from '@angular/core';
import { FormBuilder, FormGroup, FormControl, Validators } from '@angular/forms';
import { AuthService } from '../../services/auth.service';
import { ReactiveFormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { HttpClientModule } from '@angular/common/http';
import { Router } from '@angular/router';

@Component({
  selector: 'app-login',
  standalone: true,
  imports: [ReactiveFormsModule, CommonModule, HttpClientModule],
  templateUrl: './login.component.html',
  styleUrl: './login.component.scss'
})
export class LoginComponent {
  loginForm!: FormGroup;

  constructor(
    private formBuilder: FormBuilder,
    private authService: AuthService,
    private router: Router
  )
  {
    this.loginForm = this.formBuilder.group({
      userName: new FormControl('', [Validators.required]),
      password: new FormControl('', [Validators.required])
    });
  }

  login(): void {
    if (this.loginForm.valid) {
      this.authService.onLogin(this.loginForm.value).subscribe({
        next: () => {},
      });
    } else {
      this.loginForm.markAllAsTouched();
    }
  }

  goToRegister(): void {
    this.router.navigate(['/register']);  // Navigate to the registration page
  }
}
