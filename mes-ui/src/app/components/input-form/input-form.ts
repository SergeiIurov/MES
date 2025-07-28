import {AfterViewChecked, Component, ElementRef, OnDestroy, OnInit, ViewChild} from '@angular/core';
import {StationDto} from '../../Entities/StationDto';
import {FormControl, FormGroup, FormsModule, ReactiveFormsModule, Validators} from '@angular/forms';
import {DirectoryService} from '../../services/directory-service';
import {AreaDto} from '../../Entities/AreaDto';
import {ControlBoardService} from '../../services/control-board-service';
import {NotificationService} from '../../services/notification-service';
import {MessageService} from 'primeng/api';
import {ButtonDirective, ButtonIcon, ButtonLabel} from 'primeng/button';

@Component({
  selector: 'app-input-form',
  imports: [
    FormsModule,
    ReactiveFormsModule,
    ButtonDirective,
    ButtonIcon,
    ButtonLabel
  ],
  templateUrl: './input-form.html',
  styleUrl: './input-form.scss'
})
export class InputForm implements OnInit, OnDestroy, AfterViewChecked {
  stations: StationDto[];
  areas: AreaDto[];
  form: FormGroup;
  @ViewChild('frmBlock') frmBlock: ElementRef;
  elements: any[];

  constructor(
    private directoryService: DirectoryService,
    private controlBoardService: ControlBoardService,
    private notification: NotificationService,
    private messageService: MessageService,) {
    this.form = new FormGroup({})
  }

  ngAfterViewChecked(): void {
    this.elements = this.frmBlock.nativeElement.querySelectorAll("form input")
    this.elements.forEach((element, idx, mas) => {
      element.addEventListener('input',
        e => {
          if (e.target.value.length >= 3 && !this.hasDuplicate()) {
            e.target.value = e.target.value.substring(0, 3);
            if (mas[idx + 1]) {
              mas[idx + 1].focus();
              mas[idx + 1].select();
            }
          }
        }
      )
    })
  }

  //Сохранение состояния в localStorage в случае перехода на другой компонент
  ngOnDestroy(): void {
    localStorage.setItem('formData', JSON.stringify(this.form.value));
  }

  ngOnInit(): void {
    this.directoryService.getStationList().subscribe(stations => {

      this.directoryService.getAreaList().subscribe(areas => {
        this.areas = areas;
      })

      this.stations = stations;
      this.stations.forEach(station => {
        this.form.addControl((station.id).toString(), new FormControl("", [
          Validators.pattern('\\d{3}')
        ]))
      })

      //При возврате на ранее покинутый компонент, восстанавливаем состояние формы
      if (localStorage.getItem('formData') !== null) {
        this.form.setValue(JSON.parse(localStorage.getItem('formData')));
      }
    });


  }

  submitData() {
    let formData = this.form.value;
    let info = []
    Object.entries(formData).forEach(([key, value]) => {
      info.push({stationId: key, value})
    })
    if (!this.hasDuplicate()) {
      this.notification.clearMessage()
      this.form.reset();
      localStorage.removeItem('formData');
      info = [...info.map(i => ({stationId: +i.stationId, value: "" + i.value}))]
      this.controlBoardService.saveCurrentState(info).subscribe(d => {
        this.messageService.add({severity: 'success', summary: 'Success', detail: 'Данные зафиксированы!'});
      })
    } else {
      this.messageService.add({severity: 'warn', summary: 'Warning', detail: 'Найдены продублированные значения!'});
    }

  }

  hasDuplicate(): boolean {
    let d: any = this.findDuplicates(this.elements);
    let dups: any[] = []


    Object.values(d).forEach((value) => {
      let mas: any[] = (value as any[]);
      this.elements.forEach(e => e.classList.remove('signalDuplicate'));


      if (mas.length > 1) {
        dups = [...dups, ...mas];
      }
    })

    dups.filter(d => d.value !== '000').forEach(dup => dup.classList.add('signalDuplicate'));
    return dups.filter(d => d.value !== '000').length > 0;
  }

  //Формируем группы элементов для поиска дубликатов
  findDuplicates(data: any[]) {
    return Array.from(data).reduce((acc, item) => {
      const key: number = item.value !== "" ? item.value.padStart(3, '0') : item.value;
      if (!key) {
        return acc;
      }
      if (!acc[key]) {
        acc[key] = [];
      }
      acc[key].push(item);
      return acc;
    }, {})
  }
}
