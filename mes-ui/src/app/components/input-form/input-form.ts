import {Component, OnDestroy, OnInit} from '@angular/core';
import {StationDto} from '../../Entities/StationDto';
import {ConstructorService} from '../../services/constructor-service';
import {FormControl, FormGroup, FormsModule, ReactiveFormsModule, Validators} from '@angular/forms';
import {DirectoryService} from '../../services/directory-service';
import {AreaDto} from '../../Entities/AreaDto';
import {MatTab, MatTabGroup} from '@angular/material/tabs';
import {ControlBoardService} from '../../services/control-board-service';

@Component({
  selector: 'app-input-form',
  imports: [
    FormsModule,
    ReactiveFormsModule,
    MatTabGroup,
    MatTab
  ],
  templateUrl: './input-form.html',
  styleUrl: './input-form.scss'
})
export class InputForm implements OnInit, OnDestroy {
  stations: StationDto[];
  areas: AreaDto[];
  form: FormGroup;

  constructor(private service: ConstructorService, private directoryService: DirectoryService, private controlBoardService: ControlBoardService) {
    this.form = new FormGroup({})

  }

  //Сохранение состояния в localStorage в случае перехода на другой компонент
  ngOnDestroy(): void {
    localStorage.setItem('formData', JSON.stringify(this.form.value));
  }

  ngOnInit(): void {
    this.service.getStationList().subscribe(stations => {
      this.stations = stations;
      this.stations.forEach(station => {
        this.form.addControl(station.id.toString(), new FormControl("", [Validators.required]),)
      })

      //При возврате на ранее покинутый компонент, восстанавливаем состояние формы
      if (localStorage.getItem('formData') !== null) {
        this.form.setValue(JSON.parse(localStorage.getItem('formData')));
      }
    });


    this.directoryService.getAreaList().subscribe(areas => {
      this.areas = areas;
    })

  }

  submitData() {
    let formData = this.form.value;
    let info = []
    Object.entries(formData).forEach(([key, value]) => {
      info.push({stationId: key, value})
    })
    this.form.reset();
    localStorage.removeItem('formData');
    this.controlBoardService.saveCurrentState(info).subscribe(d => {
      console.log("Ok", d);
    })
    console.log(info);
  }
}
