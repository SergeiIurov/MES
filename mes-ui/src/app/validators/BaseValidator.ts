import {SpecificationDto} from '../Entities/SpecificationDto';
import {StationDto} from '../Entities/StationDto';
import {AbstractControl, FormControl, ValidationErrors, Validator} from '@angular/forms';
import {ProductTypes} from '../enums/ProductTypes';

export abstract class BaseValidator implements Validator {
  protected constructor(private specifications: SpecificationDto[] = [],
                        private station: StationDto,
                        private stations: StationDto[],
                        private hasDuplicate: () => boolean,
                        private clearDuplicateSignal: () => void) {

  }

  control: AbstractControl;
  vals: AbstractControl[] = []

  //Проверка на наличие дублирования сиквенса
  protected isDuplicate = () => {
    return this.vals.some(c => c !== this.control && c.value.toString().toLowerCase() === this.control.value.toString().toLowerCase());
  }

  validate = (control: AbstractControl): ValidationErrors | null => {
   //this.clearDuplicateSignal();

    this.control = control;
    this.vals = [];
    for (const c in control.parent?.controls) {
      this.vals.push(control.parent?.controls[c])
    }

    const value = control.value;
    const result = this.specifications.some(spec => spec.sequenceNumber?.trim().toLowerCase() == value?.trim().toLowerCase());
    if (!value) {
      return null;
    } else if (!result && value.trim()) {
      return {keyNotFound: true};
    } else {
      return this.check(value, this.station.productType, this.specifications, this.isDuplicate(), this.stations, this.hasDuplicate, control, this.vals);
    }
  }


  protected abstract check(value: string, productType: ProductTypes, specifications: SpecificationDto[], isDuplicate: boolean, stations: StationDto[], hasDuplicate: () => boolean, currentControl: AbstractControl, controls: AbstractControl[]);

//Предикат для проверки таблицы спецификаций
  protected isFind(specifications: SpecificationDto[], sequenceNumber: string, chassisAssemblyStartDate: boolean, dateInstallationCabin: boolean): boolean {
    return specifications.some(spec =>
      spec.sequenceNumber.trim().toLowerCase() == sequenceNumber.trim().toString().toLowerCase() &&
      (chassisAssemblyStartDate !== undefined ? (chassisAssemblyStartDate ? spec.chassisAssemblyStartDate !== null : spec.chassisAssemblyStartDate == null) : true) &&
      (dateInstallationCabin !== undefined ? (dateInstallationCabin ? spec.dateInstallationCabin !== null : spec.dateInstallationCabin == null) : true));
  }

  protected isFindInControlsWithSameType(currentControl: AbstractControl, controls: AbstractControl[]): boolean {
    const res = controls.some(ctrl => currentControl != ctrl &&
      ctrl.value.trim().toLowerCase() == currentControl.value.trim().toLowerCase() &&
      ctrl['station'].productType === currentControl['station'].productType && ctrl['station'].productType !== ProductTypes.НеЗадано);
    return res;
  }

  protected isFindInControlsWithOtherType(currentControl: AbstractControl, controls: AbstractControl[]): boolean {
    const res = controls.some(ctrl => currentControl != ctrl &&
      ctrl.value.trim().toLowerCase() == currentControl.value.trim().toLowerCase() &&
      ctrl['station'].productType !== currentControl['station'].productType && ctrl['station'].productType !== ProductTypes.НеЗадано);
    return res;
  }

  protected setValid(currentControl: AbstractControl, controls: AbstractControl[]): void {
    const res = controls.filter(ctrl => currentControl != ctrl &&
      ctrl.value.trim().toLowerCase() == currentControl.value.trim().toLowerCase() &&
      ctrl['station'].productType !== currentControl['station'].productType && ctrl['station'].productType !== ProductTypes.НеЗадано);
    if (res) {
      res[0].setErrors(null);
    }
  }

}

