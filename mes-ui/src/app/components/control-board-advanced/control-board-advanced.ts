import {AfterViewInit, Component, ElementRef, OnInit, Renderer2, ViewChild} from '@angular/core';
import {ControlBoardService} from '../../services/control-board-service';
import mx from '../../../mxgraph';                       // <- import values from factory()
import type {mxGraph, mxGraphModel} from 'mxgraph';  // <- import types only, "import type" is a TypeScript 3.8+ syntax

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


  constructor(private service: ControlBoardService) {
    if (mx.mxClient.isBrowserSupported()) {
      console.log('Yes! Yes!');
    }
  }

  graph: mxGraph;

  ngAfterViewInit(): void {
    this.elem.nativeElement.src = this.url;
  }

  ngOnInit(): void {
    this.service.startConnection();
    this.service.getHubConnection().on('сontrolBoardInfoUpdated', () => {
      console.log("Информация по доске контроля обновлена: ");
      this.elem.nativeElement.src = this.url+"?dummyVar="+ (new Date()).getTime();
    });
  }
}
