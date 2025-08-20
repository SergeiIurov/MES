import {ProductTypes} from '../enums/ProductTypes';

export interface StationDto {
  id: number;
  chartElementId: number;
  name: string;
  areaId: number;
  areaName?: string;
  productType?: ProductTypes;
}
