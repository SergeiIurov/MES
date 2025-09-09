//Если сотрудник вбивает два одинаковых номера, один номер на станцию с типом продукта — кабины, второй номер на станцию с типом продукта — надстройки, при этом по
// данному номеру есть дата начала сборки шасси, но нет даты установки кабины на шасси, в этом случае нет отклонения → строка из списка удаляется
import {BaseValidator} from './BaseValidator';
import {SpecificationDto} from '../Entities/SpecificationDto';
import {StationDto} from '../Entities/StationDto';
import {ProductTypes} from '../enums/ProductTypes';
import {AbstractControl} from '@angular/forms';

export class CheckDoubleNumsOnlyStartData extends BaseValidator {
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
      if (isDuplicate && productType === ProductTypes.Кабина) {
        const valCab = stations.flatMap(s => s.processStates).find(s =>
          s.value.trim().toLowerCase() == value.trim().toLowerCase() &&
          s.productType === ProductTypes.ТипНадстройки &&
          this.isFind(specifications, value, true, false))
        if (valCab) {
          return null;
        } else {
          return null;
        }
      } else if (isDuplicate && productType === ProductTypes.ТипНадстройки && this.isFind(specifications, value, true, false)) {
        const addIn = stations.flatMap(s => s.processStates).find(s =>
          s.value.trim().toLowerCase() == value.trim().toLowerCase() &&
          s.productType === ProductTypes.Кабина &&
          this.isFind(specifications, value, true, false));
        if (addIn) {
          return null;
        }
      }
      // return null;
    } else if (!isDuplicate && productType === ProductTypes.Кабина && this.isFind(specifications, value, true, false)) {
      return {message: "Также укажите номер на станции с типом 'Надстройка'"};
    }
    else if (!isDuplicate && productType === ProductTypes.ТипНадстройки && this.isFind(specifications, value, true, false)) {
      return {message: "Также укажите номер на станции с типом 'Кабина'"};
    }
    return null;
  }
}
