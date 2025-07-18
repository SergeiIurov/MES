import {Injectable} from '@angular/core';
import {Subject} from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class NotificationService {
  dataSubject: Subject<any> = new Subject<any>();

  sendMessage(message: string, time: number = 0): void {
    this.dataSubject.next(message);
    if (time) {
      const timeout = setTimeout(() => {
        this.dataSubject.next('');
      }, time)
    }
  }

  clearMessage() {
    this.dataSubject.next('');
  }


}
