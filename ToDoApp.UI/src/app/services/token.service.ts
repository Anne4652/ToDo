import { Injectable } from '@angular/core';
import { BehaviorSubject } from 'rxjs';
import { constants } from '../constants/constants'

@Injectable({
  providedIn: 'root'
})
export class TokenService {
  isAuthentification: BehaviorSubject<boolean> = new BehaviorSubject<boolean>(false);

  constructor() {
    const token = this.getToken();
    if (token){
      this.updateToken(true);
    }
  }

  updateToken(status: boolean) {
    this.isAuthentification.next(status);
  }

  setToken(token: string){
    this.updateToken(true);
    localStorage.setItem(constants.CURRENT_TOKEN, token);
  }

  getToken(): string | null {
    return localStorage.getItem(constants.CURRENT_TOKEN);
  }

  removeToken() {
    this.updateToken(false);
    localStorage.removeItem(constants.CURRENT_TOKEN);
  }
}
