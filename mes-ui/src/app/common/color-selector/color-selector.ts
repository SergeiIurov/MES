import {Component, EventEmitter, Input, Output} from '@angular/core';
import {NgStyle} from '@angular/common';

@Component({
  selector: 'app-color-selector',
  imports: [
    NgStyle
  ],
  templateUrl: './color-selector.html',
  styleUrl: './color-selector.scss'
})
export class ColorSelector {
  @Input() selectedColor: string = 'red';
  @Input() id: string = '';
  @Output() onSelectedColorChange: EventEmitter<string> = new EventEmitter();

  options = ['red', 'green', 'blue', 'purple', 'pink', 'yellow', 'black', 'orange', 'brown', 'darkblue', 'darkred', 'fuchsia', 'greenyellow'];


  changeColor(color: string) {
    this.selectedColor = color;
    this.onSelectedColorChange.emit(this.selectedColor);
  }
}
