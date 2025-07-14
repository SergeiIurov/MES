import {Routes} from '@angular/router';
import {ControlBoard} from './components/control-board/control-board';
import {authGuard} from './services/auth-guard';
import {Login} from './components/login/login';
import {BoardConstructor} from './components/constructor/board-constructor';
import {ControlBoardAdvanced} from './components/control-board-advanced/control-board-advanced';
import {InputForm} from './components/input-form/input-form';

export const routes: Routes = [
  {
    path: '', component: ControlBoard, pathMatch: 'full', canActivate: [authGuard]
  },
  {
    path: 'login', component: Login
  },
  {
    path: 'board-constructor', component: BoardConstructor
  },
  {
    path: 'control-board', component: ControlBoardAdvanced
  },
  {
    path: 'input-form', component: InputForm
  },
  {
    path: '**', redirectTo: '/'
  }
];
