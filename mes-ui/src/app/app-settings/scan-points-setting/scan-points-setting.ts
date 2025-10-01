import {Component} from '@angular/core';
import {Title} from '@angular/platform-browser';
import {Select} from 'primeng/select';
import {FormsModule} from '@angular/forms';

@Component({
  selector: 'app-scan-points-setting',
  imports: [
    Select,
    FormsModule
  ],
  templateUrl: './scan-points-setting.html',
  styleUrl: './scan-points-setting.scss'
})
export class ScanPointsSetting {
  constructor(private title: Title) {
    title.setTitle('MES_Настройка точек сканирования');
  }

  selectedLine : any = null;
  lines = [{
    name: "1", code: "1"
  }]
}
