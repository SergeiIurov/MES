import {HttpClient} from '@angular/common/http';
import {Observable} from 'rxjs';
import {Environment} from '../environments/environment';
import {Injectable} from '@angular/core';
import {AreaDto} from '../Entities/AreaDto';

@Injectable({providedIn: 'root'})
export class DirectoryService {
  constructor(private http: HttpClient) {

  }

  getAreaList(): Observable<AreaDto[]> {
    return this.http.get<AreaDto[]>(`${Environment.apiUrl}api/Directory`)
  }
}
