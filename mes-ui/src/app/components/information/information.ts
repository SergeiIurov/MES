import {Component} from '@angular/core';
import {NotificationService} from '../../services/notification-service';

@Component({
  selector: 'app-information',
  imports: [],
  templateUrl: './information.html',
  styleUrl: './information.scss'
})
export class Information {
  constructor(private notification: NotificationService) {
  }
  info: string = "";

  ngOnInit(): void {
    this.notification.dataSubject.subscribe((message: string) => {
      this.info = message;
    })
  }
}
