import { CommonModule } from '@angular/common';
import { Component, OnInit, inject } from '@angular/core';
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { finalize } from 'rxjs';
import { ProductionPlan, ProductionPlanRequest } from '../../models/production-plan';
import { ProductionPlansService } from '../../services/production-plans.service';
import { ConfirmDialogComponent } from '../../shared/components/confirm-dialog/confirm-dialog';
import { DataTableColumn, DataTableComponent } from '../../shared/components/data-table/data-table';
import { EmptyStateComponent } from '../../shared/components/empty-state/empty-state';
import { LoadingSpinnerComponent } from '../../shared/components/loading-spinner/loading-spinner';
import { PageHeaderComponent } from '../../shared/components/page-header/page-header';
import { PaginationComponent } from '../../shared/components/pagination/pagination';
import { SearchBoxComponent } from '../../shared/components/search-box/search-box';
import { getApiErrorMessage } from '../../shared/error-message';
import { productionPlanStatusOptions } from '../resource-options';

@Component({
  selector: 'app-production-plans',
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
  templateUrl: './production-plans.html',
  styleUrl: './production-plans.css',
})
export class ProductionPlansComponent implements OnInit {
  private readonly fb = inject(FormBuilder);
  private readonly productionPlansService = inject(ProductionPlansService);

  readonly statusOptions = productionPlanStatusOptions;
  readonly columns: DataTableColumn<ProductionPlan>[] = [
    { label: 'ID', value: plan => plan.planId },
    { label: 'Product', value: plan => plan.productName },
    { label: 'Target', value: plan => plan.targetQuantity },
    { label: 'Start', value: plan => this.formatDate(plan.startDate) },
    { label: 'End', value: plan => this.formatDate(plan.endDate) },
    { label: 'Status', value: plan => this.getStatusLabel(plan.status) },
    { label: 'Created By', value: plan => plan.createdBy },
  ];

  readonly form = this.fb.nonNullable.group({
    productName: ['', [Validators.required, Validators.minLength(2), Validators.maxLength(100)]],
    targetQuantity: [0, [Validators.required, Validators.min(1)]],
    startDate: ['', [Validators.required]],
    endDate: ['', [Validators.required]],
    status: [0, [Validators.required, Validators.min(1)]],
  });

  plans: ProductionPlan[] = [];
  searchTerm = '';
  pageNumber = 1;
  pageSize = 10;
  loading = false;
  saving = false;
  errorMessage = '';
  successMessage = '';
  editingPlanId: number | null = null;
  planToDelete: ProductionPlan | null = null;

  ngOnInit(): void {
    this.loadPlans();
  }

  get filteredPlans(): ProductionPlan[] {
    const term = this.searchTerm.trim().toLowerCase();
    return term
      ? this.plans.filter(plan => 
          `${plan.planId} ${plan.productName}`.toLowerCase().includes(term)
        )
      : this.plans;
  }

  loadPlans(): void {
    this.loading = true;
    this.errorMessage = '';
    this.productionPlansService
      .getProductionPlans(this.pageNumber, this.pageSize)
      .pipe(finalize(() => (this.loading = false)))
      .subscribe({
        next: plans => (this.plans = plans),
        error: err => (this.errorMessage = getApiErrorMessage(err, 'Unable to load production plans.')),
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
    const request = this.form.getRawValue() as ProductionPlanRequest;
    const request$ =
      this.editingPlanId === null
        ? this.productionPlansService.createProductionPlan(request)
        : this.productionPlansService.updateProductionPlan(this.editingPlanId, request);

    request$.pipe(finalize(() => (this.saving = false))).subscribe({
      next: () => {
        this.successMessage = this.editingPlanId === null ? 'Production plan created successfully.' : 'Production plan updated successfully.';
        this.resetForm();
        this.loadPlans();
      },
      error: err => (this.errorMessage = getApiErrorMessage(err, 'Unable to save production plan.')),
    });
  }

  editPlan(plan: ProductionPlan): void {
    this.editingPlanId = plan.planId;
    this.form.setValue({
      productName: plan.productName,
      targetQuantity: plan.targetQuantity,
      startDate: plan.startDate.split('T')[0],
      endDate: plan.endDate.split('T')[0],
      status: plan.status,
    });
  }

  requestDelete(plan: ProductionPlan): void {
    this.planToDelete = plan;
  }

  confirmDelete(): void {
    if (!this.planToDelete) {
      return;
    }

    this.productionPlansService.deleteProductionPlan(this.planToDelete.planId).subscribe({
      next: () => {
        this.successMessage = 'Production plan deleted successfully.';
        this.planToDelete = null;
        this.loadPlans();
      },
      error: err => (this.errorMessage = getApiErrorMessage(err, 'Unable to delete production plan.')),
    });
  }

  resetForm(): void {
    this.editingPlanId = null;
    this.form.reset({ productName: '', targetQuantity: 0, startDate: '', endDate: '', status: 0 });
  }

  nextPage(): void {
    this.pageNumber += 1;
    this.loadPlans();
  }

  previousPage(): void {
    if (this.pageNumber > 1) {
      this.pageNumber -= 1;
      this.loadPlans();
    }
  }

  private getStatusLabel(status: number): string {
    return this.statusOptions.find(opt => opt.value === status)?.label ?? String(status);
  }

  private formatDate(dateString: string): string {
    return new Date(dateString).toLocaleDateString();
  }
}
