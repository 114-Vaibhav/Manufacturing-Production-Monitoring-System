import { CommonModule } from '@angular/common';
import { HttpClient } from '@angular/common/http';
import { Component, Input, OnInit } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { environment } from '../../../environments/environment.development';

export type ResourceFieldType = 'text' | 'number' | 'date' | 'datetime-local' | 'select' | 'textarea';

export interface ResourceOption {
  label: string;
  value: string | number;
}

export interface ResourceField {
  key: string;
  label: string;
  type: ResourceFieldType;
  required?: boolean;
  options?: ResourceOption[];
}

export interface ResourceConfig {
  title: string;
  description: string;
  endpoint: string;
  idKey: string;
  columns: ResourceField[];
  formFields: ResourceField[];
  allowCreate?: boolean;
  allowEdit?: boolean;
  allowDelete?: boolean;
}

@Component({
  selector: 'app-api-resource',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './api-resource.html',
})
export class ApiResourceComponent implements OnInit {
  @Input({ required: true }) config!: ResourceConfig;

  records: Record<string, unknown>[] = [];
  form: Record<string, unknown> = {};
  editingId: string | number | null = null;
  loading = false;
  saving = false;
  errorMessage = '';
  successMessage = '';
  pageNumber = 1;
  pageSize = 10;

  private readonly apiUrl = environment.apiUrl;

  constructor(private http: HttpClient) {}

  ngOnInit(): void {
    this.resetForm();
    this.loadRecords();
  }

  get canCreate(): boolean {
    return this.config.allowCreate !== false && this.config.formFields.length > 0;
  }

  get canEdit(): boolean {
    return this.config.allowEdit !== false && this.config.formFields.length > 0;
  }

  get canDelete(): boolean {
    return this.config.allowDelete !== false;
  }

  loadRecords(): void {
    this.loading = true;
    this.errorMessage = '';

    this.http
      .get<Record<string, unknown>[]>(
        `${this.apiUrl}${this.config.endpoint}?pageNumber=${this.pageNumber}&pageSize=${this.pageSize}`
      )
      .subscribe({
        next: records => {
          this.records = Array.isArray(records) ? records : [];
          this.loading = false;
        },
        error: err => {
          this.errorMessage = this.getErrorMessage(err, `Unable to load ${this.config.title.toLowerCase()}.`);
          this.loading = false;
        },
      });
  }

  save(): void {
    if (!this.canCreate && this.editingId === null) {
      return;
    }

    this.saving = true;
    this.errorMessage = '';
    this.successMessage = '';

    const request$ =
      this.editingId === null
        ? this.http.post(`${this.apiUrl}${this.config.endpoint}`, this.form)
        : this.http.put(`${this.apiUrl}${this.config.endpoint}/${this.editingId}`, this.form);

    request$.subscribe({
      next: () => {
        this.successMessage = this.editingId === null ? 'Created successfully.' : 'Updated successfully.';
        this.saving = false;
        this.resetForm();
        this.loadRecords();
      },
      error: err => {
        this.errorMessage = this.getErrorMessage(err, 'Unable to save record.');
        this.saving = false;
      },
    });
  }

  edit(record: Record<string, unknown>): void {
    if (!this.canEdit) {
      return;
    }

    this.editingId = record[this.config.idKey] as string | number;
    this.form = {};

    this.config.formFields.forEach(field => {
      this.form[field.key] = this.toInputValue(record[field.key], field.type);
    });
  }

  delete(record: Record<string, unknown>): void {
    if (!this.canDelete) {
      return;
    }

    const id = record[this.config.idKey] as string | number;
    this.errorMessage = '';
    this.successMessage = '';

    this.http.delete(`${this.apiUrl}${this.config.endpoint}/${id}`).subscribe({
      next: () => {
        this.successMessage = 'Deleted successfully.';
        this.loadRecords();
      },
      error: err => {
        this.errorMessage = this.getErrorMessage(err, 'Unable to delete record.');
      },
    });
  }

  resetForm(): void {
    this.editingId = null;
    this.form = {};

    this.config?.formFields.forEach(field => {
      this.form[field.key] = field.type === 'number' || field.type === 'select' ? 0 : '';
    });
  }

  nextPage(): void {
    this.pageNumber += 1;
    this.loadRecords();
  }

  previousPage(): void {
    if (this.pageNumber === 1) {
      return;
    }

    this.pageNumber -= 1;
    this.loadRecords();
  }

  displayValue(record: Record<string, unknown>, field: ResourceField): string {
    const value = record[field.key];

    if (value === null || value === undefined || value === '') {
      return '-';
    }

    if (field.options?.length) {
      const option = field.options.find(item => item.value === value);
      return option?.label ?? String(value);
    }

    if (field.type === 'date' || field.type === 'datetime-local') {
      return new Date(String(value)).toLocaleString();
    }

    return String(value);
  }

  trackById = (_: number, record: Record<string, unknown>): unknown => record[this.config.idKey];

  private toInputValue(value: unknown, type: ResourceFieldType): unknown {
    if (value === null || value === undefined) {
      return type === 'number' || type === 'select' ? 0 : '';
    }

    if (type === 'date') {
      return String(value).slice(0, 10);
    }

    if (type === 'datetime-local') {
      return String(value).slice(0, 16);
    }

    return value;
  }

  private getErrorMessage(err: { error?: unknown; message?: string }, fallback: string): string {
    if (typeof err.error === 'string') {
      return err.error;
    }

    if (err.error && typeof err.error === 'object' && 'message' in err.error) {
      return String((err.error as { message: unknown }).message);
    }

    return err.message ?? fallback;
  }
}
