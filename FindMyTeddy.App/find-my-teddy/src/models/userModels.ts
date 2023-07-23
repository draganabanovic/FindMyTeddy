export interface ICreateUserModel {
  firstName: string;
  lastName: string;
  email: string;
  city: string;
  street: string;
  phone: string;
  password: string;
}

export interface IUserModel {
  id: string;
  role: string;
  firstName: string;
  lastName: string;
  email: string;
  city: string;
  street: string;
  profilePicture: string;
  phone: string;
  password: string;
}
