import {Component, EventEmitter, Input, Output} from '@angular/core';
import {Button} from 'primeng/button';
import {Dialog} from 'primeng/dialog';
import {StationDto} from '../../../Entities/StationDto';

@Component({
  selector: 'app-station-edit-dialog',
  imports: [
    Button,
    Dialog
  ],
  templateUrl: './station-edit-dialog.html',
  styleUrl: './station-edit-dialog.scss'
})
export class StationEditDialog {
  @Input() title: string;
  @Input() originalData: StationDto;
  @Input() visible: boolean;
  @Output() onEdit: EventEmitter<StationDto> = new EventEmitter<StationDto>();
  @Output() onCancel: EventEmitter<any> = new EventEmitter();

  edit(stationName: string, stationCode: string) {
    this.originalData.name = stationName;
    this.originalData.chartElementId = +stationCode;
    this.onEdit.emit(this.originalData);
    this.originalData = null;
  }

  cancel() {
    this.onCancel.emit();
  }
}
