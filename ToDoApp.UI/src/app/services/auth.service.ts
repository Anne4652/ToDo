import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { TokenService } from './token.service';
import { Auth } from '../models/auth';
import { LoginResponse } from '../models/loginResponse';
import { apiEndpoint } from '../constants/api';
import { map } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class AuthService {

  constructor(
    private http: HttpClient,
    private tokenService: TokenService
  ) { }

  onLogin(data: Auth){
    return this.http
      .post<LoginResponse>(`${apiEndpoint.AuthEndpoint.login}`, data)
      .pipe(
        map((response) => {
          if(response) {
            this.tokenService.setToken(response.token);
          }
          return response;
        })
      )
  }

  onSingup(data: Auth){
    return this.http
      .post<LoginResponse>(`${apiEndpoint.AuthEndpoint.register}`, data)
      .pipe(
        map((response) => {
          if(response) {
            this.tokenService.setToken(response.token);
          }
          return response;
        })
      )
  }
}
