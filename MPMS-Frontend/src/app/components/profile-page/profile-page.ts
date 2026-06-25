import { Component, inject, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators, AbstractControl, ValidationErrors, ReactiveFormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { AuthApiService } from '../../services/auth.services'; // <-- Adjust path based on your folder structure
import { EnumToStringPipe } from '../../pipes/enum-to-string-pipe'; // Adjust path as needed
import { UserRole, UserStatus } from '../../models/enum'; // Adjust path to your enums.ts

@Component({
  selector: 'app-profile-page',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule,EnumToStringPipe],
  templateUrl: './profile-page.html',
})
export class ProfilePageComponent implements OnInit {
  // 1. Inject the AuthApiService to access the currentUser signal
  private authService = inject(AuthApiService);

  public UserRoleEnum = UserRole;
  public UserStatusEnum = UserStatus;
  
  // 2. Expose the signal to the template
  currentUser = this.authService.currentUser; 

  passwordForm!: FormGroup;
  successMessage: string = '';

  constructor(private fb: FormBuilder) {}

  ngOnInit(): void {
    // Initialize the Reactive Form with validations
    this.passwordForm = this.fb.group({
      previousPassword: ['', Validators.required],
      newPassword: ['', [Validators.required, Validators.minLength(8)]],
      confirmPassword: ['', Validators.required]
    }, { validators: this.passwordMatchValidator });
  }

  // Custom validator to ensure 'new' and 'confirm' passwords match
  // passwordMatchValidator(formGroup: AbstractControl): ValidationErrors | null {
  //   const previousPassword = formGroup.get('previousPassword');
  //   const newPassword = formGroup.get('newPassword');
  //   const confirmPassword = formGroup.get('confirmPassword');
    
  //   if  ( !previousPassword || !newPassword || !confirmPassword) return null;

  //   if(previousPassword==newPassword){
  //     newPassword.setErrors({ passwordSame: true });
  //     return { passwordSame: true };
  //   }

  //   if (confirmPassword.errors && !confirmPassword.errors['passwordMismatch']) {
  //     // Return if another validator has already found an error on the matchingControl
  //     return null;
  //   }

  //   if (newPassword.value !== confirmPassword.value) {
  //     confirmPassword.setErrors({ passwordMismatch: true });
  //     return { passwordMismatch: true };
  //   } else {
  //     confirmPassword.setErrors(null);
  //     return null;
  //   }
  // }
  // Custom validator to ensure logic between passwords
  passwordMatchValidator(formGroup: AbstractControl): ValidationErrors | null {
    const previousPassword = formGroup.get('previousPassword');
    const newPassword = formGroup.get('newPassword');
    const confirmPassword = formGroup.get('confirmPassword');
    
    if (!previousPassword || !newPassword || !confirmPassword) return null;

    // 1. Validation: New Password cannot be the same as the Previous Password
    if (previousPassword.value && newPassword.value && previousPassword.value === newPassword.value) {
      // Set an error on the newPassword control so we can show a specific message in the HTML
      newPassword.setErrors({ ...newPassword.errors, passwordSame: true });
    } else {
      // If it passes this check, remove the 'passwordSame' error without clearing other errors (like minLength)
      if (newPassword.hasError('passwordSame')) {
        delete newPassword.errors?.['passwordSame'];
        newPassword.updateValueAndValidity({ onlySelf: true });
      }
    }

    // 2. Validation: New Password and Confirm Password must match
    if (confirmPassword.errors && !confirmPassword.errors['passwordMismatch']) {
      // Return if another validator has already found an error on the confirmPassword
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
    
    // TODO: Wire up your password change API call here
    
    this.successMessage = 'Password updated successfully!';
    this.passwordForm.reset();
    
    // Clear the success message after 3 seconds
    setTimeout(() => {
      this.successMessage = '';
    }, 3000);
  }
}