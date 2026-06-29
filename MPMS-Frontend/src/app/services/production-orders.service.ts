import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from '../../environments/environment.development';
import { ProductionOrder, ProductionOrderRequest } from '../models/production-order';

@Injectable({ providedIn: 'root' })
export class ProductionOrdersService {
  private readonly endpoint = `${environment.apiUrl}/api/ProductionOrders`;

  constructor(private http: HttpClient) {}

  getProductionOrders(pageNumber: number, pageSize: number): Observable<ProductionOrder[]> {
    return this.http.get<ProductionOrder[]>(`${this.endpoint}?pageNumber=${pageNumber}&pageSize=${pageSize}`);
  }

  createProductionOrder(request: ProductionOrderRequest): Observable<ProductionOrder> {
    return this.http.post<ProductionOrder>(this.endpoint, request);
  }

  updateProductionOrder(orderId: number, request: ProductionOrderRequest): Observable<ProductionOrder> {
    return this.http.put<ProductionOrder>(`${this.endpoint}/${orderId}`, request);
  }

  deleteProductionOrder(orderId: number): Observable<ProductionOrder> {
    return this.http.delete<ProductionOrder>(`${this.endpoint}/${orderId}`);
  }
}
