import { Component, OnInit, inject } from '@angular/core';
import { ReactiveFormsModule, FormBuilder, Validators } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { Router } from '@angular/router';
import { UserRole } from '../../models/enum';
import { AuthApiService } from '../../services/auth.services';

@Component({
  selector: 'app-login',
  standalone: true,
  imports: [ReactiveFormsModule, CommonModule],
  templateUrl: './login.html',
  styleUrl: './login.css',
})
export class Login implements OnInit {
  private readonly fb = inject(FormBuilder);
  private readonly authApiService = inject(AuthApiService);
  private readonly router = inject(Router);

  readonly form = this.fb.nonNullable.group({
    UserName: ['', [Validators.required, Validators.minLength(4)]],
    Password: ['', [Validators.required, Validators.minLength(6)]],
    Role: [-1, [Validators.required, Validators.min(0)]],
  });

  progress = false;
  passwordVisible = false;
  errorMessage = '';

  readonly roles = Object.keys(UserRole)
    .filter(key => isNaN(Number(key)))
    .map(key => ({
      name: key.replace(/([A-Z])/g, ' $1').trim(),
      value: UserRole[key as keyof typeof UserRole],
    }));

  ngOnInit(): void {
    // Initialize role with first available option or default
    const defaultRole = this.roles[0]?.value ?? -1;
    this.form.patchValue({ Role: defaultRole });
  }

  login(): void {
    if (this.form.invalid) {
      this.form.markAllAsTouched();
      this.errorMessage = 'Please fill in all required fields correctly';
      return;
    }

    this.progress = true;
    this.errorMessage = '';
    const credentials = this.form.getRawValue();

    this.authApiService.loginApiCall(credentials).subscribe({
      next: response => {
        this.authApiService.setCurrentUser(response);
        this.router.navigate(['/profile']);
      },
      error: err => {
        this.progress = false;
        if (typeof err.error === 'string') {
          this.errorMessage = err.error;
        } else if (err.error?.message) {
          this.errorMessage = err.error.message;
        } else {
          this.errorMessage = 'Invalid username or password';
        }
      },
    });
  }

  togglePasswordVisibility(): void {
    this.passwordVisible = !this.passwordVisible;
  }
}