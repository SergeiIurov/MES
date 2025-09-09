//Если сотрудник вбивает два одинаковых номера, один номер на станцию с типом продукта — кабины, второй номер на станцию с типом продукта — надстройки,
//при этом по данному номеру есть дата начала
//сборки шасси и есть дата установки кабины на шасси, в этом случае есть отклонение → должна сработать подсветка дубликата, строка из списка не удаляется
import {BaseValidator} from './BaseValidator';
import {SpecificationDto} from '../Entities/SpecificationDto';
import {StationDto} from '../Entities/StationDto';
import {ProductTypes} from '../enums/ProductTypes';
import {AbstractControl} from '@angular/forms';

export class CheckDoubleNumsWithTwoDatas extends BaseValidator {
  constructor(specifications: SpecificationDto[] = [],
              station: StationDto,
              stations: StationDto[],
              hasDuplicate: () => boolean,
              clearDuplicateSignal: () => void) {
    super(specifications, station, stations, hasDuplicate, clearDuplicateSignal);
  }

  override check(value: string, productType: ProductTypes, specifications: SpecificationDto[], isDuplicate: boolean, stations: StationDto[], hasDuplicate: () => boolean, currentControl: AbstractControl, controls: AbstractControl[]) {
    if (productType === ProductTypes.НеЗадано || !productType) {
      return {message: "Не задан тип продукта"};
    } else if (isDuplicate) {
      if (isDuplicate && productType === ProductTypes.Кабина && this.isFind(specifications, value, undefined, true)) {
        const valCab = stations.flatMap(s => s.processStates).find(s =>
          (s.value.trim().toLowerCase() == value.trim().toLowerCase() &&
            s.productType === ProductTypes.ТипНадстройки)) || this.isFindInControlsWithOtherType(currentControl, controls)
        if (valCab) {
          return {message: "Указана дата установки кабины на шасси"};
        }
      } else if (isDuplicate && productType === ProductTypes.ТипНадстройки && this.isFind(specifications, value, undefined, true)) {
        const addIn = stations.flatMap(s => s.processStates).find(s =>
          (s.value.trim().toLowerCase() == value.trim().toLowerCase() &&
            s.productType === ProductTypes.Кабина)) || this.isFindInControlsWithOtherType(currentControl, controls);
        if (addIn) {
          return {message: "Указана дата установки кабины на шасси"};
        }
      }
      // hasDuplicate();
      return null;
    }
    return null;

  }
}
