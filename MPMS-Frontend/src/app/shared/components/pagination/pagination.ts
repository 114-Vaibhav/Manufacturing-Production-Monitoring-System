import { Component, EventEmitter, Input, Output } from '@angular/core';

@Component({
  selector: 'app-pagination',
  standalone: true,
  templateUrl: './pagination.html',
  styleUrl: './pagination.css',
})
export class PaginationComponent {
  @Input() pageNumber = 1;
  @Input() pageSize = 10;
  @Input() itemCount = 0;
  @Input() loading = false;
  @Output() previousPage = new EventEmitter<void>();
  @Output() nextPage = new EventEmitter<void>();

  get canGoNext(): boolean {
    return this.itemCount >= this.pageSize && !this.loading;
  }

  get canGoPrevious(): boolean {
    return this.pageNumber > 1 && !this.loading;
  }
}
