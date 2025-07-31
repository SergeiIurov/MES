import {StationDto} from './StationDto';

export interface AreaDto {
  id: number;
  name: string;
  range: string;
  stations: StationDto[];
}
