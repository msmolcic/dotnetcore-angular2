import { Gender } from "./gender.model";

export class User {
  id: string;
  name: string;
  surname: string;
  birthDate: Date;
  gender: Gender;
}