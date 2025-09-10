import {AfterViewChecked, Component, ElementRef, OnDestroy, OnInit, ViewChild} from '@angular/core';
import {StationDto} from '../../Entities/StationDto';
import {FormControl, FormGroup, FormsModule, ReactiveFormsModule, Validators} from '@angular/forms';
import {DirectoryService} from '../../services/directory-service';
import {AreaDto} from '../../Entities/AreaDto';
import {ControlBoardService} from '../../services/control-board-service';
import {NotificationService} from '../../services/notification-service';
import {ConfirmationService, MenuItem, MessageService} from 'primeng/api';
import {Button, ButtonDirective, ButtonIcon, ButtonLabel} from 'primeng/button';
import {Dialog} from 'primeng/dialog';
import {Toolbar} from 'primeng/toolbar';
import {AreaEditDialog} from '../dialogs/area-edit-dialog/area-edit-dialog';
import {ConfirmDialog} from 'primeng/confirmdialog';
import {StationEditDialog} from '../dialogs/station-edit-dialog/station-edit-dialog';
import {AutoFocus} from 'primeng/autofocus';
import {TableModule} from 'primeng/table';
import {FileUpload, UploadEvent} from 'primeng/fileupload';
import {Environment} from '../../environments/environment';
import {Roles} from '../../enums/roles';
import {ProductTypes} from '../../enums/ProductTypes';
import {SpecificationDto} from '../../Entities/SpecificationDto';
import {ColorSelector} from '../../common/color-selector/color-selector';
import {CheckOnlyCabinaValidator} from '../../validators/CheckOnlyCabinaValidator';
import {CheckOnlyChassisValidator} from '../../validators/CheckOnlyChassisValidator';
import {CheckOnlyChassisWithTwoDatasValidator} from '../../validators/CheckOnlyChassisWithTwoDatasValidator';
import {CheckDoubleCabinOrChassisValidator} from '../../validators/CheckDoubleCabinOrChassisValidator';
import {CheckDoubleNumsOnlyStartData} from '../../validators/CheckDoubleNumsOnlyStartData';
import {CheckDoubleNumsWithTwoDatas} from '../../validators/CheckDoubleNumsWithTwoDatas';


@Component({
  selector: 'app-input-form',
  imports: [
    FormsModule,
    ReactiveFormsModule,
    ButtonDirective,
    ButtonIcon,
    ButtonLabel,
    Button,
    Dialog,
    Toolbar,
    AreaEditDialog,
    ConfirmDialog,
    StationEditDialog,
    AutoFocus, TableModule,
    FileUpload,
    ColorSelector
  ],
  templateUrl: './input-form.html',
  styleUrl: './input-form.scss'
})
export class InputForm implements OnInit, OnDestroy, AfterViewChecked {
  stations: StationDto[];
  areas: AreaDto[];
  selectedColor: string;
  form: FormGroup;
  @ViewChild('frmBlock') frmBlock: ElementRef;
  elements: any[];
  visibleNewAreaDialog: boolean = false;
  visibleEditAreaDialog: boolean = false;
  visibleNewStationDialog: boolean = false;
  visibleEditStationDialog: boolean = false;
  area: AreaDto;
  station: StationDto;
  items: MenuItem[] | undefined;
  fileUploadUrl = `${Environment.apiUrl}api/ControlBoardAdv/upload`;
  productTypes = Object.keys(Roles).filter(k => +k + 1);
  specifications: SpecificationDto[];
  fullSpecifications: SpecificationDto[];


  constructor(
    private confirmationService: ConfirmationService,
    private directoryService: DirectoryService,
    private controlBoardService: ControlBoardService,
    private notification: NotificationService,
    private messageService: MessageService) {
    this.form = new FormGroup({})
  }

  onInput = e => {
    //!!this.hasDuplicate();

    // if (e.target.value.length >= 3 && !this.hasDuplicate()) {
    //   e.target.value = e.target.value.substring(0, 3);
    //   if (mas[idx + 1]) {
    //     mas[idx + 1].focus();
    //     mas[idx + 1].select();
    //   }
    // }
  }

  ngAfterViewChecked(): void {
    this.elements = this.frmBlock.nativeElement.querySelectorAll("form input")
    this.elements.forEach((element, idx, mas) => {
      element.addEventListener('input',
        this.onInput
      )
    })
  }

  //Сохранение состояния в localStorage в случае перехода на другой компонент
  ngOnDestroy(): void {
    localStorage.setItem('formData', JSON.stringify(this.form.value));
  }

  createForm() {
    this.form = new FormGroup({});
    this.directoryService.getStationList().subscribe(stations => {

      this.directoryService.getAreaList().subscribe(areas => {
        this.areas = areas;

        //Сортировка участков в зависимости от диапазона
        this.areas.sort((a, b) => +a.range.match(/^(\d+).+(\d+)$/)[1] - +b.range.match(/^(\d+).+(\d+)$/)[1]);
        this.areas.forEach(area => {
          area.stations.sort((a, b) => a.chartElementId - b.chartElementId);
        })


      })

      this.stations = stations;

      this.controlBoardService.getSpecificationList().subscribe(specifications => {
        this.fullSpecifications = specifications;
        this.stations.forEach(station => {
          let formControl = new FormControl("", [
            // Validators.pattern('\\d{3}')
            Validators.minLength(3),
            Validators.maxLength(7),
            //Регулярное выражение ограничивающая ввод значения сиквенса длиной от 3 до 7 символов, без концевых пробелов
            Validators.pattern('^[^ ].{1,5}[^ ]$'),
            new CheckOnlyCabinaValidator(this.fullSpecifications, station, stations, this.hasDuplicate.bind(this), this.clearDuplicateSignal.bind(this)).validate,
            new CheckOnlyChassisValidator(this.fullSpecifications, station, stations, this.hasDuplicate.bind(this), this.clearDuplicateSignal.bind(this)).validate,
            new CheckOnlyChassisWithTwoDatasValidator(this.fullSpecifications, station, stations, this.hasDuplicate.bind(this), this.clearDuplicateSignal.bind(this)).validate,
            new CheckDoubleCabinOrChassisValidator(this.fullSpecifications, station, stations, this.hasDuplicate.bind(this), this.clearDuplicateSignal.bind(this)).validate,
            new CheckDoubleNumsOnlyStartData(this.fullSpecifications, station, stations, this.hasDuplicate.bind(this), this.clearDuplicateSignal.bind(this)).validate,
            new CheckDoubleNumsWithTwoDatas(this.fullSpecifications, station, stations, this.hasDuplicate.bind(this), this.clearDuplicateSignal.bind(this)).validate
          ]);
          formControl['station'] = station;
          this.form.addControl((station.id).toString(), formControl)
        })
        //Восстанавливаем значения формы с текущего состояния
        this.controlBoardService.getCurrentState().subscribe(currentState => {
          const object = Object.fromEntries(currentState.map(s => [s.stationId, s.value]));
          this.form.setValue(object);
        })
      })

      //При возврате на ранее покинутый компонент, восстанавливаем состояние формы
      // if (localStorage.getItem('formData') !== null) {
      //   this.form.setValue(JSON.parse(localStorage.getItem('formData')));
      // }

      this.getSpecifications();
    });
  }

  getSpecifications = () => {
    setTimeout(() => {
      this.controlBoardService.getSpecificationList().subscribe(specificationList => {
        //Получаем в массив значения всех сиквенсов на форме
        const seq = []
        Object.entries(this.form.value).forEach(([key, value]) => {
          if (value.toString().trim()) {
            seq.push(value.toString().trim());
          }
        })
        this.fullSpecifications = specificationList;

        //Оставляем только записи, которых нет в полях формы
        this.specifications = specificationList.filter(s => seq.indexOf(s.sequenceNumber.trim()) === -1);
      })
    }, 1000)

  }

  ngOnInit(): void {

    this.items = [
      {
        label: 'Add',
        icon: 'pi pi-pencil',
        command: () => {
          this.messageService.add({severity: 'info', summary: 'Add', detail: 'Data Added'});
        },
      },
      {
        label: 'Update',
        icon: 'pi pi-refresh',
        command: () => {
          this.messageService.add({severity: 'success', summary: 'Update', detail: 'Data Updated'});
        },
      },
      {
        label: 'Delete',
        icon: 'pi pi-trash',
        command: () => {
          this.messageService.add({severity: 'error', summary: 'Delete', detail: 'Data Deleted'});
        },
      }
    ];

    this.createForm();
  }

  submitData() {

    let formData = this.form.value;
    let info = []
    Object.entries(formData).forEach(([key, value]) => {
      info.push({stationId: key, value})
    })
    if (true/*!this.hasDuplicate()*/) {
      this.notification.clearMessage()
      localStorage.removeItem('formData');
      info = [...info.map(i => ({stationId: +i.stationId, value: "" + i.value}))]
      if (!this.isWarning(info)) {
        this.controlBoardService.saveCurrentState(info).subscribe(d => {
          this.createForm();
          this.messageService.add({severity: 'success', summary: 'Success', detail: 'Данные зафиксированы!'});
        })
      } else {
        this.confirmationService.confirm({
          header: 'Confirmation',
          message: "Все данные имеют пустые значения.\nВы действительно хотите продолжить?`",
          icon: 'pi pi-exclamation-circle',
          acceptButtonProps: {
            label: 'Save',
            icon: 'pi pi-check',
            size: 'small'
          },
          rejectButtonProps: {
            label: 'Cancel',
            icon: 'pi pi-times',
            variant: 'outlined',
            size: 'small'
          }
          ,
          accept: () => {
            this.controlBoardService.saveCurrentState(info).subscribe(d => {
              this.createForm();
              this.messageService.add({severity: 'success', summary: 'Success', detail: 'Данные зафиксированы!'});
            })
          },
          reject: () => {
            this.messageService.add({severity: 'warn', summary: 'Warning', detail: 'Загрузка данных отменена.'});
          }
        });
      }
    } else {
      this.messageService.add({severity: 'warn', summary: 'Warning', detail: 'Найдены продублированные значения!'});
    }

  }

  isWarning(info) {
    return info.filter(s => s.value !== '' && s.value !== 'null').length === 0;
  }

  clearDuplicateSignal() {
    this.elements.forEach(e => e.classList.remove('signalDuplicate'));
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

    //dups.filter(d => d.value !== '000').forEach(dup => dup.classList.add('signalDuplicate'));
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

  openNewAreaDialog(): void {
    this.visibleNewAreaDialog = true;
  }

  openNewStationDialog(a: AreaDto) {
    this.area = a;
    this.visibleNewStationDialog = true;
  }

  openEditAreaDialog(area: AreaDto): void {
    this.area = area
    this.visibleEditAreaDialog = true;
  }

  openEditStationDialog(station: StationDto) {
    this.station = station;
    this.visibleEditStationDialog = true;
  }

  checkRangeValue(range: string): boolean {
    const pattern: RegExp = /^\d+-\d+$/
    return pattern.test(range.trim());
  }

  saveNewArea(areaName: string, areaRange: string) {
    if (!areaName.trim()) {
      if (!areaName.trim()) {
        this.messageService.add({
          severity: 'warn',
          summary: 'Warning',
          detail: 'Не задано наименование участка',
          life: 5000
        });
        return;
      }
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
    this.directoryService.addArea({
      name: areaName,
      id: 0,
      range: areaRange,
      disabledColor: 'red',
      stations: [],
      isDisabled: false
    }).subscribe(data => {
      this.createForm();
    })

    this.visibleNewAreaDialog = false;
  }

  saveNewStation(stationName: string, stationCode: number, productType: ProductTypes) {
    this.directoryService.isFree(0, stationCode).subscribe(isFree => {
      if (!isFree) {
        this.messageService.add({
          severity: 'warn',
          summary: 'Warning',
          detail: 'Идентификатор станции уже используется!',
          life: 5000
        });
      } else {
        this.directoryService.isInRange(0, this.area.id, stationCode).subscribe(isInRange => {
          if (!isInRange) {
            this.messageService.add({
              severity: 'warn',
              summary: 'Warning',
              detail: 'Значение идентификатора не входит в заданный диапазон!',
              life: 5000
            });
          } else {
            this.directoryService.addStation({
              id: 0,
              name: stationName,
              areaId: this.area.id,
              chartElementId: +stationCode,
              productType: +productType,
            }).subscribe(data => {
              this.createForm();
              this.visibleNewStationDialog = false;
            })
          }
        })
      }
    })
  }

  editArea(area: AreaDto) {
    this.directoryService.updateArea(area).subscribe(data => {
      this.createForm();
      this.visibleEditAreaDialog = false;
    })

  }

  editStation(station: StationDto) {
    this.directoryService.updateStation(station).subscribe(data => {
      this.createForm();
      this.visibleEditStationDialog = false;
    })
  }

  cancelAreaEdit() {
    this.visibleEditAreaDialog = false;
  }

  cancelStationEdit() {
    this.visibleEditStationDialog = false;
  }

  deleteArea(area: AreaDto) {

    if (area.stations.length > 0) {
      this.messageService.add({
        severity: 'warn',
        summary: 'Warning',
        detail: 'Невозможно удалить участок!\nСначала удалите из него все станции!'
      });
      return;
    }
    this.confirmationService.confirm({
      header: 'Confirmation',
      message: `Вы действительно хотите удалить участок "${area.name}"?`,
      icon: 'pi pi-exclamation-circle',
      acceptButtonProps: {
        label: 'Save',
        icon: 'pi pi-check',
        size: 'small'
      },
      rejectButtonProps: {
        label: 'Cancel',
        icon: 'pi pi-times',
        variant: 'outlined',
        size: 'small'
      }
      ,
      accept: () => {
        this.directoryService.deleteArea(area.id).subscribe(data => {
          this.createForm();
        })
        this.messageService.add({
          severity: 'info',
          summary: 'Удаление',
          detail: `Участок "${area.name}" удален.`,
          life: 3000
        });
      },
      reject: () => {
        this.messageService.add({severity: 'error', summary: 'Отмена', detail: 'Удаление отменено.', life: 3000});
      }
    });
  }

  deleteStation(station: StationDto) {
    this.confirmationService.confirm({
      header: 'Confirmation',
      message: `Вы действительно хотите удалить станцию "${station.name}"?`,
      icon: 'pi pi-exclamation-circle',
      acceptButtonProps: {
        label: 'Save',
        icon: 'pi pi-check',
        size: 'small'
      },
      rejectButtonProps: {
        label: 'Cancel',
        icon: 'pi pi-times',
        variant: 'outlined',
        size: 'small'
      }
      ,
      accept: () => {
        this.directoryService.deleteStation(station.id).subscribe(data => {
          this.createForm();
          this.messageService.add({
            severity: 'info',
            summary: 'Удаление',
            detail: `Станция "${station.name}" удалена.`,
            life: 3000
          });
        }, error => {
          this.messageService.add({severity: 'error', summary: 'Ошибка', detail: 'Ошибка удаления.', life: 3000});
        })
      },
      reject: () => {
        this.messageService.add({severity: 'error', summary: 'Отмена', detail: 'Удаление отменено.', life: 3000});
      }
    });
  }

  onUpload(event: UploadEvent) {
    this.messageService.add({severity: 'info', summary: 'Success', detail: 'Файл был успешно загружен.'});
  }

  protected readonly Roles = Roles;
  protected readonly ProductTypes = ProductTypes;

  changeStatus(a: AreaDto) {
    const area = this.areas.find(area => area.id == a.id);
    if (area !== null) {
      area.isDisabled = !area.isDisabled;
    }
    this.controlBoardService.changeDisabledStatus(a).subscribe((data) => {
      if (data['result']) {
        this.messageService.add({severity: 'info', summary: 'Success', detail: `Участок "${a.name}" остановлен.`});
      } else {
        this.messageService.add({severity: 'info', summary: 'Success', detail: `Участок "${a.name}" запущен.`});
      }
    });
  }

  onSelectColor(areaId: number, color: string) {
    this.directoryService.updateDisabledColor(areaId, color).subscribe(data => {
      console.log('Обновление цвета выполнено')
    })
  }

}
