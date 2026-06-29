import { Component, inject, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators, AbstractControl, ValidationErrors, ReactiveFormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { AuthApiService } from '../../services/auth.services'; 
import { EnumToStringPipe } from '../../pipes/enum-to-string-pipe'; 
import { UserRole, UserStatus } from '../../models/enum'; 

// CORRECTED IMPORTS: Must include "Component" in the class name and ".component" in the file path
import { CreateUser } from '../admin/create-user/create-user';
import { GetLogs } from '../admin/get-logs/get-logs';

@Component({
  selector: 'app-profile-page',
  standalone: true,
  imports: [
    CommonModule, 
    ReactiveFormsModule,
    EnumToStringPipe,
    CreateUser, // <-- Added
    GetLogs    // <-- Added
  ],
  templateUrl: './profile-page.html',
})
export class ProfilePageComponent implements OnInit {
  private authService = inject(AuthApiService);

  public UserRoleEnum = UserRole;
  public UserStatusEnum = UserStatus;
  
  currentUser = this.authService.currentUser; 

  passwordForm!: FormGroup;
  successMessage: string = '';

  // --- Admin View States ---
  showCreateUserView = false;
  showLogsView = false;

  constructor(private fb: FormBuilder) {}

  ngOnInit(): void {
    this.passwordForm = this.fb.group({
      previousPassword: ['', Validators.required],
      newPassword: ['', [Validators.required, Validators.minLength(8)]],
      confirmPassword: ['', Validators.required]
    }, { validators: this.passwordMatchValidator });
  }

  // --- Toggle Functions ---
  toggleCreateUser() {
    this.showCreateUserView = !this.showCreateUserView;
    if (this.showCreateUserView) this.showLogsView = false; 
  }

  toggleLogs() {
    this.showLogsView = !this.showLogsView;
    if (this.showLogsView) this.showCreateUserView = false;
  }

  passwordMatchValidator(formGroup: AbstractControl): ValidationErrors | null {
    const previousPassword = formGroup.get('previousPassword');
    const newPassword = formGroup.get('newPassword');
    const confirmPassword = formGroup.get('confirmPassword');
    
    if (!previousPassword || !newPassword || !confirmPassword) return null;

    if (previousPassword.value && newPassword.value && previousPassword.value === newPassword.value) {
      newPassword.setErrors({ ...newPassword.errors, passwordSame: true });
    } else {
      if (newPassword.hasError('passwordSame')) {
        delete newPassword.errors?.['passwordSame'];
        newPassword.updateValueAndValidity({ onlySelf: true });
      }
    }

    if (confirmPassword.errors && !confirmPassword.errors['passwordMismatch']) {
      return null;
    }

    if (newPassword.value !== confirmPassword.value) {
      confirmPassword.setErrors({ passwordMismatch: true });
      return { passwordMismatch: true };
    } else {
      confirmPassword.setErrors(null);
      return null;
    }
  }

  onSubmitPasswordChange(): void {
    if (this.passwordForm.invalid) {
      this.passwordForm.markAllAsTouched();
      return;
    }

    console.log('Password Change Payload:', this.passwordForm.value);
    
    this.successMessage = 'Password updated successfully!';
    this.passwordForm.reset();
    
    setTimeout(() => {
      this.successMessage = '';
    }, 3000);
  }
}