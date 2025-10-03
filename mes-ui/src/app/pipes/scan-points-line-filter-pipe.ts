import {Pipe, PipeTransform} from '@angular/core';
import {ScanningPointDto} from '../Entities/ScanningPointDto';

@Pipe({
  name: 'scanPointsLineFilter'
})
export class ScanPointsLineFilterPipe implements PipeTransform {

  transform(scanPoints: ScanningPointDto[], lineNumber: string): ScanningPointDto[] {
    // debugger;
    return scanPoints.filter(sp => sp.lineNumber === +lineNumber);
  }

}
