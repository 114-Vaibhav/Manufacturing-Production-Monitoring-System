import { CommonModule } from '@angular/common';
import { Component, OnInit, inject } from '@angular/core';
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { finalize } from 'rxjs';
import { ProductionRecord, ProductionRecordRequest } from '../../models/production-record';
import { ProductionRecordsService } from '../../services/production-records.service';
import { ConfirmDialogComponent } from '../../shared/components/confirm-dialog/confirm-dialog';
import { DataTableColumn, DataTableComponent } from '../../shared/components/data-table/data-table';
import { EmptyStateComponent } from '../../shared/components/empty-state/empty-state';
import { LoadingSpinnerComponent } from '../../shared/components/loading-spinner/loading-spinner';
import { PageHeaderComponent } from '../../shared/components/page-header/page-header';
import { PaginationComponent } from '../../shared/components/pagination/pagination';
import { SearchBoxComponent } from '../../shared/components/search-box/search-box';
import { getApiErrorMessage } from '../../shared/error-message';

@Component({
  selector: 'app-production-records',
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
  templateUrl: './production-records.html',
  styleUrl: './production-records.css',
})
export class ProductionRecordsComponent implements OnInit {
  private readonly fb = inject(FormBuilder);
  private readonly productionRecordsService = inject(ProductionRecordsService);

  readonly columns: DataTableColumn<ProductionRecord>[] = [
    { label: 'ID', value: record => record.id },
    { label: 'Plan', value: record => record.productionPlanId },
    { label: 'Produced Quantity', value: record => record.producedQuantity },
    { label: 'Production Date', value: record => this.formatDate(record.productionDate) },
  ];

  readonly form = this.fb.nonNullable.group({
    productionPlanId: [0, [Validators.required, Validators.min(1)]],
    producedQuantity: [0, [Validators.required, Validators.min(0)]],
  });

  records: ProductionRecord[] = [];
  searchTerm = '';
  pageNumber = 1;
  pageSize = 10;
  loading = false;
  saving = false;
  errorMessage = '';
  successMessage = '';
  editingRecordId: number | null = null;
  recordToDelete: ProductionRecord | null = null;

  ngOnInit(): void {
    this.loadRecords();
  }

  get filteredRecords(): ProductionRecord[] {
    const term = this.searchTerm.trim().toLowerCase();
    return term
      ? this.records.filter(record => 
          `${record.id} ${record.productionPlanId}`.toLowerCase().includes(term)
        )
      : this.records;
  }

  loadRecords(): void {
    this.loading = true;
    this.errorMessage = '';
    this.productionRecordsService
      .getProductionRecords(this.pageNumber, this.pageSize)
      .pipe(finalize(() => (this.loading = false)))
      .subscribe({
        next: records => (this.records = records),
        error: err => (this.errorMessage = getApiErrorMessage(err, 'Unable to load production records.')),
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
    const request = this.form.getRawValue() as ProductionRecordRequest;
    const request$ =
      this.editingRecordId === null
        ? this.productionRecordsService.createProductionRecord(request)
        : this.productionRecordsService.updateProductionRecord(this.editingRecordId, request);

    request$.pipe(finalize(() => (this.saving = false))).subscribe({
      next: () => {
        this.successMessage = this.editingRecordId === null ? 'Production record created successfully.' : 'Production record updated successfully.';
        this.resetForm();
        this.loadRecords();
      },
      error: err => (this.errorMessage = getApiErrorMessage(err, 'Unable to save production record.')),
    });
  }

  editRecord(record: ProductionRecord): void {
    this.editingRecordId = record.id;
    this.form.setValue({
      productionPlanId: record.productionPlanId,
      producedQuantity: record.producedQuantity,
    });
  }

  requestDelete(record: ProductionRecord): void {
    this.recordToDelete = record;
  }

  confirmDelete(): void {
    if (!this.recordToDelete) {
      return;
    }

    this.productionRecordsService.deleteProductionRecord(this.recordToDelete.id).subscribe({
      next: () => {
        this.successMessage = 'Production record deleted successfully.';
        this.recordToDelete = null;
        this.loadRecords();
      },
      error: err => (this.errorMessage = getApiErrorMessage(err, 'Unable to delete production record.')),
    });
  }

  resetForm(): void {
    this.editingRecordId = null;
    this.form.reset({ productionPlanId: 0, producedQuantity: 0 });
  }

  nextPage(): void {
    this.pageNumber += 1;
    this.loadRecords();
  }

  previousPage(): void {
    if (this.pageNumber > 1) {
      this.pageNumber -= 1;
      this.loadRecords();
    }
  }

  private formatDate(dateString: string): string {
    return new Date(dateString).toLocaleDateString();
  }
}
