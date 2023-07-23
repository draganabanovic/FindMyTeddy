import { IAccessToken } from "../models/accessToken";
import jwt_decode from "jwt-decode";

export const accountUtils = {
  getUserId,
  isLogedIn,
  isTokenExpired,
};

function isLogedIn() {
  let token = localStorage.getItem("access_token");

  if (!token) {
    return false;
  }

  return true;
}

function isTokenExpired() {
  let token = localStorage.getItem("access_token");
  if (!token) {
    return true;
  }

  try {
    const decodedToken = jwt_decode<IAccessToken>(token);
    if (decodedToken.exp < Date.now() / 1000) return true;
  } catch (error) {
    return true;
  }

  return false;
}

function getUserId() {
  let token = localStorage.getItem("access_token");
  if (!token) {
    return null;
  }

  const decodedToken = jwt_decode<IAccessToken>(token);
  return decodedToken.userId;
}
