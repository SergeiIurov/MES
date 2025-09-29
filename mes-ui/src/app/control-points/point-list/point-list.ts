import {AfterViewInit, Component, ElementRef, Input, ViewChild} from '@angular/core';
import {StationDto} from '../../Entities/StationDto';
import {PointInfo} from '../point-info/point-info';

@Component({
  selector: 'app-point-list',
  imports: [
    PointInfo
  ],
  templateUrl: './point-list.html',
  styleUrl: './point-list.scss'
})
export class PointList implements AfterViewInit {
  ngAfterViewInit(): void {
    this.inputRef.nativeElement.focus();
  }

  @Input() stations: StationDto[] = [];
  @ViewChild('inputRef') inputRef: ElementRef;
  currantStationName: string;

}
