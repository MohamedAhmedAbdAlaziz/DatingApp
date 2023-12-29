import { Photo } from './Photo';

export interface User {
  id: number;
  username: string;
  token: string;
  photoUrl: string;
  knowAs: string;
  gender: string;
}
