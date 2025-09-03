import {Component, HostListener} from '@angular/core';
import {AuthService} from '../../services/auth-service';
import {Router, RouterLink, RouterLinkActive} from '@angular/router';
import {Information} from '../information/information';
import {Roles} from '../../enums/roles';
import {ButtonDirective, ButtonLabel} from 'primeng/button';

@Component({
  selector: 'app-header',
  imports: [
    RouterLink,
    RouterLinkActive,
    Information,
    ButtonDirective,
    ButtonLabel
  ],
  templateUrl: './header.html',
  styleUrl: './header.scss'
})
export class Header {
  constructor(protected auth: AuthService, private router: Router) {
  }

  logout() {
    this.auth.logout();
  }

  protected readonly Roles = Roles;

  login() {
    this.router.navigate(['/login']);
  }
}
