import {AfterViewInit, Component, ElementRef, OnInit, ViewChild} from '@angular/core';

@Component({
  selector: 'app-constructor',
  imports: [],
  templateUrl: './constructor.html',
  styleUrl: './constructor.scss'
})
export class Constructor implements AfterViewInit {
  url = '/constructor.html'
  @ViewChild('refElem') elem: ElementRef;


  ngAfterViewInit(): void {
    this.elem.nativeElement.src = this.url;
  }
}
