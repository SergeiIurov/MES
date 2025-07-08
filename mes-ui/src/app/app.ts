import {Component} from '@angular/core';
import {RouterLink, RouterLinkActive, RouterOutlet} from '@angular/router';
import {AuthService} from './services/auth-service';

@Component({
  selector: 'app-root',
  imports: [
    RouterOutlet,
    RouterLink,
    RouterLinkActive
  ],
  templateUrl: './app.html',
  styleUrl: './app.scss'
})
export class App {
  constructor(protected auth: AuthService) {
  }

  login() {
    //this.showLogin = true;
  }

  logout() {
    this.auth.logout();
  }
}
