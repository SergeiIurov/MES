import {Injectable} from '@angular/core';
import {HttpClient} from '@angular/common/http';
import {Router} from '@angular/router';

@Injectable({
  providedIn: 'root'
})
export class AuthService {

  isAuthenticated = false;

  constructor(private http: HttpClient, private router: Router) {
  }

  login(username: string, password: string) {
    this.isAuthenticated = true;
  }

  logout() {
    this.isAuthenticated = false;
    this.router.navigate(['/login']); // Redirect
  }
}
