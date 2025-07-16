import {StationDto} from './StationDto';

export interface AreaDto {
  id: number;
  name: string;
  stations: StationDto[];
}
