import {Component, OnInit} from '@angular/core';
import {DirectoryService} from '../../services/directory-service';
import {StationDto} from '../../Entities/StationDto';
import {TableModule} from 'primeng/table';
import {ButtonDirective, ButtonIcon, ButtonLabel} from 'primeng/button';
import {PointList} from '../point-list/point-list';
import {FormsModule} from '@angular/forms';

interface AreaData {
  name: string;
  code: number;
}

@Component({
  selector: 'app-entry-point',
  imports: [
    TableModule,
    ButtonDirective,
    ButtonIcon,
    ButtonLabel,
    PointList,
    FormsModule
  ],
  templateUrl: './entry-point.html',
  styleUrl: './entry-point.scss'
})
export class EntryPoint implements OnInit {

  areas: AreaData[];
  selectedArea: AreaData;

  stations: StationDto[] = [];
  selectedStations: StationDto[] = [];
  showList = true;

  constructor(private directoryService: DirectoryService) {
  }


  ngOnInit(): void {
    this.directoryService.getAreaList().subscribe(areaList => {
      this.areas = areaList.map(area => ({code: area.id, name: area.name}))
      console.log(this.areas)
    });

    this.directoryService.getStationList().subscribe(data => {
      this.stations = data.sort((a, b) => a.chartElementId - b.chartElementId);
    })
  }
}
