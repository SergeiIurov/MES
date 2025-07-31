import {HttpClient} from '@angular/common/http';
import {Observable} from 'rxjs';
import {Environment} from '../environments/environment';
import {Injectable} from '@angular/core';
import {AreaDto} from '../Entities/AreaDto';
import {StationDto} from '../Entities/StationDto';
import {ProductTypeDto} from '../Entities/ProductTypeDto';

@Injectable({providedIn: 'root'})
export class DirectoryService {
  constructor(private http: HttpClient) {

  }

  getAreaList(): Observable<AreaDto[]> {
    return this.http.get<AreaDto[]>(`${Environment.apiUrl}api/Directory/areas`)
  }

  addArea(area: AreaDto): Observable<AreaDto> {
    return this.http.post<AreaDto>(`${Environment.apiUrl}api/Directory/areas`, area)
  }

  deleteArea(id: number): Observable<any> {
    return this.http.delete<any>(`${Environment.apiUrl}api/Directory/areas/${id}`)
  }

  updateArea(area: AreaDto): Observable<AreaDto> {
    return this.http.put<AreaDto>(`${Environment.apiUrl}api/Directory/areas`, area)
  }

  getStationList(): Observable<StationDto[]> {
    return this.http.get<StationDto[]>(`${Environment.apiUrl}api/Directory/stations`)
  }

  addStation(station: StationDto): Observable<StationDto> {
    return this.http.post<StationDto>(`${Environment.apiUrl}api/Directory/stations`, station)
  }

  deleteStation(id: number): Observable<any> {
    return this.http.delete<any>(`${Environment.apiUrl}api/Directory/stations/${id}`)
  }

  updateStation(station: StationDto): Observable<StationDto> {
    return this.http.put<StationDto>(`${Environment.apiUrl}api/Directory/stations`, station)
  }

  isFree(id: number, chartElementId: number): Observable<boolean> {
    return this.http.get<boolean>(`${Environment.apiUrl}api/Directory/stations/isfree/${id}/${chartElementId}`);
  }

  isInRange(id: number, areaId: number, chartElementId: number): Observable<boolean> {
    return this.http.get<boolean>(`${Environment.apiUrl}api/Directory/stations/isinrange/${id}/${areaId}/${chartElementId}`);
  }

  getProductTypeList(): Observable<ProductTypeDto[]> {
    return this.http.get<ProductTypeDto[]>(`${Environment.apiUrl}api/Directory/product-types`)
  }
}
