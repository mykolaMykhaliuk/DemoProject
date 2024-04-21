import { HttpClient } from '@angular/common/http';
import { Injectable, inject } from '@angular/core';
import { BehaviorSubject, Observable, tap } from 'rxjs';
import * as configurl from '../../assets/config.json';
import { Router } from '@angular/router';
import { jwtDecode } from 'jwt-decode';

@Injectable({
  providedIn: 'root',
})
export class AuthService {
  private authPath: string = configurl.apiServer.url;
  private readonly JWT_TOKEN = 'JWT_TOKEN';
  private loggedUser?: string;
  private isAuthenticatedSubject = new BehaviorSubject<boolean>(false);
  private http = inject(HttpClient);
  private routerService = inject(Router);

  constructor() {}

  login(user: { email: string; password: string }): Observable<any> {
    return this.http.post(this.authPath + '/Login', user).pipe(
      tap((response: any) => {
        this.doLoginUser(user.email, response.token);
      })
    );
  }
  doLoginUser(userName: string, token: any) {
    this.loggedUser = userName;
    this.storeJwtToken(token);
    this.isAuthenticatedSubject.next(true);
  }
  storeJwtToken(jwt: any) {
    localStorage.setItem(this.JWT_TOKEN, jwt);
  }

  getHeaders() {
    let token = localStorage.getItem(this.JWT_TOKEN);
    return {
      Authorization: `Bearer ${token}`,
    };
  }
  logout() {
    localStorage.removeItem(this.JWT_TOKEN);
    this.isAuthenticatedSubject.next(false);
    this.routerService.navigate(['/login']);
  }
  isLoggedIn() {
    return !!localStorage.getItem(this.JWT_TOKEN);
  }

  isTokenExpired() {
    const token = localStorage.getItem(this.JWT_TOKEN);
    if (!token) return true;
    const decoded = jwtDecode(token);
    if (!decoded.exp) return true;
    const expirationDate = decoded.exp * 1000;
    const now = new Date().getTime();

    return expirationDate < now;
  }

  refreshToken() {
    return this.http.post<any>(this.authPath + '/refreshToken', {}).pipe(
      tap((tokens: any) => {
        this.storeJwtToken(tokens.token);
      })
    );
  }
}
