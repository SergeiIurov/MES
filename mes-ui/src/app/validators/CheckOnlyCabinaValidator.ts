import {BaseValidator} from './BaseValidator';
import {AbstractControl} from '@angular/forms';
import {ProductTypes} from '../enums/ProductTypes';
import {SpecificationDto} from '../Entities/SpecificationDto';
import {StationDto} from '../Entities/StationDto';

//Если сотрудник вбивает один номер на станцию с типом продукта — кабина,
//при этом по данному номеру отсутствует дата начала сборки шасси и дата установки кабины на шасси, в этом случае нет отклонения → строка из списка удаляется
export class CheckOnlyCabinaValidator extends BaseValidator {
  constructor(specifications: SpecificationDto[] = [],
              station: StationDto,
              stations: StationDto[],
              hasDuplicate: () => boolean,
              clearDuplicateSignal: () => void) {
    super(specifications, station, stations, hasDuplicate, clearDuplicateSignal);
  }

  override check(value: string, productType: ProductTypes, specifications: SpecificationDto[], isDuplicate: boolean, stations: StationDto[], hasDuplicate: () => boolean, currentControl: AbstractControl, controls: AbstractControl[]) {
    if(productType === ProductTypes.НеЗадано || !productType) {
      return {message: "Не задан тип продукта"};
    }
    else if (productType === ProductTypes.Кабина && this.isFind(specifications, value, false, false) && !isDuplicate) {
      return null;
    } else {

      return null;
    }
  }
}
