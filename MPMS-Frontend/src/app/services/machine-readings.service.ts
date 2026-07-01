import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from '../../environments/environment.development';
import { MachineReading, MachineReadingRequest } from '../models/machine-reading';

@Injectable({ providedIn: 'root' })
export class MachineReadingsService {
  private readonly endpoint = `${environment.apiUrl}/api/MachineReadings`;

  constructor(private http: HttpClient) {}

  getMachineReadings(pageNumber: number, pageSize: number): Observable<MachineReading[]> {
    return this.http.get<MachineReading[]>(`${this.endpoint}?pageNumber=${pageNumber}&pageSize=${pageSize}`);
    // const readings = this.http.get<MachineReading[]>(`${this.endpoint}`);
    // console.log(readings);
    // return readings;
  }

  updateMachineReading(readingId: number, request: MachineReadingRequest): Observable<MachineReading> {
    return this.http.put<MachineReading>(`${this.endpoint}/${readingId}`, request);
  }

  deleteMachineReading(readingId: number): Observable<MachineReading> {
    return this.http.delete<MachineReading>(`${this.endpoint}/${readingId}`);
  }
}
