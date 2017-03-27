import { Gender } from "./gender.model";

export class UserRegistration {
  name: string;
  surname: string;
  birthDate: Date;
  gender: Gender;

  username: string;
  email: string;
  password: string;
  confirmPassword: string;
}