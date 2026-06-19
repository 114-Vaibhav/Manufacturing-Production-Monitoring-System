// import { Component } from '@angular/core';

// @Component({
//   selector: 'app-navbar',
//   imports: [],
//   templateUrl: './navbar.html',
//   styleUrl: './navbar.css',
// })
// export class Navbar {}

import { Component } from '@angular/core';

@Component({
  selector: 'app-navbar',
  
  templateUrl: './navbar.html'
})
export class NavbarComponent {

  currentUser = {
    name: 'Vaibhav Gupta',
    role: 'Plant Manager'
  };

}