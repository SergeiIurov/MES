import {Injectable} from '@angular/core';
import {HttpClient} from '@angular/common/http';
import {Router} from '@angular/router';
import {Environment} from '../environments/environment';
import {Observable} from 'rxjs';
import {jwtDecode} from 'jwt-decode';

@Injectable({
  providedIn: 'root'
})
export class AuthService {

  _token: string = '';
  _name: string = '';
  _role: string = '';

  constructor(private http: HttpClient, private router: Router) {
  }

  get role(): string {
    return this._role;
  }

  set role(role: string) {
    this._role = role;
  }

  get name(): string {
    return this._name;
  }

  set name(name: string) {
    this._name = name;
  }

  get isAuthenticated() {
    return !!this.token || !!localStorage.getItem('access_token');
  }

  set token(token: string) {
    this._token = token;
  }

  get token(): string {
    return this._token || localStorage.getItem('access_token');
  }

  login(username: string, password: string): Observable<any> {
    return this.http.post(`${Environment.apiUrl}api/Auth/`, {username, password}, {responseType: 'text'});
  }

  logout() {
    this.token = '';
    localStorage.removeItem('access_token');
    this.router.navigate(['/login']);
  }
}
