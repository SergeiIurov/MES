import {Routes} from '@angular/router';
import {ControlBoard} from './components/control-board/control-board';
import {authGuard} from './services/auth-guard';
import {Login} from './components/login/login';

export const routes: Routes = [
  {
    path: '', component: ControlBoard, pathMatch: 'full', canActivate: [authGuard]
  },
  {
    path: 'login', component: Login
  }
];
