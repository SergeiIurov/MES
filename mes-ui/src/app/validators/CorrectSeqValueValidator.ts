import {AbstractControl, ValidationErrors, ValidatorFn} from '@angular/forms';
import {SpecificationDto} from '../Entities/SpecificationDto';

export function CorrectSeqValueValidator(specifications: SpecificationDto[] = []): ValidatorFn {
  return (control: AbstractControl): ValidationErrors | null => {
    const value = control.value;
    const result = specifications.some(spec => spec.sequenceNumber.trim().toLowerCase() == value.trim().toLowerCase());
    return !result && value.trim() ? {keyNotFound: true} : null;
  }
}
