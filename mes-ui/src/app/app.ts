import {Component, HostListener} from '@angular/core';
import {RouterOutlet} from '@angular/router';
import {Header} from './components/app-header/header';
import {Toast} from 'primeng/toast';
import {AuthService} from './services/auth-service';

@Component({
  selector: 'app-root',
  imports: [
    RouterOutlet,
    Header,
    Toast
  ],
  templateUrl: './app.html',
  styleUrl: './app.scss'
})
export class App {
  constructor(private auth: AuthService) {
  }

  @HostListener('window:beforeunload', ['$event'])
  onBeforeUnload($event: BeforeUnloadEvent) {
    $event.preventDefault();
    this.auth.logout();
  }
}
