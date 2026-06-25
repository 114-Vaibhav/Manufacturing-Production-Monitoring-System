import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-sidebar',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './sidebar.html'
})
export class Sidebar {

  menuItems = [
    { icon: '📊', label: 'Dashboard' },
    { icon: '📅', label: 'Production Planning' },
    { icon: '⚙️', label: 'Machine Monitoring' },
    { icon: '🚨', label: 'Defect Tracking' },
    { icon: '📈', label: 'Analytics' },
    { icon: '📑', label: 'Reports' }
  ];

}