import { CommonModule } from '@angular/common';
import { Component, OnInit, inject } from '@angular/core';
import { finalize } from 'rxjs';
import { UserService, UserLog } from '../../../services/user.service';
import { DataTableColumn, DataTableComponent } from '../../../shared/components/data-table/data-table';
import { EmptyStateComponent } from '../../../shared/components/empty-state/empty-state';
import { LoadingSpinnerComponent } from '../../../shared/components/loading-spinner/loading-spinner';
import { PageHeaderComponent } from '../../../shared/components/page-header/page-header';
import { PaginationComponent } from '../../../shared/components/pagination/pagination';
import { SearchBoxComponent } from '../../../shared/components/search-box/search-box';
import { getApiErrorMessage } from '../../../shared/error-message';

@Component({
  selector: 'app-get-logs',
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
  templateUrl: './get-logs.html',
  styleUrl: './get-logs.css',
})
export class GetLogs implements OnInit {
  private readonly userService = inject(UserService);

  readonly columns: DataTableColumn<UserLog>[] = [
    { label: 'ID', value: log => log.id },
    { label: 'User', value: log => log.username },
    { label: 'Action', value: log => log.action },
    { label: 'Resource', value: log => log.resource },
    { label: 'Timestamp', value: log => this.formatDate(log.timestamp) },
    { label: 'Details', value: log => log.details ?? '-' },
  ];

  logs: UserLog[] = [];
  searchTerm = '';
  pageNumber = 1;
  pageSize = 10;
  loading = false;
  errorMessage = '';

  ngOnInit(): void {
    this.loadLogs();
  }

  get filteredLogs(): UserLog[] {
    const term = this.searchTerm.trim().toLowerCase();
    return term
      ? this.logs.filter(log =>
          `${log.username} ${log.action} ${log.resource} ${log.details ?? ''}`.toLowerCase().includes(term)
        )
      : this.logs;
  }

  loadLogs(): void {
    this.loading = true;
    this.errorMessage = '';
    this.userService
      .getLogs(this.pageNumber, this.pageSize)
      .pipe(finalize(() => (this.loading = false)))
      .subscribe({
        next: logs => (this.logs = logs),
        error: err => (this.errorMessage = getApiErrorMessage(err, 'Unable to load logs.')),
      });
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

  private formatDate(dateString: string): string {
    return new Date(dateString).toLocaleString();
  }
}
