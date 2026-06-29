import { Component, Input } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormControl, ReactiveFormsModule } from '@angular/forms';

@Component({
  selector: 'app-form-field',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule],
  templateUrl: './form-field.html',
  styleUrl: './form-field.css',
})
export class FormFieldComponent {
  @Input({ required: true }) label!: string;
  @Input({ required: true }) control!: FormControl;
  @Input() type: 'text' | 'number' | 'date' | 'email' | 'password' | 'textarea' | 'select' = 'text';
  @Input() placeholder = '';
  @Input() options: { label: string; value: string | number }[] = [];
  @Input() rows = 3;
  @Input() fullWidth = false;
  @Input() required = false;

  get errorMessage(): string {
    if (!this.control.errors || !this.control.touched) {
      return '';
    }

    const errors = this.control.errors;

    if (errors['required']) {
      return `${this.label} is required`;
    }

    if (errors['minlength']) {
      return `${this.label} must be at least ${errors['minlength'].requiredLength} characters`;
    }

    if (errors['maxlength']) {
      return `${this.label} must be at most ${errors['maxlength'].requiredLength} characters`;
    }

    if (errors['min']) {
      return `${this.label} must be at least ${errors['min'].min}`;
    }

    if (errors['max']) {
      return `${this.label} must be at most ${errors['max'].max}`;
    }

    if (errors['pattern']) {
      return `${this.label} format is invalid`;
    }

    if (errors['email']) {
      return `${this.label} must be a valid email`;
    }

    return 'Invalid value';
  }

  get isInvalid(): boolean {
    return this.control.invalid && this.control.touched;
  }
}
