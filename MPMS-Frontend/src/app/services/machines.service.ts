import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from '../../environments/environment.development';
import { Machine, MachineRequest } from '../models/machine';

@Injectable({ providedIn: 'root' })
export class MachinesService {
  private readonly endpoint = `${environment.apiUrl}/api/Machines`;

  constructor(private http: HttpClient) {}

  getMachines(pageNumber: number, pageSize: number): Observable<Machine[]> {
    return this.http.get<Machine[]>(`${this.endpoint}?pageNumber=${pageNumber}&pageSize=${pageSize}`);
  }

  createMachine(request: MachineRequest): Observable<Machine> {
    return this.http.post<Machine>(this.endpoint, request);
  }

  updateMachine(machineId: number, request: MachineRequest): Observable<Machine> {
    return this.http.put<Machine>(`${this.endpoint}/${machineId}`, request);
  }

  deleteMachine(machineId: number): Observable<Machine> {
    return this.http.delete<Machine>(`${this.endpoint}/${machineId}`);
  }
}
