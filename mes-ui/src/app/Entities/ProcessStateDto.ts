import {ProductTypes} from '../enums/ProductTypes';

export interface ProcessStateDto {
  value: string;
  stationName: string;
  productType: ProductTypes;
}
