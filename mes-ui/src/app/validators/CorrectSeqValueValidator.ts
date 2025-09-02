import {AbstractControl, ValidationErrors, ValidatorFn} from '@angular/forms';
import {SpecificationDto} from '../Entities/SpecificationDto';
import {StationDto} from '../Entities/StationDto';
import {ProductTypes} from '../enums/ProductTypes';

//Валидатор для проверки наличия значения на форме в загруженном файле спецификации
//Кроме этого выполняется проверка на соответствие бизнес-правилам при работе с формой.
export function CorrectSeqValueValidator(specifications: SpecificationDto[] = [], station: StationDto, stations: StationDto[], hasDuplicate: () => boolean, clearDuplicateSignal: () => void): ValidatorFn {
  return (control: AbstractControl): ValidationErrors | null => {
    clearDuplicateSignal();
    //Проверка на наличие дублирования сиквенса
    const isDuplicate = () => {
      return vals.some(c => c !== control && c.value.toString().toLowerCase() === control.value.toString().toLowerCase());
    }

    const vals: AbstractControl[] = []
    // for (let val in control.parent?.value) {
    //   if (control.parent?.value[val]) {
    //     vals.push(control.parent?.value[val]);
    //   }
    // }

    for (const c in control.parent?.controls) {
      vals.push(control.parent?.controls[c])
    }


    const value = control.value;
    const result = specifications.some(spec => spec.sequenceNumber.trim().toLowerCase() == value.trim().toLowerCase());
    if (!value) {
      return null;
    } else if (!result && value.trim()) {
      return {keyNotFound: true};
    } else {
      clearDuplicateSignal();
      return check1(value, station.productType, specifications, isDuplicate(), stations, hasDuplicate);
    }

  }
}

//Если сотрудник вбивает один номер на станцию с типом продукта — кабина,
//при этом по данному номеру отсутствует дата начала сборки шасси и дата установки кабины на шасси, в этом случае нет отклонения → строка из списка удаляется
function check1(value: string, productType: ProductTypes, specifications: SpecificationDto[], isDuplicate: boolean, stations: StationDto[], hasDuplicate: () => boolean): {
  [message: string]: string
} | null {
  if (productType === ProductTypes.Кабина && isFind(specifications, value, false, false)) {
    return null;
  } else {

    return check2(value, productType, specifications, isDuplicate, stations, hasDuplicate);
  }
}

//Если сотрудник вбивает один номер на станцию с типом продукта — надстройки, при этом по данному номеру отсутствует дата начала сборки шасси,
// в этом случае есть отклонение → строка из списка не должна удаляется, и должно появиться оповещение «отсутствует дата начала сборки шасси»
function check2(value: string, productType: ProductTypes, specifications: SpecificationDto[], isDuplicate: boolean, stations: StationDto[], hasDuplicate: () => boolean): {
  [message: string]: string
} | null {
  if (!isDuplicate && productType === ProductTypes.ТипНадстройки && isFind(specifications, value, false, undefined)) {
    return {message: "Отсутствует дата начала сборки шасси"};
  }
  return check3(value, productType, specifications, isDuplicate, stations, hasDuplicate);
}

//Если сотрудник вбивает один номер на станцию с типом продукта — надстройки, при этом по данному номеру есть дата начала
//сборки шасси и дата установки кабины на шасси, в этом случае нет отклонения → строка из списка удаляется
function check3(value: string, productType: ProductTypes, specifications: SpecificationDto[], isDuplicate: boolean, stations: StationDto[], hasDuplicate: () => boolean): {
  [message: string]: string
} | null {
  if (!isDuplicate && productType === ProductTypes.ТипНадстройки && isFind(specifications, value, true, true)) {
    return null;
  } else {
    return check4(value, productType, specifications, isDuplicate, stations, hasDuplicate);
  }
}

//1) Если сотрудник вбивает два одинаковых номера на станции с типом продукта — кабина,
//в этом случае есть отклонение → должна сработать подсветка дубликата, строка из списка не удаляется
//2) Если сотрудник вбивает два одинаковых номера на станции с типом продукта — надстройки, в этом случае есть
// отклонение → должна сработать подсветка дубликата, строка из списка не удаляется

function check4(value: string, productType: ProductTypes, specifications: SpecificationDto[], isDuplicate: boolean, stations: StationDto[], hasDuplicate: () => boolean): {
  [message: string]: string
} | null {
  if (isDuplicate && productType === ProductTypes.Кабина) {
    const valCab = stations.flatMap(s => s.processStates).find(s =>
      s.value.trim().toLowerCase() == value.trim().toLowerCase() &&
      s.productType === ProductTypes.Кабина &&
      isFind(specifications, value, undefined, undefined))
    if (valCab) {
      hasDuplicate();
      return {message: "Найдены продублированные значения"};
    }
  } else if (isDuplicate && productType === ProductTypes.ТипНадстройки && isFind(specifications, value, undefined, undefined)) {
    const addIn = stations.flatMap(s => s.processStates).find(s =>
      s.value.trim().toLowerCase() == value.trim().toLowerCase() &&
      s.productType === ProductTypes.ТипНадстройки);
    if (addIn) {
      hasDuplicate()
      return {message: "Найдены продублированные значения"};
    }
  }
  return check5(value, productType, specifications, isDuplicate, stations, hasDuplicate);
}

//Если сотрудник вбивает два одинаковых номера, один номер на станцию с типом продукта — кабины, второй номер на станцию с типом продукта — надстройки, при этом по
// данному номеру есть дата начала сборки шасси, но нет даты установки кабины на шасси, в этом случае нет отклонения → строка из списка удаляется
function check5(value: string, productType: ProductTypes, specifications: SpecificationDto[], isDuplicate: boolean, stations: StationDto[], hasDuplicate: () => boolean): {
  [message: string]: string
} | null {
  if (isDuplicate) {
    if (isDuplicate && productType === ProductTypes.Кабина) {
      const valCab = stations.flatMap(s => s.processStates).find(s =>
        s.value.trim().toLowerCase() == value.trim().toLowerCase() &&
        s.productType === ProductTypes.ТипНадстройки &&
        isFind(specifications, value, true, false))
      if (valCab) {
        return null;
      }
    } else if (isDuplicate && productType === ProductTypes.ТипНадстройки && isFind(specifications, value, true, false)) {
      const addIn = stations.flatMap(s => s.processStates).find(s =>
        s.value.trim().toLowerCase() == value.trim().toLowerCase() &&
        s.productType === ProductTypes.Кабина);
      if (addIn) {
        return null;
      }
    }
    return check6(value, productType, specifications, isDuplicate, stations, hasDuplicate);
  }
  return null;
}

//Если сотрудник вбивает два одинаковых номера, один номер на станцию с типом продукта — кабины, второй номер на станцию с типом продукта — надстройки,
//при этом по данному номеру есть дата начала
//сборки шасси и есть дата установки кабины на шасси, в этом случае есть отклонение → должна сработать подсветка дубликата, строка из списка не удаляется
function check6(value: string, productType: ProductTypes, specifications: SpecificationDto[], isDuplicate: boolean, stations: StationDto[], hasDuplicate: () => boolean): {
  [message: string]: string
} | null {
  if (isDuplicate) {
    if (isDuplicate && productType === ProductTypes.Кабина) {
      const valCab = stations.flatMap(s => s.processStates).find(s =>
        s.value.trim().toLowerCase() == value.trim().toLowerCase() &&
        s.productType === ProductTypes.ТипНадстройки &&
        isFind(specifications, value, true, true))
      if (!valCab) {
        return null;
      }
    } else if (isDuplicate && productType === ProductTypes.ТипНадстройки && isFind(specifications, value, true, true)) {
      const addIn = stations.flatMap(s => s.processStates).find(s =>
        s.value.trim().toLowerCase() == value.trim().toLowerCase() &&
        s.productType === ProductTypes.Кабина);
      if (!addIn) {
        return null;
      }
    }
    hasDuplicate();
    return {message: "Найдены продублированные значения"};
  }
  return null;

}

//Предикат для проверки таблицы спецификаций
function isFind(specifications: SpecificationDto[], sequenceNumber: string, chassisAssemblyStartDate: boolean, dateInstallationCabin: boolean): boolean {
  return specifications.some(spec =>
    spec.sequenceNumber.trim().toLowerCase() == sequenceNumber.trim().toString().toLowerCase() &&
    (chassisAssemblyStartDate !== undefined ? (chassisAssemblyStartDate ? spec.chassisAssemblyStartDate !== null : spec.chassisAssemblyStartDate == null) : true) &&
    (dateInstallationCabin !== undefined ? (dateInstallationCabin ? spec.dateInstallationCabin !== null : spec.dateInstallationCabin == null) : true));
}
