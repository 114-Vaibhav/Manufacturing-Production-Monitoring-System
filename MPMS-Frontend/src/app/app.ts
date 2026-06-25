import { Component, signal, inject } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { Login } from './components/login/login';
import { Sidebar } from './shared/sidebar/sidebar';
import { Navbar } from './shared/navbar/navbar';
import { Footer } from './shared/footer/footer';
import { ProfilePageComponent } from './components/profile-page/profile-page';
import { CommonModule } from '@angular/common'; // <-- 1. Import CommonModule here
import { AuthApiService } from './services/auth.services';

@Component({
  selector: 'app-root',
  imports: [RouterOutlet, 
    CommonModule,
     Navbar, Sidebar, Footer],
  templateUrl: './app.html',
  styleUrl: './app.css'
})
export class App {
  protected readonly title = signal('MPMS-Frontend');
  private authService = inject(AuthApiService);
  currentUser = this.authService.currentUser;
}
