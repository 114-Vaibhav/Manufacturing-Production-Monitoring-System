import { Component, EventEmitter, Input, Output } from '@angular/core';
import { FormsModule } from '@angular/forms';

@Component({
  selector: 'app-search-box',
  standalone: true,
  imports: [FormsModule],
  templateUrl: './search-box.html',
  styleUrl: './search-box.css',
})
export class SearchBoxComponent {
  @Input() placeholder = 'Search';
  @Input() value = '';
  @Output() valueChange = new EventEmitter<string>();
}
