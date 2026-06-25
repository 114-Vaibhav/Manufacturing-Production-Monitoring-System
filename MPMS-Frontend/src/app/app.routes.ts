import { Routes } from '@angular/router';
import { ProfilePageComponent } from './components/profile-page/profile-page';
import { Login } from './components/login/login';
import { authGuard } from './guards/auth.guard';


export const routes: Routes = [
    // 1. Default route: If the URL is empty, send them to login
  { path: '', redirectTo: '/login', pathMatch: 'full' },
  
  // 2. When the URL is '/login', put the Login component into the router-outlet
  { path: 'login', component: Login },
  
  // 3. When the URL is '/profile', put the Profile component into the router-outlet
 
    { path: 'profile', component: ProfilePageComponent  , canActivate: [ authGuard]},
];
