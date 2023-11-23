import { User } from './users';

export interface UserParams {
  gender: string;
  minAge: number;
  maxAge: number;
  pageNumber: number;
  pageSize: number;
  country: string;
}
