import { Routes } from '@angular/router';
import { ProfilePageComponent } from './components/profile-page/profile-page';
import { Login } from './components/login/login';
import { authGuard } from './guards/auth.guard';
import { DefectsComponent } from './components/defects/defects';
import { MachineReadingsComponent } from './components/machine-readings/machine-readings';
import { MachinesComponent } from './components/machines/machines';
import { MaintenanceLogsComponent } from './components/maintenance-logs/maintenance-logs';
import { ProductsComponent } from './components/products/products';
import { ProductionAnalyticsComponent } from './components/production-analytics/production-analytics';
import { ProductionOrdersComponent } from './components/production-orders/production-orders';
import { ProductionPlansComponent } from './components/production-plans/production-plans';
import { ProductionRecordsComponent } from './components/production-records/production-records';
import { CreateUser } from './components/admin/create-user/create-user';

export const routes: Routes = [
  // 1. Default route: If the URL is empty, send them to login
  { path: '', redirectTo: '/login', pathMatch: 'full' },

  // 2. When the URL is '/login', put the Login component into the router-outlet
  { path: 'login', component: Login },

  // 3. When the URL is '/profile', put the Profile component into the router-outlet

  { path: 'admin/create-user', component: CreateUser, canActivate: [authGuard]},
  { path: 'profile', component: ProfilePageComponent, canActivate: [authGuard] },
  { path: 'machines', component: MachinesComponent, canActivate: [authGuard] },
  { path: 'machine-readings', component: MachineReadingsComponent, canActivate: [authGuard] },
  { path: 'maintenance-logs', component: MaintenanceLogsComponent, canActivate: [authGuard] },
  { path: 'defects', component: DefectsComponent, canActivate: [authGuard] },
  { path: 'products', component: ProductsComponent, canActivate: [authGuard] },
  { path: 'production-orders', component: ProductionOrdersComponent, canActivate: [authGuard] },
  { path: 'production-plans', component: ProductionPlansComponent, canActivate: [authGuard] },
  { path: 'production-records', component: ProductionRecordsComponent, canActivate: [authGuard] },
  { path: 'production-analytics', component: ProductionAnalyticsComponent, canActivate: [authGuard] },
];
