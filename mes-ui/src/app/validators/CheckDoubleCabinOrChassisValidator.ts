//1) Если сотрудник вбивает два одинаковых номера на станции с типом продукта — кабина,
//в этом случае есть отклонение → должна сработать подсветка дубликата, строка из списка не удаляется
//2) Если сотрудник вбивает два одинаковых номера на станции с типом продукта — надстройки, в этом случае есть
// отклонение → должна сработать подсветка дубликата, строка из списка не удаляется
import {BaseValidator} from './BaseValidator';
import {SpecificationDto} from '../Entities/SpecificationDto';
import {StationDto} from '../Entities/StationDto';
import {ProductTypes} from '../enums/ProductTypes';
import {AbstractControl} from '@angular/forms';

export class CheckDoubleCabinOrChassisValidator extends BaseValidator {
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
    } else if (isDuplicate && productType === ProductTypes.Кабина && this.isFind(specifications, value, undefined, undefined)) {
      const valCab = stations.flatMap(s => s.processStates).find(s =>
          (s.value.trim().toLowerCase() == value.trim().toLowerCase() &&
            s.productType === ProductTypes.Кабина)) ||
        this.isFindInControlsWithSameType(currentControl, controls);
      if (valCab) {
        hasDuplicate();
        return {message: "Найдены продублированные значения"};
      }
    } else if (isDuplicate && productType === ProductTypes.ТипНадстройки && this.isFind(specifications, value, undefined, undefined)) {
      const addIn = stations.flatMap(s => s.processStates).find(s =>
          (s.value.trim().toLowerCase() == value.trim().toLowerCase() &&
            s.productType === ProductTypes.ТипНадстройки)) ||
        this.isFindInControlsWithSameType(currentControl, controls);
      if (addIn) {
        hasDuplicate()
        return {message: "Найдены продублированные значения"};
      }
    }
    return null;
  }
}
