import { ComponentFixture, TestBed } from '@angular/core/testing';
import { Login } from './login'; // Adjust path if needed
import { AuthApiService } from '../../services/auth.services';
import { Router } from '@angular/router';
import { of, throwError } from 'rxjs';
import { vi } from 'vitest';

describe('Login', () => {
  let component: Login;
  let fixture: ComponentFixture<Login>;
  let authServiceMock: any;
  let routerMock: any;

  beforeEach(async () => {
    // 1. Create mock objects using Vitest's vi.fn()
    authServiceMock = {
      loginApiCall: vi.fn(),
      setCurrentUser: vi.fn()
    };
    
    routerMock = {
      navigate: vi.fn()
    };

    await TestBed.configureTestingModule({
      imports: [Login],
      providers: [
        { provide: AuthApiService, useValue: authServiceMock },
        { provide: Router, useValue: routerMock }
      ]
    }).compileComponents();

    fixture = TestBed.createComponent(Login);
    component = fixture.componentInstance;
    
    // Spy on the browser's alert box using Vitest
    vi.spyOn(window, 'alert').mockImplementation(() => {});
    
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });

  it('should alert if username or password is empty', () => {
    component.loginModel.UserName = '   '; 
    component.loginModel.Password = '12345';
    
    component.login();
    
    expect(window.alert).toHaveBeenCalledWith('Username and Password are required');
    expect(authServiceMock.loginApiCall).not.toHaveBeenCalled();
  });

  it('should alert if username is less than 4 characters', () => {
    component.loginModel.UserName = 'abc'; 
    component.loginModel.Password = 'validPassword';
    
    component.login();
    
    expect(window.alert).toHaveBeenCalledWith('Username must be at least 4 characters long');
  });

  it('should call loginApiCall and navigate on successful login', () => {
    component.loginModel.UserName = 'validUser';
    component.loginModel.Password = 'validPassword';
    component.loginModel.Role = 1;

    const mockResponse = { accessToken: 'token123', fullName: 'Test User' };
    authServiceMock.loginApiCall.mockReturnValue(of(mockResponse));

    component.login();

    // Use .toBe(true) instead of .toBeTrue() for Vitest
    expect(component.progress).toBe(true);
    expect(authServiceMock.loginApiCall).toHaveBeenCalledWith(component.loginModel);
    expect(authServiceMock.setCurrentUser).toHaveBeenCalledWith(mockResponse);
    expect(routerMock.navigate).toHaveBeenCalledWith(['/profile']);
  });

  it('should show alert on login failure', () => {
    component.loginModel.UserName = 'validUser';
    component.loginModel.Password = 'wrongPassword';
    component.loginModel.Role = 1;

    const mockError = { error: 'Invalid credentials provided' };
    authServiceMock.loginApiCall.mockReturnValue(throwError(() => mockError));

    component.login();

    expect(window.alert).toHaveBeenCalledWith('Invalid credentials provided');
    expect(component.progress).toBe(false);
  });
});