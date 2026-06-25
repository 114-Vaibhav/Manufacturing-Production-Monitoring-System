import { HttpClient } from '@angular/common/http';
import { Injectable, signal } from '@angular/core';
import { LoginModel } from '../models/login.model';
import { User } from '../models/user.model'; 
import { environment } from '../../environments/environment.development';

@Injectable({
  providedIn: 'root'
})
export class AuthApiService {

  currentUser = signal<User | null>(null);
  apiURL = environment.apiUrl;
  
  constructor(private http: HttpClient) {
    // 1. RECOVER STATE: This runs instantly when Angular reloads
    const savedUser = sessionStorage.getItem('currentUser');
    if (savedUser) {
      this.currentUser.set(JSON.parse(savedUser));
    }
  }

  loginApiCall(loginModel: LoginModel) {
    return this.http.post<User>(
      `${this.apiURL}/user-login`,
      loginModel
    );
  }

  setCurrentUser(user: User) {
    // 2. SAVE STATE: Update the signal for immediate UI changes
    this.currentUser.set(user);
    
    // 3. PERSIST STATE: Save to storage so it survives refresh
    sessionStorage.setItem('currentUser', JSON.stringify(user));
    
    // 4. SAVE TOKENS: Save both tokens for Interceptors/Guards
    if (user.accessToken) {
       sessionStorage.setItem('accessToken', user.accessToken);
    }
    if (user.refreshToken) {
       sessionStorage.setItem('refreshToken', user.refreshToken);
    }
  }

  logout() {
    this.currentUser.set(null);
    sessionStorage.clear(); // Wipes the saved user and tokens
  }
}