import {Component} from '@angular/core';
import {ControlBoard} from './components/control-board/control-board';

@Component({
  selector: 'app-root',
  imports: [
    ControlBoard
  ],
  templateUrl: './app.html',
  styleUrl: './app.scss'
})
export class App {

}
