import {Component} from '@angular/core';
import {RouterOutlet} from '@angular/router';
import {AuthService} from './services/auth-service';
import {Header} from './components/app-header/header';
import {Information} from './components/information/information';

@Component({
  selector: 'app-root',
  imports: [
    RouterOutlet,
    Header,
    Information
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
