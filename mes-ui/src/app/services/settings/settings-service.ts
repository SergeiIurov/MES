import {Injectable} from '@angular/core';
import {HttpClient} from '@angular/common/http';
import {Environment} from '../../environments/environment';
import {Observable} from 'rxjs';

@Injectable({providedIn: 'root'})
export class SettingsService {
  constructor(private http: HttpClient) {
  }

  getLineCount(): Observable<number> {
    return this.http.get<number>(`${Environment.apiUrl}api/settings/line-count`);
  }
}
