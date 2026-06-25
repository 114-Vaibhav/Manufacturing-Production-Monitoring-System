// import { Component } from '@angular/core';

// @Component({
//   selector: 'app-navbar',
//   imports: [],
//   templateUrl: './navbar.html',
//   styleUrl: './navbar.css',
// })
// export class Navbar {}

import { Component } from '@angular/core';
import { inject } from '@angular/core';
import { AuthApiService } from '../../services/auth.services';


@Component({
  selector: 'app-navbar',
  
  templateUrl: './navbar.html'
})
export class Navbar {

  private authService = inject(AuthApiService);

  // Expose the signal to the template
  currentUser = this.authService.currentUser;

}