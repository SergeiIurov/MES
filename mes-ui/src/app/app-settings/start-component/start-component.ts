import {Component} from '@angular/core';
import {FormsModule} from '@angular/forms';
import {Select} from 'primeng/select';
import {ScanPointsSetting} from '../scan-points-setting/scan-points-setting';
import {Title} from '@angular/platform-browser';

@Component({
  selector: 'app-start-component',
  imports: [
    FormsModule,
    Select,
    ScanPointsSetting
  ],
  templateUrl: './start-component.html',
  styleUrl: './start-component.scss'
})
export class StartComponent {

  windowsList = [{
    name: "Настройка точек сканирования", code: "ScanPointsSetting"
  }]

  selectedWindow: any = null;

  constructor(private title: Title) {
    title.setTitle("MES");
  }

  onClear() {
    this.title.setTitle("MES");
  }
}
