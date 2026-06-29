import { CommonModule } from '@angular/common';
import { Component, EventEmitter, Input, Output } from '@angular/core';

export interface DataTableColumn<T extends object> {
  label: string;
  value: (row: T) => string | number;
}

@Component({
  selector: 'app-data-table',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './data-table.html',
  styleUrl: './data-table.css',
})
export class DataTableComponent<T extends object> {
  @Input() columns: DataTableColumn<T>[] = [];
  @Input() rows: T[] = [];
  @Input() canEdit = true;
  @Input() canDelete = true;
  @Output() edit = new EventEmitter<T>();
  @Output() delete = new EventEmitter<T>();
}
