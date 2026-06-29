import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterLink, RouterLinkActive } from '@angular/router';

@Component({
  selector: 'app-sidebar',
  standalone: true,
  imports: [CommonModule, RouterLink, RouterLinkActive],
  templateUrl: './sidebar.html'
})
export class Sidebar {
  
  // Track whether the sidebar is expanded or collapsed
  isSidebarOpen = true;

  toggleSidebar() {
    this.isSidebarOpen = !this.isSidebarOpen;
  }

  // Array matching the routes you provided
  menuItems = [
    { label: 'Machines', route: '/machines', icon: '⚙️' },
    { label: 'Machine Readings', route: '/machine-readings', icon: '📊' },
    { label: 'Maintenance Logs', route: '/maintenance-logs', icon: '🛠️' },
    { label: 'Defects', route: '/defects', icon: '⚠️' },
    { label: 'Products', route: '/products', icon: '📦' },
    { label: 'Production Orders', route: '/production-orders', icon: '📋' },
    { label: 'Production Plans', route: '/production-plans', icon: '📅' },
    { label: 'Production Records', route: '/production-records', icon: '📝' },
    { label: 'Production Analytics', route: '/production-analytics', icon: '📈' }
  ];
}