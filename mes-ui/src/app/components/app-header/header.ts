import {Component} from '@angular/core';
import {AuthService} from '../../services/auth-service';
import {RouterLink, RouterLinkActive} from '@angular/router';

@Component({
  selector: 'app-header',
  imports: [
    RouterLink,
    RouterLinkActive
  ],
  templateUrl: './header.html',
  styleUrl: './header.scss'
})
export class Header {
  constructor(protected auth: AuthService) {
  }

  logout() {
    this.auth.logout();
  }
}
