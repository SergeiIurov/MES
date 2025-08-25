import {Injectable} from '@angular/core';
import * as signalR from "@microsoft/signalr";
import {Environment} from "../environments/environment"
import {Observable} from 'rxjs';
import {HttpClient} from '@angular/common/http';
import {ProcessStateAdvDto} from '../Entities/ProcessStateAdvDto';
import {SpecificationDto} from '../Entities/SpecificationDto';
import {AreaDto} from '../Entities/AreaDto';

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

  saveCurrentState(data: ProcessStateAdvDto[]): Observable<any> {
    return this.http.post(`${Environment.apiUrl}api/ControlBoardAdv`, data)
  }

  getCurrentState(): Observable<ProcessStateAdvDto[]> {
    return this.http.get<ProcessStateAdvDto[]>(`${Environment.apiUrl}api/ControlBoardAdv/state`)
  }

  getChart(): Observable<string> {
    return this.http.get(`${Environment.apiUrl}api/BoardConstructor/chart`, {responseType: "text"})
  }

  getSpecificationList(): Observable<SpecificationDto[]> {
    return this.http.get<SpecificationDto[]>(`${Environment.apiUrl}api/ControlBoardAdv/specifications`)
  }

  changeDisabledStatus(area: AreaDto): Observable<Object> {
    return this.http.post(`${Environment.apiUrl}api/ControlBoardAdv/change_disabled_status`, area)
  }

}
