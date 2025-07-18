import {Component} from '@angular/core';
import {RouterLink, RouterLinkActive, RouterOutlet} from '@angular/router';
import {AuthService} from './services/auth-service';
import {Header} from './components/app-header/header';

@Component({
  selector: 'app-root',
  imports: [
    RouterOutlet,
    RouterLink,
    RouterLinkActive,
    Header
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
