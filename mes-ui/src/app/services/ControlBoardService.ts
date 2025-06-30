import {Injectable} from '@angular/core';
import * as signalR from "@microsoft/signalr";
import {Environment} from "../environments/environment"

@Injectable({
  providedIn: 'root',
})
export class ControlBoardService {
  private hubConnection!: signalR.HubConnection;

  constructor() {
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

}
