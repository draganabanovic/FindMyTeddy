import {
  faCirclePlus,
  faHouse,
  faMagnifyingGlass,
  faPaw,
  faPlus,
  faUser,
} from "@fortawesome/free-solid-svg-icons";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import React, { useEffect, useState } from "react";
import Container from "react-bootstrap/Container";
import Nav from "react-bootstrap/Nav";
import Navbar from "react-bootstrap/Navbar";
import NavDropdown from "react-bootstrap/NavDropdown";
import { Link } from "react-router-dom";

import { NavLink, useNavigate } from "react-router-dom";

const NavBar = () => {
  const navigate = useNavigate();
  const [expanded, setExpanded] = useState(false);

  const [isLoggedIn, setIsLoggedIn] = useState(false);

  useEffect(() => {
    let token = localStorage.getItem("access_token");
    if (token) {
      setIsLoggedIn(true);
    } else {
      setIsLoggedIn(false);
    }
  }, [localStorage.getItem("access_token")]);

  const logout = () => {
    setExpanded(false);
    localStorage.clear();
    navigate("/");
  };

  return (
    <Navbar
      expanded={expanded} //true
      onToggle={(e) => setExpanded(e)}
      sticky="top"
      expand="md"
      className="navbar-custom px-2"
    >
      <Navbar.Brand
        className="logo"
        onClick={() => {
          navigate("/");
          setExpanded(false);
        }}
      >
        {" "}
        <img src="/images/logo-2.png" alt="logo" />
      </Navbar.Brand>
      <Navbar.Toggle aria-controls="basic-navbar-nav" />
      <Navbar.Collapse id="basic-navbar-nav" className="justify-content-end">
        <Nav className="m-d ">
          <NavLink to="/" onClick={() => setExpanded(false)}>
            {" "}
            <FontAwesomeIcon icon={faHouse} /> Home
          </NavLink>

          <NavLink to="/lost-pets" onClick={() => setExpanded(false)}>
            {" "}
            <FontAwesomeIcon icon={faMagnifyingGlass} /> Lost Pets
          </NavLink>
          {isLoggedIn && (
            <NavLink to="/add-pet" onClick={() => setExpanded(false)}>
              <FontAwesomeIcon icon={faCirclePlus} />
              Add Pet
            </NavLink>
          )}

          {isLoggedIn && (
            <NavLink to="/my-pets" onClick={() => setExpanded(false)}>
              {" "}
              <FontAwesomeIcon icon={faPaw} /> My Pets{" "}
            </NavLink>
          )}

          {isLoggedIn && (
            <NavLink to="/owner" onClick={() => setExpanded(false)}>
              {" "}
              <FontAwesomeIcon icon={faUser} /> Owner
            </NavLink>
          )}

          {!isLoggedIn && (
            <NavLink to="/login" onClick={() => setExpanded(false)}>
              {" "}
              Login
            </NavLink>
          )}
          {!isLoggedIn && (
            <NavLink to="/registration" onClick={() => setExpanded(false)}>
              {" "}
              Sign up
            </NavLink>
          )}

          {isLoggedIn && (
            <Link to={"/"} onClick={() => logout()}>
              {" "}
              Logout
            </Link>
          )}
        </Nav>
      </Navbar.Collapse>
    </Navbar>
  );
};

export default NavBar;
