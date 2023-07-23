export interface IAccessToken {
  name: string;
  givenname: string;
  surname: string;
  email: string;
  userId: string;
  exp: number;
  iss: string;
  aud: string;
  "http://schemas.microsoft.com/ws/2008/06/identity/claims/role": "User";
}
