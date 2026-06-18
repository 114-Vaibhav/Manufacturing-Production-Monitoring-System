import { Component } from '@angular/core';
import { FormsModule } from '@angular/forms'; // 👈 Crucial for [(ngModel)]
import { CommonModule } from '@angular/common'; // 👈 Optional if using @for, required if keeping *ngFor
@Component({
  selector: 'app-login',
  imports: [FormsModule, CommonModule],
  templateUrl: './login.html',
  styleUrl: './login.css',
})
export class Login {
  username: string = '';
  password: string = '';
  role: string = '';
  roles: string[] = ['Admin',
        'Operator',
        'ProductionManager',
        'QualityInspector',
        'MaintenanceTechnician',
        'ProductionPlanner',
        'PlantManager'];

  login() {
    // Implement your login logic here
    console.log('Username:', this.username);
    console.log('Password:', this.password);
    console.log('Role:', this.role);

  }
}
