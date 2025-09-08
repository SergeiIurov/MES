//Если сотрудник вбивает один номер на станцию с типом продукта — надстройки, при этом по данному номеру отсутствует дата начала сборки шасси,
// в этом случае есть отклонение → строка из списка не должна удаляется, и должно появиться оповещение «отсутствует дата начала сборки шасси»
import {BaseValidator} from './BaseValidator';
import {SpecificationDto} from '../Entities/SpecificationDto';
import {StationDto} from '../Entities/StationDto';
import {ProductTypes} from '../enums/ProductTypes';
import {AbstractControl} from '@angular/forms';

export class CheckOnlyChassisValidator extends BaseValidator {
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
    } else if (!isDuplicate && productType === ProductTypes.ТипНадстройки && this.isFind(specifications, value, false, undefined)) {
      return {message: "Отсутствует дата начала сборки шасси"};
    }
    return null;
  }
}
