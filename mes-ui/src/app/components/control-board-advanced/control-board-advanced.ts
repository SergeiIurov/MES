import {AfterViewInit, Component, ElementRef, OnInit, ViewChild} from '@angular/core';
import {ControlBoardService} from '../../services/control-board-service';
import {AuthService} from '../../services/auth-service';
import {Roles} from '../../enums/roles';

@Component({
  selector: 'app-control-board-advanced',
  imports: [],
  templateUrl: './control-board-advanced.html',
  styleUrl: './control-board-advanced.scss'
})
export class ControlBoardAdvanced implements OnInit, AfterViewInit {
  @ViewChild('graphContainer') graphContainer!: ElementRef;
  url = '/control-board-adv.html'
  @ViewChild('refElem') elem: ElementRef;


  constructor(private service: ControlBoardService,
              protected auth: AuthService) {

  }


  ngAfterViewInit(): void {
    this.elem.nativeElement.src = this.url;
  }

  ngOnInit(): void {
    this.service.startConnection();
    this.service.getHubConnection().on('сontrolBoardInfoUpdated', () => {

      this.elem.nativeElement.src = this.url + "?dummyVar=" + (new Date()).getTime();
    });
  }

  protected readonly Roles = Roles;
}
