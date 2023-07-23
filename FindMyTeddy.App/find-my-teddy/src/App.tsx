import React, { useEffect, useState } from "react";
import "./App.css";
import { Button, Container } from "react-bootstrap";
import "bootstrap/dist/css/bootstrap.min.css";

import "react-notifications/lib/notifications.css";
// @ts-ignore
import { NotificationContainer } from "react-notifications";

import {
  BrowserRouter,
  Outlet,
  Route,
  Routes,
  useNavigate,
} from "react-router-dom";
import { petService } from "./Services/petService";
import { IPetModel } from "./models/petModels";
import LostPets from "./pages/LostPets";
import NavBar from "./components/NavBar";

import Home from "./pages/Home";
import Pet from "./pages/Pet";
import Owner from "./pages/Owner";
import MyPets from "./pages/MyPets";
import AddPet from "./pages/AddPet";
import Login from "./pages/Login";
import Registration from "./pages/Registration";
import Footer from "./components/Footer";
import ProtectedRoute from "./components/ProtectedRoute";
import { accountUtils } from "./utils/AccountUtils";

interface IUser {
  name: string;
  surname: string;
}

function App() {
  useEffect(() => {
    if (accountUtils.isTokenExpired()) {
      localStorage.clear();
    }
  }, []);

  return (
    <div className="text-center position-relative bg-white min-vh-100 ">
      <BrowserRouter>
        <Routes>
          <Route
            path="/"
            element={
              <>
                <NavBar />
                <Container fluid className="content bg-white ">
                  <Outlet />
                </Container>
                <Footer />
              </>
            }
          >
            <Route index element={<Home />} />
            <Route path="pet/:petId" element={<Pet />} />
            <Route path="lost-pets" element={<LostPets />} />
            <Route
              path="owner"
              element={<ProtectedRoute children={<Owner />} />}
            />
            <Route
              path="my-pets"
              element={<ProtectedRoute children={<MyPets />} />}
            />

            <Route
              path="add-pet"
              element={<ProtectedRoute children={<AddPet />} />}
            />
            <Route
              path="edit-pet"
              element={<ProtectedRoute children={<AddPet />} />}
            />
            <Route path="login" element={<Login />} />
            <Route
              path="owner/edit"
              element={<ProtectedRoute children={<Registration />} />}
            />
            <Route path="registration" element={<Registration />} />
            <Route path="*" element={<Home />} />
          </Route>
        </Routes>
      </BrowserRouter>
      <NotificationContainer />
    </div>
  );
}

export default App;
