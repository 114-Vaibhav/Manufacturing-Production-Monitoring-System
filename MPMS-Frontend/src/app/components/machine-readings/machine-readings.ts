import { CommonModule } from '@angular/common';
import { Component, OnInit, inject } from '@angular/core';
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { finalize } from 'rxjs';
import { MachineReading, MachineReadingRequest } from '../../models/machine-reading';
import { MachineReadingsService } from '../../services/machine-readings.service';
import { ConfirmDialogComponent } from '../../shared/components/confirm-dialog/confirm-dialog';
import { DataTableColumn, DataTableComponent } from '../../shared/components/data-table/data-table';
import { EmptyStateComponent } from '../../shared/components/empty-state/empty-state';
import { LoadingSpinnerComponent } from '../../shared/components/loading-spinner/loading-spinner';
import { PageHeaderComponent } from '../../shared/components/page-header/page-header';
import { PaginationComponent } from '../../shared/components/pagination/pagination';
import { SearchBoxComponent } from '../../shared/components/search-box/search-box';
import { getApiErrorMessage } from '../../shared/error-message';

@Component({
  selector: 'app-machine-readings',
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
  templateUrl: './machine-readings.html',
  styleUrl: './machine-readings.css',
})
export class MachineReadingsComponent implements OnInit {
  private readonly fb = inject(FormBuilder);
  private readonly machineReadingsService = inject(MachineReadingsService);

  readonly columns: DataTableColumn<MachineReading>[] = [
    { label: 'ID', value: reading => reading.readingId },
    { label: 'Machine', value: reading => reading.machineId },
    { label: 'Temperature', value: reading => reading.temperature },
    { label: 'Vibration', value: reading => reading.vibration },
    { label: 'Power', value: reading => reading.powerConsumption },
    { label: 'Timestamp', value: reading => this.formatDate(reading.timestamp) },
  ];

  readonly form = this.fb.nonNullable.group({
    machineId: [0, [Validators.required, Validators.min(1)]],
    temperature: [0, [Validators.required, Validators.min(-50), Validators.max(250)]],
    vibration: [0, [Validators.required, Validators.min(0)]],
    powerConsumption: [0, [Validators.required, Validators.min(0)]],
  });

  readings: MachineReading[] = [];
  searchTerm = '';
  pageNumber = 1;
  pageSize = 10;
  loading = false;
  saving = false;
  errorMessage = '';
  successMessage = '';
  editingReadingId: number | null = null;
  readingToDelete: MachineReading | null = null;

  ngOnInit(): void {
    this.loadReadings();
  }

  get filteredReadings(): MachineReading[] {
    const term = this.searchTerm.trim().toLowerCase();
    return term ? this.readings.filter(reading => `${reading.readingId} ${reading.machineId}`.includes(term)) : this.readings;
  }

  loadReadings(): void {
    this.loading = true;
    this.errorMessage = '';
    this.machineReadingsService
      .getMachineReadings(this.pageNumber, this.pageSize)
      .pipe(finalize(() => (this.loading = false)))
      .subscribe({
        next: readings => (this.readings = readings),
        error: err => (this.errorMessage = getApiErrorMessage(err, 'Unable to load machine readings.')),
      });
  }

  submit(): void {
    if (this.editingReadingId === null) {
      this.errorMessage = 'Select a reading before updating.';
      return;
    }

    if (this.form.invalid) {
      this.form.markAllAsTouched();
      return;
    }

    this.saving = true;
    this.errorMessage = '';
    this.successMessage = '';
    this.machineReadingsService
      .updateMachineReading(this.editingReadingId, this.form.getRawValue())
      .pipe(finalize(() => (this.saving = false)))
      .subscribe({
        next: () => {
          this.successMessage = 'Machine reading updated successfully.';
          this.resetForm();
          this.loadReadings();
        },
        error: err => (this.errorMessage = getApiErrorMessage(err, 'Unable to update machine reading.')),
      });
  }

  editReading(reading: MachineReading): void {
    this.editingReadingId = reading.readingId;
    this.form.setValue({
      machineId: reading.machineId,
      temperature: reading.temperature,
      vibration: reading.vibration,
      powerConsumption: reading.powerConsumption,
    });
  }

  requestDelete(reading: MachineReading): void {
    this.readingToDelete = reading;
  }

  confirmDelete(): void {
    if (!this.readingToDelete) {
      return;
    }

    this.machineReadingsService.deleteMachineReading(this.readingToDelete.readingId).subscribe({
      next: () => {
        this.successMessage = 'Machine reading deleted successfully.';
        this.readingToDelete = null;
        this.loadReadings();
      },
      error: err => (this.errorMessage = getApiErrorMessage(err, 'Unable to delete machine reading.')),
    });
  }

  resetForm(): void {
    this.editingReadingId = null;
    this.form.reset({ machineId: 0, temperature: 0, vibration: 0, powerConsumption: 0 });
  }

  nextPage(): void {
    this.pageNumber += 1;
    this.loadReadings();
  }

  previousPage(): void {
    if (this.pageNumber > 1) {
      this.pageNumber -= 1;
      this.loadReadings();
    }
  }

  private formatDate(value: string): string {
    return value ? new Date(value).toLocaleString() : '-';
  }
}
