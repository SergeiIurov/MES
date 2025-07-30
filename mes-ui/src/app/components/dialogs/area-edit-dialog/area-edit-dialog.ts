import {Component, EventEmitter, Input, Output, ViewChild} from '@angular/core';
import {Button} from "primeng/button";
import {Dialog} from "primeng/dialog";
import {AreaDto} from '../../../Entities/AreaDto';

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

  edit(value: string) {
    this.originalData.name = value;
    this.onEdit.emit(this.originalData);
    this.originalData = null;
  }

  cancel() {
    this.onCancel.emit();
  }
}
