import {Component, OnInit} from '@angular/core';
import {Title} from '@angular/platform-browser';
import {Select} from 'primeng/select';
import {FormsModule} from '@angular/forms';
import {SettingsService} from '../../../services/settings/settings-service';
import {ScanningPointService} from '../../../services/settings/scanning-point-service';
import {TableModule} from 'primeng/table';
import {ScanningPointDto} from '../../../Entities/ScanningPointDto';
import {ScanPointsLineFilterPipe} from '../../../pipes/scan-points-line-filter-pipe';
import {DatePipe} from '@angular/common';

@Component({
  selector: 'app-scan-points-setting',
  imports: [
    Select,
    FormsModule,
    TableModule,
    ScanPointsLineFilterPipe,
    DatePipe
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

  scanPoints: ScanningPointDto[] = []
  selectedScanPoint: ScanningPointDto;
  date: Date;

  constructor(
    private title: Title,
    private settingService: SettingsService,
    private scanningPointService: ScanningPointService) {
    title.setTitle('MES_Настройка точек сканирования');
  }

  updateScanningPoints() {
    this.date = new Date();
    this.scanningPointService.getScanningPoints().subscribe(scanningPoints => {
      this.scanPoints = scanningPoints.sort((a, b) => +a.orderNum - +b.orderNum);
    })
  }

  ngOnInit(): void {
    this.settingService.getLineCount().subscribe(lineCount => {
      this.lineCount = lineCount;
      this.lines = [];
      for (let val = 1; val <= this.lineCount; val++) {
        this.lines.push({name: val.toString(), code: val.toString()});
      }
      this.updateScanningPoints();
    })
  }
}
