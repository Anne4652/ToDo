import { CommonModule } from '@angular/common';
import { HttpClientModule } from '@angular/common/http';
import { Component } from '@angular/core';
import { ReactiveFormsModule, FormGroup, FormBuilder, FormControl, Validators } from '@angular/forms';
import { AuthService } from '../../services/auth.service';

@Component({
  selector: 'app-singup',
  standalone: true,
  imports: [ReactiveFormsModule, CommonModule, HttpClientModule],
  templateUrl: './singup.component.html',
  styleUrl: './singup.component.scss'
})
export class SingupComponent {
  singupForm!: FormGroup;

  constructor(
    private formBuilder: FormBuilder,
    private authService: AuthService
  )
  {
    this.singupForm = this.formBuilder.group({
      userName: new FormControl('', [Validators.required]),
      email: new FormControl('', [Validators.required]),
      password: new FormControl('', [Validators.required])
    });
  }

  onSubmit() {
    if (this.singupForm.valid) {
      this.authService.onSingup(this.singupForm.value).subscribe({
        next: () => {},
      });
    } else {
      this.singupForm.markAllAsTouched();
    }
  }
}
