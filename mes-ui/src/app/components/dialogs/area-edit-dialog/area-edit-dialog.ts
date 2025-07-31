import {Component, EventEmitter, Input, Output, ViewChild} from '@angular/core';
import {Button} from "primeng/button";
import {Dialog} from "primeng/dialog";
import {AreaDto} from '../../../Entities/AreaDto';
import {MessageService} from 'primeng/api';

@Component({
  selector: 'app-area-edit-dialog',
  imports: [
    Button,
    Dialog
  ],
  templateUrl: './area-edit-dialog.html',
  styleUrl: './area-edit-dialog.scss'
})
export class AreaEditDialog {
  @Input() title: string;
  @Input() originalData: AreaDto;
  @Input() visible: boolean;
  @Output() onEdit: EventEmitter<AreaDto> = new EventEmitter<AreaDto>();
  @Output() onCancel: EventEmitter<any> = new EventEmitter();

  constructor(public messageService: MessageService) {
  }

  checkRangeValue(range: string): boolean {
    const pattern: RegExp = /^\d+-\d+$/
    return pattern.test(range.trim());
  }

  edit(areaName: string, areaRange: string) {
    if (!areaName.trim()) {
      this.messageService.add({
        severity: 'warn',
        summary: 'Warning',
        detail: 'Не задано наименование участка',
        life: 5000
      });
      return;
    }
    if (!this.checkRangeValue(areaRange)) {
      this.messageService.add({
        severity: 'warn',
        summary: 'Warning',
        detail: 'Некорректное значение диапазона!\nДопустимо два числа, разделённые тире (1-100, 305-500)',
        life: 5000
      });
      return;
    }
    this.originalData.name = areaName;
    this.originalData.range = areaRange;
    this.onEdit.emit(this.originalData);
    this.originalData = null;
  }

  cancel() {
    this.onCancel.emit();
  }
}
