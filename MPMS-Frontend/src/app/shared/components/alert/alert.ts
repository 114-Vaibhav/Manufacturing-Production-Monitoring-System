import { Component, Input } from '@angular/core';
import { CommonModule } from '@angular/common';

export type AlertType = 'success' | 'error' | 'warning' | 'info';

@Component({
  selector: 'app-alert',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './alert.html',
  styleUrl: './alert.css',
})
export class AlertComponent {
  @Input({ required: true }) message!: string;
  @Input() type: AlertType = 'info';
  @Input() closable = false;
  closed = false;

  get colorClasses(): string {
    const colors: Record<AlertType, string> = {
      error: 'border-red-200 bg-red-50 text-red-700',
      success: 'border-emerald-200 bg-emerald-50 text-emerald-700',
      warning: 'border-yellow-200 bg-yellow-50 text-yellow-700',
      info: 'border-blue-200 bg-blue-50 text-blue-700',
    };
    return colors[this.type];
  }

  close(): void {
    this.closed = true;
  }
}
