import {Component, Input} from '@angular/core';
import {StationDto} from '../../Entities/StationDto';

@Component({
  selector: 'app-point-info',
  imports: [],
  templateUrl: './point-info.html',
  styleUrl: './point-info.scss'
})
export class PointInfo {
  @Input() station!: StationDto;
}
