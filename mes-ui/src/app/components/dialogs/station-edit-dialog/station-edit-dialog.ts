import {Component, EventEmitter, Input, Output} from '@angular/core';
import {Button} from 'primeng/button';
import {Dialog} from 'primeng/dialog';
import {StationDto} from '../../../Entities/StationDto';
import {DirectoryService} from '../../../services/directory-service';
import {MessageService} from 'primeng/api';

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

  constructor(
    private directoryService: DirectoryService,
    private messageService: MessageService) {
  }

  edit(stationName: string, stationCode: number) {

    this.directoryService.isFree(this.originalData.id, stationCode).subscribe(isFree => {
      if (!isFree) {
        console.log('Step1')
        this.messageService.add({
          severity: 'warn',
          summary: 'Warning',
          detail: 'Идентификатор станции уже используется!',
          life: 5000
        });
      } else {
        this.directoryService.isInRange(this.originalData.id, this.originalData.areaId, this.originalData.chartElementId).subscribe(isInRange => {
          if (!isInRange) {
            console.log('Step2')
            this.messageService.add({
              severity: 'warn',
              summary: 'Warning',
              detail: 'Значение идентификатора не входит в заданный диапазон!',
              life: 5000
            });
          } else {
            this.originalData.name = stationName;
            this.originalData.chartElementId = +stationCode;
            this.onEdit.emit(this.originalData);
            this.originalData = null;
          }
        })
      }
    })
  }

  cancel() {
    this.onCancel.emit();
  }
}
