import React, { useEffect, useState } from "react";
import { Navigate } from "react-router-dom";

// @ts-ignore
import { NotificationManager } from "react-notifications";
import { accountUtils } from "../utils/AccountUtils";
interface IProps {
  children: JSX.Element;
}
const ProtectedRoute = ({ children }: IProps) => {
  return accountUtils.isLogedIn() ? children : <Navigate to="/login" />;
};

export default ProtectedRoute;
