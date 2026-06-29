import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from '../../environments/environment.development';
import { LoginModel } from '../models/login.model';
import { User } from '../models/user.model';

@Injectable({ providedIn: 'root' })
export class LoginService {
  private readonly endpoint = `${environment.apiUrl}/user-login`;

  constructor(private http: HttpClient) {}

  login(request: LoginModel): Observable<User> {
    return this.http.post<User>(this.endpoint, request);
  }
}
