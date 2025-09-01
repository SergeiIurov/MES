import {ProductTypes} from '../enums/ProductTypes';
import {ProcessStateDto} from './ProcessStateDto';

export interface StationDto {
  id: number;
  chartElementId: number;
  name: string;
  areaId: number;
  areaName?: string;
  productType?: ProductTypes;
  processStates?: ProcessStateDto[];
}
