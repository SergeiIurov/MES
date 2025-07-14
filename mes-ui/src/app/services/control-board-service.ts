import {Injectable} from '@angular/core';
import * as signalR from "@microsoft/signalr";
import {Environment} from "../environments/environment"
import {Observable} from 'rxjs';
import {StationDto} from '../Entities/StationDto';
import {HttpClient} from '@angular/common/http';

@Injectable({
  providedIn: 'root',
})
export class ControlBoardService {
  private hubConnection!: signalR.HubConnection;

  constructor(private http: HttpClient) {
  }

  public startConnection() {
    this.hubConnection = new signalR.HubConnectionBuilder()
      .configureLogging(signalR.LogLevel.Information)
      .withUrl(Environment.apiUrl + 'api/mes-hub', {
        withCredentials:
          false
      })
      .build();
    console.log("Starting connection...");
    this.hubConnection
      .start()
      .then(() => console.log("Connection started."))
      .catch((err: any) => console.log(err));

  }

  public getHubConnection() {
    return this.hubConnection;
  }


  getChart(): Observable<string> {
    return this.http.get(`${Environment.apiUrl}api/BoardConstructor/chart`, {responseType: "text"})
  }

}
