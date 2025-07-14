import {Component, OnDestroy, OnInit} from '@angular/core';
import {StationDto} from '../../Entities/StationDto';
import {ConstructorService} from '../../services/constructor-service';
import {FormControl, FormGroup, FormsModule, ReactiveFormsModule, Validators} from '@angular/forms';

@Component({
  selector: 'app-input-form',
  imports: [
    FormsModule,
    ReactiveFormsModule
  ],
  templateUrl: './input-form.html',
  styleUrl: './input-form.scss'
})
export class InputForm implements OnInit, OnDestroy {
  stations: StationDto[]
  form: FormGroup;

  constructor(private service: ConstructorService) {
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

  }

  submitData() {
    let formData = this.form.value;
    //this.form.reset();
    // localStorage.removeItem('formData');
    console.log(formData);
  }
}
