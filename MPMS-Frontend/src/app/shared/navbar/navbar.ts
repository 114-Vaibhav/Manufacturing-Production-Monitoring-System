import { Component, inject } from '@angular/core';
import { RouterLink } from '@angular/router'; // <-- ADDED: Required for routerLink in HTML
import { AuthApiService } from '../../services/auth.services';

@Component({
  selector: 'app-navbar',
  standalone: true, // <-- RESTORED: Required to use the imports array
  imports: [RouterLink], // <-- ADDED: Gives HTML permission to use routerLink
  templateUrl: './navbar.html'
})
export class Navbar {

  private authService = inject(AuthApiService);

  // Expose the signal to the template
  currentUser = this.authService.currentUser;

}