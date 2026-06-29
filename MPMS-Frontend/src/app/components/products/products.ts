import { CommonModule } from '@angular/common';
import { Component, OnInit, inject } from '@angular/core';
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { finalize } from 'rxjs';
import { Product, ProductRequest } from '../../models/product';
import { ProductsService } from '../../services/products.service';
import { ConfirmDialogComponent } from '../../shared/components/confirm-dialog/confirm-dialog';
import { DataTableColumn, DataTableComponent } from '../../shared/components/data-table/data-table';
import { EmptyStateComponent } from '../../shared/components/empty-state/empty-state';
import { LoadingSpinnerComponent } from '../../shared/components/loading-spinner/loading-spinner';
import { PageHeaderComponent } from '../../shared/components/page-header/page-header';
import { PaginationComponent } from '../../shared/components/pagination/pagination';
import { SearchBoxComponent } from '../../shared/components/search-box/search-box';
import { getApiErrorMessage } from '../../shared/error-message';
import { productStatusOptions } from '../resource-options';

@Component({
  selector: 'app-products',
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
  templateUrl: './products.html',
  styleUrl: './products.css',
})
export class ProductsComponent implements OnInit {
  private readonly fb = inject(FormBuilder);
  private readonly productsService = inject(ProductsService);

  readonly statusOptions = productStatusOptions;
  readonly columns: DataTableColumn<Product>[] = [
    { label: 'ID', value: product => product.productId },
    { label: 'Name', value: product => product.productName },
    { label: 'Code', value: product => product.productCode },
    { label: 'Price', value: product => product.unitPrice },
    { label: 'Status', value: product => product.status },
    { label: 'Created', value: product => this.formatDate(product.createdAt) },
  ];

  readonly form = this.fb.nonNullable.group({
    productName: ['', [Validators.required, Validators.minLength(2), Validators.maxLength(100)]],
    productCode: ['', [Validators.required, Validators.minLength(2), Validators.maxLength(50), Validators.pattern(/^[A-Za-z0-9-]+$/)]],
    description: ['', [Validators.required, Validators.minLength(5), Validators.maxLength(500)]],
    unitPrice: [0, [Validators.required, Validators.min(0.01)]],
    status: ['Active', [Validators.required]],
  });

  products: Product[] = [];
  searchTerm = '';
  pageNumber = 1;
  pageSize = 10;
  loading = false;
  saving = false;
  errorMessage = '';
  successMessage = '';
  editingProductId: number | null = null;
  productToDelete: Product | null = null;

  ngOnInit(): void {
    this.loadProducts();
  }

  get filteredProducts(): Product[] {
    const term = this.searchTerm.trim().toLowerCase();
    if (!term) {
      return this.products;
    }

    return this.products.filter(product =>
      `${product.productName} ${product.productCode} ${product.status}`.toLowerCase().includes(term)
    );
  }

  loadProducts(): void {
    this.loading = true;
    this.errorMessage = '';

    this.productsService
      .getProducts(this.pageNumber, this.pageSize)
      .pipe(finalize(() => (this.loading = false)))
      .subscribe({
        next: products => (this.products = products),
        error: err => (this.errorMessage = getApiErrorMessage(err, 'Unable to load products.')),
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
      this.editingProductId === null
        ? this.productsService.createProduct(request)
        : this.productsService.updateProduct(this.editingProductId, request);

    request$.pipe(finalize(() => (this.saving = false))).subscribe({
      next: () => {
        this.successMessage = this.editingProductId === null ? 'Product created successfully.' : 'Product updated successfully.';
        this.resetForm();
        this.loadProducts();
      },
      error: err => (this.errorMessage = getApiErrorMessage(err, 'Unable to save product.')),
    });
  }

  editProduct(product: Product): void {
    this.editingProductId = product.productId;
    this.form.setValue({
      productName: product.productName,
      productCode: product.productCode,
      description: product.description,
      unitPrice: product.unitPrice,
      status: product.status,
    });
  }

  requestDelete(product: Product): void {
    this.productToDelete = product;
  }

  confirmDelete(): void {
    if (!this.productToDelete) {
      return;
    }

    this.productsService.deleteProduct(this.productToDelete.productId).subscribe({
      next: () => {
        this.successMessage = 'Product deleted successfully.';
        this.productToDelete = null;
        this.loadProducts();
      },
      error: err => (this.errorMessage = getApiErrorMessage(err, 'Unable to delete product.')),
    });
  }

  resetForm(): void {
    this.editingProductId = null;
    this.form.reset({
      productName: '',
      productCode: '',
      description: '',
      unitPrice: 0,
      status: 'Active',
    });
  }

  nextPage(): void {
    this.pageNumber += 1;
    this.loadProducts();
  }

  previousPage(): void {
    if (this.pageNumber > 1) {
      this.pageNumber -= 1;
      this.loadProducts();
    }
  }

  private getRequest(): ProductRequest {
    return this.form.getRawValue();
  }

  private formatDate(value: string): string {
    return value ? new Date(value).toLocaleString() : '-';
  }
}
