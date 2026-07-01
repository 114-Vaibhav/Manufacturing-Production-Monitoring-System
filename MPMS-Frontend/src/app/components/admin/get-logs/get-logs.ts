import { CommonModule } from '@angular/common';
import { finalize } from 'rxjs';
import { UserService, UserLog } from '../../../services/user.service';
import { DataTableColumn, DataTableComponent } from '../../../shared/components/data-table/data-table';
import { EmptyStateComponent } from '../../../shared/components/empty-state/empty-state';
import { LoadingSpinnerComponent } from '../../../shared/components/loading-spinner/loading-spinner';
import { PageHeaderComponent } from '../../../shared/components/page-header/page-header';
import { PaginationComponent } from '../../../shared/components/pagination/pagination';
import { SearchBoxComponent } from '../../../shared/components/search-box/search-box';
import { getApiErrorMessage } from '../../../shared/error-message';
import { Component, OnInit, inject, ChangeDetectorRef } from '@angular/core';
  // ...
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
  private readonly cdr = inject(ChangeDetectorRef); // 1. Inject ChangeDetectorRef


  // NOTE: Ensure these match the exact casing of your C# response
  readonly columns: DataTableColumn<UserLog>[] = [
    { label: 'ID', value: log => log.entityId ?? '-' },
    { label: 'User', value: log => log.userName ?? 'System' },
    { label: 'Action', value: log => log.action ?? '-' },
    { label: 'Resource', value: log => log.entityName ?? '-' },
    { label: 'Timestamp', value: log => this.formatDate(log.createdAt) },
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
    // 1. Prevent crash if logs isn't an array yet
    if (!Array.isArray(this.logs)) return [];

    // 2. Prevent crash if searchTerm is undefined/null
    const term = (this.searchTerm || '').trim().toLowerCase();

    return term
      ? this.logs.filter(log =>
          // 3. Prevent crash if any object properties are missing
          `${log?.userName || ''} ${log?.action || ''} ${log?.entityName || ''} ${log?.details || ''}`
            .toLowerCase()
            .includes(term)
        )
      : this.logs;
  }

  // ... rest of your variables

  loadLogs(): void {
    this.loading = true;
    this.errorMessage = '';
    
    this.userService
      .getLogs(this.pageNumber, this.pageSize)
      .pipe(finalize(() => {
         this.loading = false;
         this.cdr.detectChanges(); // 2. Force Angular to redraw the screen
      }))
      .subscribe({
        next: (response: any) => {
          this.logs = Array.isArray(response) ? response : (response?.data || response?.items || []);
          this.cdr.detectChanges(); // 3. Force update when data arrives
        },
        error: err => {
          this.errorMessage = getApiErrorMessage(err, 'Unable to load logs.');
          this.cdr.detectChanges(); // 4. Force update on error
        },
      });
  }

  // loadLogs(): void {
  //   this.loading = true;
  //   this.errorMessage = '';
  //   this.userService
  //     .getLogs(this.pageNumber, this.pageSize)
  //     .pipe(finalize(() => (this.loading = false)))
  //     .subscribe({
  //       next: (response: any) => {
  //         // Fallback parsing just in case it's wrapped in an object
  //         this.logs = Array.isArray(response) ? response : (response?.data || response?.items || []);
  //       },
  //       error: err => {
  //         this.errorMessage = getApiErrorMessage(err, 'Unable to load logs.');
  //       },
  //     });
  // }

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

  private formatDate(dateString: string | undefined): string {
    if (!dateString) return '-';
    const date = new Date(dateString);
    // Prevent "Invalid Date" from rendering or crashing
    return isNaN(date.getTime()) ? '-' : date.toLocaleString();
  }
}
