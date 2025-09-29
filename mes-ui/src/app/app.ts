import {Component, HostListener, OnInit} from '@angular/core';
import {ActivatedRoute, RouterOutlet} from '@angular/router';
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
export class App implements OnInit {
  returnUrl: string;

  constructor(private auth: AuthService,
              private activatedRoute: ActivatedRoute) {
  }

  ngOnInit(): void {
    this.activatedRoute.queryParams.subscribe(params => {
      this.returnUrl = params['returnUrl'];
    })
  }

  //Принудительный выход из приложения при закрытии вкладки или браузера
  @HostListener('window:beforeunload', ['$event'])
  onBeforeUnload($event: BeforeUnloadEvent) {
    if (!this.returnUrl) {
      $event.stopImmediatePropagation();
      $event.preventDefault();
      this.auth.logout();
    }
  }
}
