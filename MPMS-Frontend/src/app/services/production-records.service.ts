import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from '../../environments/environment.development';
import { ProductionRecord, ProductionRecordRequest } from '../models/production-record';

@Injectable({ providedIn: 'root' })
export class ProductionRecordsService {
  private readonly endpoint = `${environment.apiUrl}/api/ProductionRecords`;

  constructor(private http: HttpClient) {}

  getProductionRecords(pageNumber: number, pageSize: number): Observable<ProductionRecord[]> {
    return this.http.get<ProductionRecord[]>(`${this.endpoint}?pageNumber=${pageNumber}&pageSize=${pageSize}`);
  }

  createProductionRecord(request: ProductionRecordRequest): Observable<ProductionRecord> {
    return this.http.post<ProductionRecord>(this.endpoint, request);
  }

  updateProductionRecord(recordId: number, request: ProductionRecordRequest): Observable<ProductionRecord> {
    return this.http.put<ProductionRecord>(`${this.endpoint}/${recordId}`, request);
  }

  deleteProductionRecord(recordId: number): Observable<ProductionRecord> {
    return this.http.delete<ProductionRecord>(`${this.endpoint}/${recordId}`);
  }
}
