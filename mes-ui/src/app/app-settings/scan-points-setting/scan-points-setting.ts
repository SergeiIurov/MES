import {Component, OnInit} from '@angular/core';
import {Title} from '@angular/platform-browser';
import {Select} from 'primeng/select';
import {FormsModule} from '@angular/forms';
import {SettingsService} from '../../services/settings/settings-service';

@Component({
  selector: 'app-scan-points-setting',
  imports: [
    Select,
    FormsModule
  ],
  templateUrl: './scan-points-setting.html',
  styleUrl: './scan-points-setting.scss'
})
export class ScanPointsSetting implements OnInit {

  lineCount: number = 0;
  selectedLine: any = null;
  lines: { name: string, code: string }[] = [{
    name: "1", code: "1"
  }]

  constructor(
    private title: Title,
    private settingService: SettingsService) {
    title.setTitle('MES_Настройка точек сканирования');
  }

  ngOnInit(): void {
    this.settingService.getLineCount().subscribe(lineCount => {
      this.lineCount = lineCount;
      this.lines = [];
      for (let val = 1; val <= this.lineCount; val++) {
        this.lines.push({name: val.toString(), code: val.toString()});
      }
    })
  }
}
