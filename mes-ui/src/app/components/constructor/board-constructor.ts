import {AfterViewInit, Component, ElementRef, OnInit, ViewChild} from '@angular/core';
import {ConstructorService} from '../../services/constructor-service';
import {StationDto} from '../../Entities/StationDto';

@Component({
  selector: 'app-constructor',
  imports: [],
  templateUrl: './board-constructor.html',
  styleUrl: './board-constructor.scss'
})
export class BoardConstructor implements AfterViewInit, OnInit {
  stations: StationDto[]

  constructor(private service: ConstructorService) {

  }

  ngOnInit(): void {
    this.service.getStationList().subscribe(stations => {
      this.stations = stations;
      this.stations.sort((a, b) => a.chartElementId - b.chartElementId);
    });

  }

  url = '/constructor.html'
  @ViewChild('refElem') elem: ElementRef;


  ngAfterViewInit(): void {
    this.elem.nativeElement.src = this.url;
  }
}
