import {Component, OnInit} from '@angular/core';
import {HttpClient} from '@angular/common/http';
import {ControlBoardService} from '../../services/control-board-service';
import {Environment} from '../../environments/environment';

@Component({
  selector: 'app-control-board',
  imports: [],
  templateUrl: './control-board.html',
  styleUrl: './control-board.scss'
})
export class ControlBoard implements OnInit {


  constructor(private http: HttpClient, private service: ControlBoardService) {
  }

  url = `${Environment.apiUrl}api/ControlBoard/image`;
  src = ""

  updateUrl() {
    this.http.get(this.url, {responseType: "text"}).subscribe(data => {
      this.src = data;
    })
  }

  ngOnInit(): void {
    this.service.startConnection();
    this.service.getHubConnection().on('notifyAll', () => {
      console.log("Получено серверное сообщение: ");
      this.updateUrl();
    });
    this.updateUrl();
  }

}
