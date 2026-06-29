import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from '../../environments/environment.development';
import { Product, ProductRequest } from '../models/product';

@Injectable({ providedIn: 'root' })
export class ProductsService {
  private readonly endpoint = `${environment.apiUrl}/api/Products`;

  constructor(private http: HttpClient) {}

  getProducts(pageNumber: number, pageSize: number): Observable<Product[]> {
    return this.http.get<Product[]>(`${this.endpoint}?pageNumber=${pageNumber}&pageSize=${pageSize}`);
  }

  createProduct(request: ProductRequest): Observable<Product> {
    return this.http.post<Product>(this.endpoint, request);
  }

  updateProduct(productId: number, request: ProductRequest): Observable<Product> {
    return this.http.put<Product>(`${this.endpoint}/${productId}`, request);
  }

  deleteProduct(productId: number): Observable<Product> {
    return this.http.delete<Product>(`${this.endpoint}/${productId}`);
  }
}
