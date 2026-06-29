import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from '../../environments/environment.development';
import { ProductionPlan, ProductionPlanRequest } from '../models/production-plan';

@Injectable({ providedIn: 'root' })
export class ProductionPlansService {
  private readonly endpoint = `${environment.apiUrl}/api/ProductionPlans`;

  constructor(private http: HttpClient) {}

  getProductionPlans(pageNumber: number, pageSize: number): Observable<ProductionPlan[]> {
    return this.http.get<ProductionPlan[]>(`${this.endpoint}?pageNumber=${pageNumber}&pageSize=${pageSize}`);
  }

  createProductionPlan(request: ProductionPlanRequest): Observable<ProductionPlan> {
    return this.http.post<ProductionPlan>(this.endpoint, request);
  }

  updateProductionPlan(planId: number, request: ProductionPlanRequest): Observable<ProductionPlan> {
    return this.http.put<ProductionPlan>(`${this.endpoint}/${planId}`, request);
  }

  deleteProductionPlan(planId: number): Observable<ProductionPlan> {
    return this.http.delete<ProductionPlan>(`${this.endpoint}/${planId}`);
  }
}
