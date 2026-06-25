import { ComponentFixture, TestBed } from '@angular/core/testing';
import { ProfilePageComponent } from './profile-page'; 
import { ReactiveFormsModule } from '@angular/forms';
import { AuthApiService } from '../../services/auth.services';
import { signal } from '@angular/core';
import { UserRole, UserStatus } from '../../models/enum';
import { vi } from 'vitest';

describe('ProfilePageComponent', () => {
  let component: ProfilePageComponent;
  let fixture: ComponentFixture<ProfilePageComponent>;

  const mockUser = {
    id: 1,
    username: 'testuser',
    fullName: 'Test User',
    email: 'test@test.com',
    role: UserRole.Admin,
    status: UserStatus.Active,
    accessToken: '',
    refreshToken: ''
  };

  const mockAuthService = {
    currentUser: signal(mockUser)
  };

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [ProfilePageComponent, ReactiveFormsModule],
      providers: [
        { provide: AuthApiService, useValue: mockAuthService }
      ]
    }).compileComponents();

    fixture = TestBed.createComponent(ProfilePageComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });

  it('should initialize form with empty controls', () => {
    expect(component.passwordForm.get('previousPassword')?.value).toBe('');
    expect(component.passwordForm.get('newPassword')?.value).toBe('');
    expect(component.passwordForm.invalid).toBe(true);
  });

  describe('Password Validators', () => {
    it('should set passwordSame error if previous and new passwords match', () => {
      component.passwordForm.patchValue({
        previousPassword: 'Password123!',
        newPassword: 'Password123!',
        confirmPassword: 'DifferentPassword!'
      });

      expect(component.passwordForm.get('newPassword')?.hasError('passwordSame')).toBe(true);
    });

    it('should set passwordMismatch error if new and confirm passwords do not match', () => {
      component.passwordForm.patchValue({
        previousPassword: 'OldPassword123!',
        newPassword: 'NewPassword123!',
        confirmPassword: 'MismatchPassword123!'
      });

      expect(component.passwordForm.get('confirmPassword')?.hasError('passwordMismatch')).toBe(true);
    });

    it('should be valid when all rules are met', () => {
      component.passwordForm.patchValue({
        previousPassword: 'OldPassword123!',
        newPassword: 'NewPassword123!',
        confirmPassword: 'NewPassword123!'
      });

      expect(component.passwordForm.valid).toBe(true);
      expect(component.passwordForm.get('newPassword')?.errors).toBeNull();
      expect(component.passwordForm.get('confirmPassword')?.errors).toBeNull();
    });
  });

  describe('onSubmitPasswordChange', () => {
    it('should not submit and mark all as touched if form is invalid', () => {
      // Use Vitest's vi.spyOn
      vi.spyOn(component.passwordForm, 'markAllAsTouched');
      
      component.onSubmitPasswordChange();

      expect(component.passwordForm.markAllAsTouched).toHaveBeenCalled();
      expect(component.successMessage).toBe('');
    });

    it('should set success message and reset form on valid submission', () => {
      vi.spyOn(component.passwordForm, 'reset');

      component.passwordForm.patchValue({
        previousPassword: 'OldPassword123!',
        newPassword: 'NewPassword123!',
        confirmPassword: 'NewPassword123!'
      });

      component.onSubmitPasswordChange();

      expect(component.successMessage).toBe('Password updated successfully!');
      expect(component.passwordForm.reset).toHaveBeenCalled();
    });
  });
});