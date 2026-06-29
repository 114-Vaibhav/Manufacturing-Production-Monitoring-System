import { CommonModule } from '@angular/common';
import { Component, OnInit, inject } from '@angular/core';
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { finalize } from 'rxjs';
import { MaintenanceLog } from '../../models/maintenance-log';
import { MaintenanceLogsService } from '../../services/maintenance-logs.service';
import { ConfirmDialogComponent } from '../../shared/components/confirm-dialog/confirm-dialog';
import { DataTableColumn, DataTableComponent } from '../../shared/components/data-table/data-table';
import { EmptyStateComponent } from '../../shared/components/empty-state/empty-state';
import { LoadingSpinnerComponent } from '../../shared/components/loading-spinner/loading-spinner';
import { PageHeaderComponent } from '../../shared/components/page-header/page-header';
import { PaginationComponent } from '../../shared/components/pagination/pagination';
import { SearchBoxComponent } from '../../shared/components/search-box/search-box';
import { getApiErrorMessage } from '../../shared/error-message';

@Component({
  selector: 'app-maintenance-logs',
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
  templateUrl: './maintenance-logs.html',
  styleUrl: './maintenance-logs.css',
})
export class MaintenanceLogsComponent implements OnInit {
  private readonly fb = inject(FormBuilder);
  private readonly maintenanceLogsService = inject(MaintenanceLogsService);

  readonly columns: DataTableColumn<MaintenanceLog>[] = [
    { label: 'ID', value: log => log.logId },
    { label: 'Machine', value: log => log.machineId },
    { label: 'Engineer', value: log => log.engineerId },
    { label: 'Issue', value: log => log.issueDescription },
    { label: 'Resolution', value: log => log.resolution },
    { label: 'Date', value: log => this.formatDate(log.maintenanceDate) },
  ];

  readonly form = this.fb.nonNullable.group({
    machineId: [0, [Validators.required, Validators.min(1)]],
    issueDescription: ['', [Validators.required, Validators.minLength(5), Validators.maxLength(500)]],
    resolution: ['', [Validators.required, Validators.minLength(3), Validators.maxLength(500)]],
  });

  logs: MaintenanceLog[] = [];
  searchTerm = '';
  pageNumber = 1;
  pageSize = 10;
  loading = false;
  saving = false;
  errorMessage = '';
  successMessage = '';
  editingLogId: number | null = null;
  logToDelete: MaintenanceLog | null = null;

  ngOnInit(): void {
    this.loadLogs();
  }

  get filteredLogs(): MaintenanceLog[] {
    const term = this.searchTerm.trim().toLowerCase();
    return term
      ? this.logs.filter(log => `${log.machineId} ${log.issueDescription} ${log.resolution}`.toLowerCase().includes(term))
      : this.logs;
  }

  loadLogs(): void {
    this.loading = true;
    this.errorMessage = '';
    this.maintenanceLogsService
      .getMaintenanceLogs(this.pageNumber, this.pageSize)
      .pipe(finalize(() => (this.loading = false)))
      .subscribe({
        next: logs => (this.logs = logs),
        error: err => (this.errorMessage = getApiErrorMessage(err, 'Unable to load maintenance logs.')),
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
    const request = this.form.getRawValue();
    const request$ =
      this.editingLogId === null
        ? this.maintenanceLogsService.createMaintenanceLog(request)
        : this.maintenanceLogsService.updateMaintenanceLog(this.editingLogId, request);

    request$.pipe(finalize(() => (this.saving = false))).subscribe({
      next: () => {
        this.successMessage = this.editingLogId === null ? 'Maintenance log created successfully.' : 'Maintenance log updated successfully.';
        this.resetForm();
        this.loadLogs();
      },
      error: err => (this.errorMessage = getApiErrorMessage(err, 'Unable to save maintenance log.')),
    });
  }

  editLog(log: MaintenanceLog): void {
    this.editingLogId = log.logId;
    this.form.setValue({
      machineId: log.machineId,
      issueDescription: log.issueDescription,
      resolution: log.resolution,
    });
  }

  requestDelete(log: MaintenanceLog): void {
    this.logToDelete = log;
  }

  confirmDelete(): void {
    if (!this.logToDelete) {
      return;
    }

    this.maintenanceLogsService.deleteMaintenanceLog(this.logToDelete.logId).subscribe({
      next: () => {
        this.successMessage = 'Maintenance log deleted successfully.';
        this.logToDelete = null;
        this.loadLogs();
      },
      error: err => (this.errorMessage = getApiErrorMessage(err, 'Unable to delete maintenance log.')),
    });
  }

  resetForm(): void {
    this.editingLogId = null;
    this.form.reset({ machineId: 0, issueDescription: '', resolution: '' });
  }

  nextPage(): void {
    this.pageNumber += 1;
    this.loadLogs();
  }

  previousPage(): void {
    if (this.pageNumber > 1) {
      this.pageNumber -= 1;
      this.loadLogs();
    }
  }

  private formatDate(value: string): string {
    return value ? new Date(value).toLocaleString() : '-';
  }
}
