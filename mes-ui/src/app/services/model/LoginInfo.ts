import {Roles} from '../../enums/roles';

export interface LoginInfo {
  name: string;
  password?: string;
  role: Roles;
  isActive: boolean;
}
