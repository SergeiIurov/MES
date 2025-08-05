import {Injectable} from '@angular/core';
import {HttpClient} from '@angular/common/http';
import {Observable} from 'rxjs';
import {Environment} from '../environments/environment';
import {LoginInfo} from './model/LoginInfo';

@Injectable({
  providedIn: 'root'
})
export class AdminService {
  constructor(private http: HttpClient) {

  }

  getUserList(): Observable<LoginInfo[]> {
    return this.http.get<LoginInfo[]>(`${Environment.apiUrl}api/admin/users`)
  }

  addUser(user: LoginInfo): Observable<LoginInfo> {
    return this.http.post<LoginInfo>(`${Environment.apiUrl}api/admin/createuser`, user)
  }

  deleteUser(name: string): Observable<string> {
    return this.http.delete<string>(`${Environment.apiUrl}api/admin/deleteuser/${name}`)
  }

  changeRole(loginInfo: LoginInfo, newRole: number): Observable<string> {
    return this.http.put<string>(`${Environment.apiUrl}api/admin/changerole/${newRole}`, loginInfo)
  }
}
