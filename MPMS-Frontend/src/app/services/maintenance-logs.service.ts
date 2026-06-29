import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from '../../environments/environment.development';
import { MaintenanceLog, MaintenanceLogRequest } from '../models/maintenance-log';

@Injectable({ providedIn: 'root' })
export class MaintenanceLogsService {
  private readonly endpoint = `${environment.apiUrl}/api/MaintenanceLogs`;

  constructor(private http: HttpClient) {}

  getMaintenanceLogs(pageNumber: number, pageSize: number): Observable<MaintenanceLog[]> {
    return this.http.get<MaintenanceLog[]>(`${this.endpoint}?pageNumber=${pageNumber}&pageSize=${pageSize}`);
  }

  createMaintenanceLog(request: MaintenanceLogRequest): Observable<MaintenanceLog> {
    return this.http.post<MaintenanceLog>(this.endpoint, request);
  }

  updateMaintenanceLog(logId: number, request: MaintenanceLogRequest): Observable<MaintenanceLog> {
    return this.http.put<MaintenanceLog>(`${this.endpoint}/${logId}`, request);
  }

  deleteMaintenanceLog(logId: number): Observable<MaintenanceLog> {
    return this.http.delete<MaintenanceLog>(`${this.endpoint}/${logId}`);
  }
}
