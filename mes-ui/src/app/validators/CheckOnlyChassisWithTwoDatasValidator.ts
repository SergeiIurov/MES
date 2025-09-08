//Если сотрудник вбивает один номер на станцию с типом продукта — надстройки, при этом по данному номеру есть дата начала
//сборки шасси и дата установки кабины на шасси, в этом случае нет отклонения → строка из списка удаляется
import {BaseValidator} from './BaseValidator';
import {SpecificationDto} from '../Entities/SpecificationDto';
import {StationDto} from '../Entities/StationDto';
import {ProductTypes} from '../enums/ProductTypes';
import {AbstractControl} from '@angular/forms';

export class CheckOnlyChassisWithTwoDatasValidator extends BaseValidator {
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
    } else if (!isDuplicate && productType === ProductTypes.ТипНадстройки && this.isFind(specifications, value, true, true)) {
      return null;
    } else {
      return null;
    }
  }
}
