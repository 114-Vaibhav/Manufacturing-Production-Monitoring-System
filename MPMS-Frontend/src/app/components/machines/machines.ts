import { CommonModule } from '@angular/common';
import { Component, OnInit, inject } from '@angular/core';
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { finalize } from 'rxjs';
import { Machine, MachineRequest } from '../../models/machine';
import { MachinesService } from '../../services/machines.service';
import { ConfirmDialogComponent } from '../../shared/components/confirm-dialog/confirm-dialog';
import { DataTableColumn, DataTableComponent } from '../../shared/components/data-table/data-table';
import { EmptyStateComponent } from '../../shared/components/empty-state/empty-state';
import { LoadingSpinnerComponent } from '../../shared/components/loading-spinner/loading-spinner';
import { PageHeaderComponent } from '../../shared/components/page-header/page-header';
import { PaginationComponent } from '../../shared/components/pagination/pagination';
import { SearchBoxComponent } from '../../shared/components/search-box/search-box';
import { getApiErrorMessage } from '../../shared/error-message';
import { machineStatusOptions } from '../resource-options';

@Component({
  selector: 'app-machines',
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
  templateUrl: './machines.html',
  styleUrl: './machines.css',
})
export class MachinesComponent implements OnInit {
  private readonly fb = inject(FormBuilder);
  private readonly machinesService = inject(MachinesService);

  readonly statusOptions = machineStatusOptions;
  readonly columns: DataTableColumn<Machine>[] = [
    { label: 'ID', value: machine => machine.machineId },
    { label: 'Name', value: machine => machine.machineName },
    { label: 'Code', value: machine => machine.machineCode },
    { label: 'Location', value: machine => machine.locationId },
    { label: 'Status', value: machine => this.getStatusLabel(machine.status) },
    { label: 'Last Maintenance', value: machine => this.formatDate(machine.lastMaintenanceDate) },
  ];

  readonly form = this.fb.nonNullable.group({
    machineName: ['', [Validators.required, Validators.minLength(2), Validators.maxLength(100)]],
    machineCode: ['', [Validators.required, Validators.minLength(2), Validators.maxLength(50), Validators.pattern(/^[A-Za-z0-9-]+$/)]],
    locationId: [0, [Validators.required, Validators.min(1)]],
    status: [0, [Validators.required]],
    lastMaintenanceDate: [''],
  });

  machines: Machine[] = [];
  searchTerm = '';
  pageNumber = 1;
  pageSize = 10;
  loading = false;
  saving = false;
  errorMessage = '';
  successMessage = '';
  editingMachineId: number | null = null;
  machineToDelete: Machine | null = null;

  ngOnInit(): void {
    this.loadMachines();
  }

  get filteredMachines(): Machine[] {
    const term = this.searchTerm.trim().toLowerCase();
    if (!term) {
      return this.machines;
    }

    return this.machines.filter(machine =>
      `${machine.machineName} ${machine.machineCode} ${this.getStatusLabel(machine.status)}`.toLowerCase().includes(term)
    );
  }

  loadMachines(): void {
    this.loading = true;
    this.errorMessage = '';

    this.machinesService
      .getMachines(this.pageNumber, this.pageSize)
      .pipe(finalize(() => (this.loading = false)))
      .subscribe({
        next: machines => (this.machines = machines),
        error: err => (this.errorMessage = getApiErrorMessage(err, 'Unable to load machines.')),
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

    const request = this.getRequest();
    const request$ =
      this.editingMachineId === null
        ? this.machinesService.createMachine(request)
        : this.machinesService.updateMachine(this.editingMachineId, request);

    request$.pipe(finalize(() => (this.saving = false))).subscribe({
      next: () => {
        this.successMessage = this.editingMachineId === null ? 'Machine created successfully.' : 'Machine updated successfully.';
        this.resetForm();
        this.loadMachines();
      },
      error: err => (this.errorMessage = getApiErrorMessage(err, 'Unable to save machine.')),
    });
  }

  editMachine(machine: Machine): void {
    this.editingMachineId = machine.machineId;
    this.form.setValue({
      machineName: machine.machineName,
      machineCode: machine.machineCode,
      locationId: machine.locationId,
      status: machine.status,
      lastMaintenanceDate: machine.lastMaintenanceDate?.slice(0, 10) ?? '',
    });
  }

  requestDelete(machine: Machine): void {
    this.machineToDelete = machine;
  }

  confirmDelete(): void {
    if (!this.machineToDelete) {
      return;
    }

    this.machinesService.deleteMachine(this.machineToDelete.machineId).subscribe({
      next: () => {
        this.successMessage = 'Machine deleted successfully.';
        this.machineToDelete = null;
        this.loadMachines();
      },
      error: err => (this.errorMessage = getApiErrorMessage(err, 'Unable to delete machine.')),
    });
  }

  resetForm(): void {
    this.editingMachineId = null;
    this.form.reset({
      machineName: '',
      machineCode: '',
      locationId: 0,
      status: 0,
      lastMaintenanceDate: '',
    });
  }

  nextPage(): void {
    this.pageNumber += 1;
    this.loadMachines();
  }

  previousPage(): void {
    if (this.pageNumber > 1) {
      this.pageNumber -= 1;
      this.loadMachines();
    }
  }

  private getRequest(): MachineRequest {
    const value = this.form.getRawValue();
    return {
      machineName: value.machineName,
      machineCode: value.machineCode,
      locationId: value.locationId,
      status: value.status,
      lastMaintenanceDate: value.lastMaintenanceDate || null,
    };
  }

  private getStatusLabel(status: number): string {
    return this.statusOptions.find(option => option.value === status)?.label ?? String(status);
  }

  private formatDate(value: string | null): string {
    return value ? new Date(value).toLocaleDateString() : '-';
  }
}
