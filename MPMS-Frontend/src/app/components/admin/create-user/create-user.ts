import { CommonModule } from '@angular/common';
import { Component, OnInit, inject } from '@angular/core';
import { FormBuilder, ReactiveFormsModule, Validators, AbstractControl, ValidationErrors } from '@angular/forms';
import { finalize } from 'rxjs';
import { UserRole, UserStatus } from '../../../models/enum';
import { UserService, CreateUserRequest } from '../../../services/user.service';
import { PageHeaderComponent } from '../../../shared/components/page-header/page-header';
import { getApiErrorMessage } from '../../../shared/error-message';

@Component({
  selector: 'app-create-user',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule, PageHeaderComponent],
  templateUrl: './create-user.html',
  styleUrl: './create-user.css',
})
export class CreateUser implements OnInit {
  private readonly fb = inject(FormBuilder);
  private readonly userService = inject(UserService);

  readonly roleOptions = [
    { label: 'Admin', value: UserRole.Admin },
    { label: 'Operator', value: UserRole.Operator },
    { label: 'Production Manager', value: UserRole.ProductionManager },
    { label: 'Quality Inspector', value: UserRole.QualityInspector },
    { label: 'Maintenance Technician', value: UserRole.MaintenanceTechnician },
    { label: 'Production Planner', value: UserRole.ProductionPlanner },
    { label: 'Plant Manager', value: UserRole.PlantManager },
  ];

  readonly statusOptions = [
    { label: 'Active', value: UserStatus.Active },
    { label: 'Inactive', value: UserStatus.Inactive },
    { label: 'Suspended', value: UserStatus.Suspended },
  ];

  readonly form = this.fb.nonNullable.group(
    {
      username: ['', [Validators.required, Validators.minLength(4), Validators.maxLength(50), Validators.pattern(/^[a-zA-Z0-9_-]+$/)]],
      fullName: ['', [Validators.required, Validators.minLength(2), Validators.maxLength(100)]],
      email: ['', [Validators.required, Validators.email]],
      password: ['', [Validators.required, Validators.minLength(8)]],
      passwordConfirm: ['', [Validators.required]],
      role: [UserRole.Operator, [Validators.required]],
      status: [UserStatus.Active, [Validators.required]],
    },
    { validators: this.passwordMatchValidator }
  );

  saving = false;
  errorMessage = '';
  successMessage = '';
  passwordVisible = false;
  passwordConfirmVisible = false;

  ngOnInit(): void {
    // No additional initialization needed
  }

  get passwordMatchError(): boolean {
    return this.form.hasError('passwordMismatch') && this.form.controls.passwordConfirm.touched;
  }

  submit(): void {
    if (this.form.invalid) {
      this.form.markAllAsTouched();
      return;
    }

    this.saving = true;
    this.errorMessage = '';
    this.successMessage = '';

    const formValue = this.form.getRawValue();
    const request: CreateUserRequest = {
      username: formValue.username,
      fullName: formValue.fullName,
      email: formValue.email,
      password: formValue.password,
      role: formValue.role,
      status: formValue.status,
    };

    this.userService
      .createUser(request)
      .pipe(finalize(() => (this.saving = false)))
      .subscribe({
        next: () => {
          this.successMessage = 'User created successfully.';
          this.resetForm();
        },
        error: err => (this.errorMessage = getApiErrorMessage(err, 'Unable to create user.')),
      });
  }

  resetForm(): void {
    this.form.reset({
      username: '',
      fullName: '',
      email: '',
      password: '',
      passwordConfirm: '',
      role: UserRole.Operator,
      status: UserStatus.Active,
    });
    this.passwordVisible = false;
    this.passwordConfirmVisible = false;
  }

  togglePasswordVisibility(): void {
    this.passwordVisible = !this.passwordVisible;
  }

  togglePasswordConfirmVisibility(): void {
    this.passwordConfirmVisible = !this.passwordConfirmVisible;
  }

  private passwordMatchValidator(control: AbstractControl): ValidationErrors | null {
    const password = control.get('password');
    const passwordConfirm = control.get('passwordConfirm');

    if (!password || !passwordConfirm) {
      return null;
    }

    return password.value === passwordConfirm.value ? null : { passwordMismatch: true };
  }
}
