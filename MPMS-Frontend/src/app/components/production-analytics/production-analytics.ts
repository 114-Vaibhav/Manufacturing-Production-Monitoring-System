import { CommonModule } from '@angular/common';
import { Component, OnInit, inject } from '@angular/core';
import { finalize } from 'rxjs';
import { ProductionAnalytics } from '../../models/production-analytics';
import { ProductionAnalyticsService } from '../../services/production-analytics.service';
import { DataTableColumn, DataTableComponent } from '../../shared/components/data-table/data-table';
import { EmptyStateComponent } from '../../shared/components/empty-state/empty-state';
import { LoadingSpinnerComponent } from '../../shared/components/loading-spinner/loading-spinner';
import { PageHeaderComponent } from '../../shared/components/page-header/page-header';
import { PaginationComponent } from '../../shared/components/pagination/pagination';
import { SearchBoxComponent } from '../../shared/components/search-box/search-box';
import { getApiErrorMessage } from '../../shared/error-message';

@Component({
  selector: 'app-production-analytics',
  standalone: true,
  imports: [
    CommonModule,
    DataTableComponent,
    EmptyStateComponent,
    LoadingSpinnerComponent,
    PageHeaderComponent,
    PaginationComponent,
    SearchBoxComponent,
  ],
  templateUrl: './production-analytics.html',
  styleUrl: './production-analytics.css',
})
export class ProductionAnalyticsComponent implements OnInit {
  private readonly productionAnalyticsService = inject(ProductionAnalyticsService);

  readonly columns: DataTableColumn<ProductionAnalytics>[] = [
    { label: 'ID', value: analytics => analytics.analyticsId },
    { label: 'Machine', value: analytics => analytics.machineId },
    { label: 'Efficiency', value: analytics => analytics.efficiency },
    { label: 'Downtime', value: analytics => analytics.downtime },
    { label: 'Defect Rate', value: analytics => analytics.defectRate },
    { label: 'Calculated', value: analytics => this.formatDate(analytics.calculatedDate) },
  ];

  analytics: ProductionAnalytics[] = [];
  searchTerm = '';
  pageNumber = 1;
  pageSize = 10;
  loading = false;
  errorMessage = '';

  ngOnInit(): void {
    this.loadProductionAnalytics();
  }

  get filteredAnalytics(): ProductionAnalytics[] {
    const term = this.searchTerm.trim().toLowerCase();
    if (!term) {
      return this.analytics;
    }

    return this.analytics.filter(item => `${item.analyticsId} ${item.machineId}`.includes(term));
  }

  loadProductionAnalytics(): void {
    this.loading = true;
    this.errorMessage = '';

    this.productionAnalyticsService
      .getProductionAnalytics(this.pageNumber, this.pageSize)
      .pipe(finalize(() => (this.loading = false)))
      .subscribe({
        next: analytics => (this.analytics = analytics),
        error: err => (this.errorMessage = getApiErrorMessage(err, 'Unable to load production analytics.')),
      });
  }

  nextPage(): void {
    this.pageNumber += 1;
    this.loadProductionAnalytics();
  }

  previousPage(): void {
    if (this.pageNumber > 1) {
      this.pageNumber -= 1;
      this.loadProductionAnalytics();
    }
  }

  private formatDate(value: string): string {
    return value ? new Date(value).toLocaleString() : '-';
  }
}
