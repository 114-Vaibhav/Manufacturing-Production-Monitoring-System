import { CommonModule } from '@angular/common';
import { Component, OnInit, inject } from '@angular/core';
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { finalize } from 'rxjs';
import { Defect, DefectRequest } from '../../models/defect';
import { DefectsService } from '../../services/defects.service';
import { ConfirmDialogComponent } from '../../shared/components/confirm-dialog/confirm-dialog';
import { DataTableColumn, DataTableComponent } from '../../shared/components/data-table/data-table';
import { EmptyStateComponent } from '../../shared/components/empty-state/empty-state';
import { LoadingSpinnerComponent } from '../../shared/components/loading-spinner/loading-spinner';
import { PageHeaderComponent } from '../../shared/components/page-header/page-header';
import { PaginationComponent } from '../../shared/components/pagination/pagination';
import { SearchBoxComponent } from '../../shared/components/search-box/search-box';
import { getApiErrorMessage } from '../../shared/error-message';
import { defectSeverityOptions, defectTypeOptions } from '../resource-options';

@Component({
  selector: 'app-defects',
  standalone: true,
  imports: [
    CommonModule,
    ReactiveFormsModule,
    ConfirmDialogComponent,
    DataTableComponent,
    EmptyStateComponent,
    LoadingSpinnerComponent,
    PageHeaderComponent,
    PaginationComponent,
    SearchBoxComponent,
  ],
  templateUrl: './defects.html',
  styleUrl: './defects.css',
})
export class DefectsComponent implements OnInit {
  private readonly fb = inject(FormBuilder);
  private readonly defectsService = inject(DefectsService);

  readonly typeOptions = defectTypeOptions;
  readonly severityOptions = defectSeverityOptions;
  readonly columns: DataTableColumn<Defect>[] = [
    { label: 'ID', value: defect => defect.defectId },
    { label: 'Order', value: defect => defect.orderId },
    { label: 'Machine', value: defect => defect.machineId },
    { label: 'Type', value: defect => this.getTypeLabel(defect.type) },
    { label: 'Severity', value: defect => this.getSeverityLabel(defect.severity) },
    { label: 'Description', value: defect => defect.description },
    { label: 'Reporter', value: defect => defect.reportedBy },
    { label: 'Created', value: defect => this.formatDate(defect.createdAt) },
  ];

  readonly form = this.fb.nonNullable.group({
    orderId: [0, [Validators.required, Validators.min(1)]],
    machineId: [0, [Validators.required, Validators.min(1)]],
    type: [0, [Validators.required, Validators.min(1)]],
    severity: [0, [Validators.required, Validators.min(1)]],
    description: ['', [Validators.required, Validators.minLength(5), Validators.maxLength(500)]],
  });

  defects: Defect[] = [];
  searchTerm = '';
  pageNumber = 1;
  pageSize = 10;
  loading = false;
  saving = false;
  errorMessage = '';
  successMessage = '';
  editingDefectId: number | null = null;
  defectToDelete: Defect | null = null;

  ngOnInit(): void {
    this.loadDefects();
  }

  get filteredDefects(): Defect[] {
    const term = this.searchTerm.trim().toLowerCase();
    return term
      ? this.defects.filter(defect => 
          `${defect.orderId} ${defect.machineId} ${defect.description}`.toLowerCase().includes(term)
        )
      : this.defects;
  }

  loadDefects(): void {
    this.loading = true;
    this.errorMessage = '';
    this.defectsService
      .getDefects(this.pageNumber, this.pageSize)
      .pipe(finalize(() => (this.loading = false)))
      .subscribe({
        next: defects => (this.defects = defects),
        error: err => (this.errorMessage = getApiErrorMessage(err, 'Unable to load defects.')),
      });
  }

  submit(): void {
    if (this.form.invalid) {
      this.form.markAllAsTouched();
      return;
    }

    this.saving = true;
    this.errorMessage = '';
    this.successMessage = '';
    const request = this.form.getRawValue() as DefectRequest;
    const request$ =
      this.editingDefectId === null
        ? this.defectsService.createDefect(request)
        : this.defectsService.updateDefect(this.editingDefectId, request);

    request$.pipe(finalize(() => (this.saving = false))).subscribe({
      next: () => {
        this.successMessage = this.editingDefectId === null ? 'Defect created successfully.' : 'Defect updated successfully.';
        this.resetForm();
        this.loadDefects();
      },
      error: err => (this.errorMessage = getApiErrorMessage(err, 'Unable to save defect.')),
    });
  }

  editDefect(defect: Defect): void {
    this.editingDefectId = defect.defectId;
    this.form.setValue({
      orderId: defect.orderId,
      machineId: defect.machineId,
      type: defect.type,
      severity: defect.severity,
      description: defect.description,
    });
  }

  requestDelete(defect: Defect): void {
    this.defectToDelete = defect;
  }

  confirmDelete(): void {
    if (!this.defectToDelete) {
      return;
    }

    this.defectsService.deleteDefect(this.defectToDelete.defectId).subscribe({
      next: () => {
        this.successMessage = 'Defect deleted successfully.';
        this.defectToDelete = null;
        this.loadDefects();
      },
      error: err => (this.errorMessage = getApiErrorMessage(err, 'Unable to delete defect.')),
    });
  }

  resetForm(): void {
    this.editingDefectId = null;
    this.form.reset({ orderId: 0, machineId: 0, type: 0, severity: 0, description: '' });
  }

  nextPage(): void {
    this.pageNumber += 1;
    this.loadDefects();
  }

  previousPage(): void {
    if (this.pageNumber > 1) {
      this.pageNumber -= 1;
      this.loadDefects();
    }
  }

  private getTypeLabel(type: number): string {
    return this.typeOptions.find(opt => opt.value === type)?.label ?? String(type);
  }

  private getSeverityLabel(severity: number): string {
    return this.severityOptions.find(opt => opt.value === severity)?.label ?? String(severity);
  }

  private formatDate(dateString: string): string {
    return new Date(dateString).toLocaleDateString();
  }
}
