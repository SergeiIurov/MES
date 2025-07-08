import {Injectable} from '@angular/core';
import {HttpClient} from '@angular/common/http';
import {Observable} from 'rxjs';
import {StationDto} from '../Entities/StationDto';
import {Environment} from '../environments/environment';

@Injectable({
  providedIn: 'root'
})
export class ConstructorService {

  constructor(private http: HttpClient) {

  }

  getStationList(): Observable<StationDto[]> {
    return this.http.get<StationDto[]>(`${Environment.apiUrl}api/BoardConstructor`)
  }
}
