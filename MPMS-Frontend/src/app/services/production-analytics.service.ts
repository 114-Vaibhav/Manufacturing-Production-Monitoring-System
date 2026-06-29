import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from '../../environments/environment.development';
import { ProductionAnalytics } from '../models/production-analytics';

@Injectable({ providedIn: 'root' })
export class ProductionAnalyticsService {
  private readonly endpoint = `${environment.apiUrl}/api/ProductionAnalytics`;

  constructor(private http: HttpClient) {}

  getProductionAnalytics(pageNumber: number, pageSize: number): Observable<ProductionAnalytics[]> {
    return this.http.get<ProductionAnalytics[]>(`${this.endpoint}?pageNumber=${pageNumber}&pageSize=${pageSize}`);
  }
}
