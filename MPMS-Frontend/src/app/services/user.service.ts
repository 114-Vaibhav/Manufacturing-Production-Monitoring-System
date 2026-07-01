import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from '../../environments/environment.development';
import { User } from '../models/user.model';

export interface CreateUserRequest {
  username: string;
  fullName: string;
  email: string;
  password: string;
  role: number;
  status: number;
}

// export interface UserLog {
//   id: number;
//   userId: number;
//   username: string;
//   action: string;
//   resource: string;
//   timestamp: string;
//   details?: string;
// }
// Update this interface in user.service.ts
export interface UserLog {
  entityId: number;      // was 'id'
  userName: string;      // was 'username'
  action: string;
  entityName: string;    // was 'resource'
  createdAt: string;     // was 'timestamp'
  userId?: number;       // (Optional since backend isn't sending it right now)
  details?: string;      // (Optional since backend isn't sending it right now)
}
@Injectable({ providedIn: 'root' })
export class UserService {
  private readonly endpoint = `${environment.apiUrl}/user-register`;
  private readonly logsEndpoint = `${environment.apiUrl}/api/admin/logs`;

  constructor(private http: HttpClient) {}

  createUser(request: CreateUserRequest): Observable<User> {
    return this.http.post<User>(this.endpoint, request);
  }

  updateUser(userId: number, request: Partial<CreateUserRequest>): Observable<User> {
    return this.http.put<User>(`${this.endpoint}/${userId}`, request);
  }

  deleteUser(userId: number): Observable<void> {
    return this.http.delete<void>(`${this.endpoint}/${userId}`);
  }

  getUsers(pageNumber: number, pageSize: number): Observable<User[]> {
    return this.http.get<User[]>(`${this.endpoint}?pageNumber=${pageNumber}&pageSize=${pageSize}`);
  }

  getUserById(userId: number): Observable<User> {
    return this.http.get<User>(`${this.endpoint}/${userId}`);
  }

  getLogs(pageNumber: number, pageSize: number): Observable<UserLog[]> {
    return this.http.get<UserLog[]>(`${this.logsEndpoint}`);
    // return this.http.get<UserLog[]>(`${this.logsEndpoint}?pageNumber=${pageNumber}&pageSize=${pageSize}`);
  }

  getLogsByUser(userId: number, pageNumber: number, pageSize: number): Observable<UserLog[]> {
    return this.http.get<UserLog[]>(`${this.logsEndpoint}/user/${userId}?pageNumber=${pageNumber}&pageSize=${pageSize}`);
  }
}
