import { CommonModule } from '@angular/common';
import { Component, OnInit, inject } from '@angular/core';
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { finalize } from 'rxjs';
import { ProductionOrder, ProductionOrderRequest } from '../../models/production-order';
import { ProductionOrdersService } from '../../services/production-orders.service';
import { ConfirmDialogComponent } from '../../shared/components/confirm-dialog/confirm-dialog';
import { DataTableColumn, DataTableComponent } from '../../shared/components/data-table/data-table';
import { EmptyStateComponent } from '../../shared/components/empty-state/empty-state';
import { LoadingSpinnerComponent } from '../../shared/components/loading-spinner/loading-spinner';
import { PageHeaderComponent } from '../../shared/components/page-header/page-header';
import { PaginationComponent } from '../../shared/components/pagination/pagination';
import { SearchBoxComponent } from '../../shared/components/search-box/search-box';
import { getApiErrorMessage } from '../../shared/error-message';
import { productionStatusOptions } from '../resource-options';

@Component({
  selector: 'app-production-orders',
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
  templateUrl: './production-orders.html',
  styleUrl: './production-orders.css',
})
export class ProductionOrdersComponent implements OnInit {
  private readonly fb = inject(FormBuilder);
  private readonly productionOrdersService = inject(ProductionOrdersService);

  readonly statusOptions = productionStatusOptions;
  readonly columns: DataTableColumn<ProductionOrder>[] = [
    { label: 'ID', value: order => order.orderId },
    { label: 'Plan', value: order => order.planId },
    { label: 'Machine', value: order => order.machineId },
    { label: 'Quantity', value: order => order.quantity },
    { label: 'Produced', value: order => order.producedQuantity },
    { label: 'Status', value: order => this.getStatusLabel(order.status) },
  ];

  readonly form = this.fb.nonNullable.group({
    planId: [0, [Validators.required, Validators.min(1)]],
    machineId: [0, [Validators.required, Validators.min(1)]],
    quantity: [0, [Validators.required, Validators.min(1)]],
    producedQuantity: [0, [Validators.required, Validators.min(0)]],
    status: [0, [Validators.required, Validators.min(1)]],
  });

  orders: ProductionOrder[] = [];
  searchTerm = '';
  pageNumber = 1;
  pageSize = 10;
  loading = false;
  saving = false;
  errorMessage = '';
  successMessage = '';
  editingOrderId: number | null = null;
  orderToDelete: ProductionOrder | null = null;

  ngOnInit(): void {
    this.loadOrders();
  }

  get filteredOrders(): ProductionOrder[] {
    const term = this.searchTerm.trim().toLowerCase();
    return term
      ? this.orders.filter(order => 
          `${order.orderId} ${order.planId} ${order.machineId}`.toLowerCase().includes(term)
        )
      : this.orders;
  }

  loadOrders(): void {
    this.loading = true;
    this.errorMessage = '';
    this.productionOrdersService
      .getProductionOrders(this.pageNumber, this.pageSize)
      .pipe(finalize(() => (this.loading = false)))
      .subscribe({
        next: orders => (this.orders = orders),
        error: err => (this.errorMessage = getApiErrorMessage(err, 'Unable to load production orders.')),
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
    const request = this.form.getRawValue() as ProductionOrderRequest;
    const request$ =
      this.editingOrderId === null
        ? this.productionOrdersService.createProductionOrder(request)
        : this.productionOrdersService.updateProductionOrder(this.editingOrderId, request);

    request$.pipe(finalize(() => (this.saving = false))).subscribe({
      next: () => {
        this.successMessage = this.editingOrderId === null ? 'Production order created successfully.' : 'Production order updated successfully.';
        this.resetForm();
        this.loadOrders();
      },
      error: err => (this.errorMessage = getApiErrorMessage(err, 'Unable to save production order.')),
    });
  }

  editOrder(order: ProductionOrder): void {
    this.editingOrderId = order.orderId;
    this.form.setValue({
      planId: order.planId,
      machineId: order.machineId,
      quantity: order.quantity,
      producedQuantity: order.producedQuantity,
      status: order.status,
    });
  }

  requestDelete(order: ProductionOrder): void {
    this.orderToDelete = order;
  }

  confirmDelete(): void {
    if (!this.orderToDelete) {
      return;
    }

    this.productionOrdersService.deleteProductionOrder(this.orderToDelete.orderId).subscribe({
      next: () => {
        this.successMessage = 'Production order deleted successfully.';
        this.orderToDelete = null;
        this.loadOrders();
      },
      error: err => (this.errorMessage = getApiErrorMessage(err, 'Unable to delete production order.')),
    });
  }

  resetForm(): void {
    this.editingOrderId = null;
    this.form.reset({ planId: 0, machineId: 0, quantity: 0, producedQuantity: 0, status: 0 });
  }

  nextPage(): void {
    this.pageNumber += 1;
    this.loadOrders();
  }

  previousPage(): void {
    if (this.pageNumber > 1) {
      this.pageNumber -= 1;
      this.loadOrders();
    }
  }

  private getStatusLabel(status: number): string {
    return this.statusOptions.find(opt => opt.value === status)?.label ?? String(status);
  }
}
