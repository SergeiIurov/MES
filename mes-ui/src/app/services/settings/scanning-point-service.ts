import {Injectable} from '@angular/core';
import {HttpClient} from '@angular/common/http';
import {Observable} from 'rxjs';
import {Environment} from '../../environments/environment';
import {ScanningPointDto} from '../../Entities/ScanningPointDto';

@Injectable({providedIn: 'root'})
export class ScanningPointService {
  constructor(private http: HttpClient) {
  }

  getScanningPoints(): Observable<ScanningPointDto[]> {
    return this.http.get<ScanningPointDto[]>(`${Environment.apiUrl}api/ScanningPoints/scanning-points`)
  }

  addScanningPoint(scanningPoint: ScanningPointDto): Observable<ScanningPointDto> {
    return this.http.post<ScanningPointDto>(`${Environment.apiUrl}api/ScanningPoints/scanning-points`, scanningPoint)
  }

  deleteScanningPoint(id: number): Observable<any> {
    return this.http.delete<any>(`${Environment.apiUrl}api/ScanningPoints/scanning-points/${id}`)
  }

  updateScanningPoint(scanningPoint: ScanningPointDto): Observable<ScanningPointDto> {
    return this.http.put<ScanningPointDto>(`${Environment.apiUrl}api/ScanningPoints/scanning-points`, scanningPoint)
  }
}
