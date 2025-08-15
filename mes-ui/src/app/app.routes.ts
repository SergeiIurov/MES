import {Routes} from '@angular/router';
import {ControlBoard} from './components/control-board/control-board';
import {authGuard} from './services/auth-guard';
import {Login} from './components/login/login';
import {BoardConstructor} from './components/constructor/board-constructor';
import {ControlBoardAdvanced} from './components/control-board-advanced/control-board-advanced';
import {InputForm} from './components/input-form/input-form';
import {authorizeGuard} from './services/authorize-guard';
import {Admin} from './components/admin/admin';

export const routes: Routes = [
  {
    path: '', component: ControlBoardAdvanced, pathMatch: 'full', canActivate: [authGuard]
  },
  {
    path: 'login', component: Login
  },
  {
    path: 'board-constructor', component: BoardConstructor, canActivate: [authGuard, authorizeGuard]
  },
  {
    path: 'control-board', component: ControlBoard, canActivate: [authGuard]
  },
  {
    path: 'input-form', component: InputForm, canActivate: [authGuard, authorizeGuard]
  },
  {
    path: 'administration', component: Admin, canActivate: [authGuard, authorizeGuard]
  },
  {
    path: '**', redirectTo: '/'
  }
];
