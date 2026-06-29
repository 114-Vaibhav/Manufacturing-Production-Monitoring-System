import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from '../../environments/environment.development';
import { Defect, DefectRequest } from '../models/defect';

@Injectable({ providedIn: 'root' })
export class DefectsService {
  private readonly endpoint = `${environment.apiUrl}/api/Defects`;

  constructor(private http: HttpClient) {}

  getDefects(pageNumber: number, pageSize: number): Observable<Defect[]> {
    return this.http.get<Defect[]>(`${this.endpoint}?pageNumber=${pageNumber}&pageSize=${pageSize}`);
  }

  createDefect(request: DefectRequest): Observable<Defect> {
    return this.http.post<Defect>(this.endpoint, request);
  }

  updateDefect(defectId: number, request: DefectRequest): Observable<Defect> {
    return this.http.put<Defect>(`${this.endpoint}/${defectId}`, request);
  }

  deleteDefect(defectId: number): Observable<Defect> {
    return this.http.delete<Defect>(`${this.endpoint}/${defectId}`);
  }
}
