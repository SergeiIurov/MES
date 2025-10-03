import {Routes} from '@angular/router';
import {ControlBoard} from './components/control-board/control-board';
import {authGuard} from './services/guards/auth-guard';
import {Login} from './components/login/login';
import {BoardConstructor} from './components/constructor/board-constructor';
import {ControlBoardAdvanced} from './components/control-board-advanced/control-board-advanced';
import {InputForm} from './components/input-form/input-form';
import {authorizeGuard} from './services/guards/authorize-guard';
import {Admin} from './components/admin/admin';
import {authEntryPointGuard} from './services/guards/auth-entry-point-guard';
import {authStartSettingsGuard} from './services/guards/authStartSettingsGuard';
import {EntryPoint} from './components/control-points/entry-point/entry-point';
import {StartComponent} from './components/app-settings/start-component/start-component';

export const routes: Routes = [
  {
    path: '', component: ControlBoardAdvanced, pathMatch: 'full'/*, canActivate: [authGuard]*/
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
    path: 'scan', component: EntryPoint, canActivate: [authEntryPointGuard]
  },
  {
    path: 'start-settings', component: StartComponent, canActivate: [authStartSettingsGuard]
  },
  {
    path: '**', redirectTo: '/'
  }
];
