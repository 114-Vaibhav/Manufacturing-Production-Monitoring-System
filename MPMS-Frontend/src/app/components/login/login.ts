import { Component } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common'; 
import { Router } from '@angular/router';
import { LoginModel } from '../../models/login.model'; 
import { UserRole } from '../../models/enum';
import { AuthApiService } from '../../services/auth.services'; 

@Component({
  selector: 'app-login',
  standalone: true, 
  imports: [FormsModule, CommonModule], 
  templateUrl: './login.html',
  styleUrl: './login.css',
})
export class Login {

  loginModel: LoginModel = new LoginModel();
  progress = false;

  // 1. Define roles here at the class level so the HTML can see it immediately.
  roles = Object.keys(UserRole)
    .filter(key => isNaN(Number(key))) 
    .map(key => ({
        name: key.replace(/([A-Z])/g, ' $1').trim(), 
        value: UserRole[key as keyof typeof UserRole] 
    }));

  constructor(
    private authApiService: AuthApiService,
    private router: Router
  ) {}

  login() { 
    if (
      this.loginModel.UserName.trim() === '' ||
      this.loginModel.Password.trim() === ''
    ) {
      alert('Username and Password are required');
      return;
    }

    if (this.loginModel.UserName.length < 4) {
      alert('Username must be at least 4 characters long');
      return;
    }

    if(this.loginModel.Role === -1) {
      alert('Please select a role');
      return;
    }

    this.progress = true;
    this.authApiService.loginApiCall(this.loginModel).subscribe({
      next: (response) => {
        // The service handles setting the user and saving the tokens to sessionStorage
        this.authApiService.setCurrentUser(response);
        
        this.router.navigate(['/profile']);
      },
      error: (err) => { 
        console.error('Login failed', err);
        
        // Check if the backend sent a custom string message
        if (typeof err.error === 'string') {
            alert(err.error); 
        } else {
            alert('Invalid username or password'); 
        }

        this.progress = false;
      }
    });
  }
}