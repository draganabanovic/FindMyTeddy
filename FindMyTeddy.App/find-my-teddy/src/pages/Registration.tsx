import React, { useRef } from "react";
import { useState, useEffect } from "react";
import { ICreateUserModel, IUserModel } from "../models/userModels";
import { userService } from "../Services/userService";
import {
  Button,
  ButtonGroup,
  Card,
  Col,
  Form,
  InputGroup,
  Row,
  Spinner,
} from "react-bootstrap";

import { accountUtils } from "../utils/AccountUtils";
import { useNavigate } from "react-router-dom";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import { faImage, faX } from "@fortawesome/free-solid-svg-icons";

const Registration = () => {
  const [user, setUser] = useState<IUserModel>({
    firstName: "",
    lastName: "",
    email: "",
    password: "",
    city: "",
    street: "",
    phone: "",
    id: "",
    profilePicture: "",
    role: "",
  });
  const [validated, setValidated] = useState(false);
  const [IsEdit, setIsEdited] = useState<boolean>(accountUtils.isLogedIn());
  const [pictureFile, setPictureFile] = useState<File>(null);
  const [passwordCheck, setPasswordCheck] = useState("");
  const [disabledSubmit, setDisabledSubmit] = useState(false);
  const navigate = useNavigate();
  const inputFileRef = useRef(null);
  const inputRefs = useRef<HTMLInputElement[]>([]);
  const maxFileSize = 4 * 1024 * 1024; //4MB

  useEffect(() => {
    (async () => {
      if (IsEdit) {
        const userId = accountUtils.getUserId();
        const data = await userService.getUserById(userId);
        if (data !== null) {
          setUser(data);
        }
      }
    })();
  }, [IsEdit]);

  const hasInvalidInput = (): boolean => {
    return (
      inputRefs.current.some((inputRef) =>
        inputRef.classList.contains("is-invalid")
      ) || inputFileRef.current.classList.contains("is-invalid")
    );
  };
  const handleSubmit = async (event: any) => {
    const form = event.currentTarget;
    event.preventDefault();
    event.stopPropagation();

    if (form.checkValidity() !== false && !hasInvalidInput()) {
      setDisabledSubmit(true);
      if (IsEdit) {
        var data = await userService.update(user, pictureFile);
        if (data != null) {
          navigate("/owner");
        }
      } else {
        var data = await userService.register(user, pictureFile);
        if (data != null) {
          navigate("/login");
        }
      }
    } else {
      window.scrollTo({
        top: 0,
        behavior: "smooth",
      });
    }
    setDisabledSubmit(false);
    setValidated(true);
  };

  const preventSpaceOnKeyDown = (event) => {
    if (event.key === " ") {
      event.preventDefault();
    }
  };

  const ImageLink = () => {
    if (user.profilePicture !== "" && pictureFile == null) {
      return (
        <a
          className="mx-2 btn btn-outline-success btn-sm"
          href={user.profilePicture}
          target="_blank"
        >
          <FontAwesomeIcon icon={faImage} /> Current image
        </a>
      );
    } else {
      return <> </>;
    }
  };
  function isPasswordValid(password: string) {
    const regex = /^(?=.*[!@#$%^&*])(?=.*\d)(?=.*[A-Z]).{8,}$/;
    if (regex.test(password)) {
      return true;
    }
    return false;
  }
  function isEmailValid(email: string) {
    const regex = /^[A-Z0-9._%+-]+@[A-Z0-9.-]+\.[A-Z]{2,}$/i;
    if (regex.test(email)) {
      return true;
    }
    return false;
  }

  const invalidPassword = () => {
    if (IsEdit) {
      return passwordCheck !== "" ||
        (user.password !== "" && user.password !== null)
        ? !isPasswordValid(user.password)
        : false;
    }
    return user.password === "" || !isPasswordValid(user.password);
  };

  const invalidPasswordConfirm = () => {
    if (IsEdit) {
      if (
        (user.password !== "" && user.password !== null) ||
        passwordCheck !== ""
      )
        return user.password != passwordCheck;
      return false;
    }
    return user.password != passwordCheck;
  };

  return (
    <Form noValidate onSubmit={async (e) => await handleSubmit(e)}>
      <Row className="justify-content-center bg-white  mb-3">
        <Col xs sm="9" md="7" lg="5">
          <Card className="add-pet">
            <Card.Header>
              {" "}
              {IsEdit ? "Edit profil" : "Create your account"}{" "}
            </Card.Header>
            <Card.Body className="p-3">
              <Form.Group controlId="validationCustom01">
                <Form.Label className=" text-start w-100">
                  First Name
                </Form.Label>
                <Form.Control
                  ref={(el) => (inputRefs.current[0] = el)}
                  isInvalid={
                    validated && user.firstName.trim() === "" ? true : false
                  }
                  value={user.firstName}
                  onChange={(e) =>
                    setUser((prev) => ({ ...prev, firstName: e.target.value }))
                  }
                  required
                  type="text"
                />
                <Form.Control.Feedback type="invalid">
                  First name is required
                </Form.Control.Feedback>
              </Form.Group>
              <Form.Group controlId="validation">
                <Form.Label className=" text-start w-100">Last Name</Form.Label>
                <Form.Control
                  ref={(el) => (inputRefs.current[1] = el)}
                  isInvalid={
                    validated && user.lastName.trim() === "" ? true : false
                  }
                  value={user.lastName}
                  onChange={(e) =>
                    setUser((prev) => ({ ...prev, lastName: e.target.value }))
                  }
                  required
                  type="text"
                />
                <Form.Control.Feedback type="invalid">
                  Last name is required
                </Form.Control.Feedback>
              </Form.Group>
              <Form.Group controlId="validation">
                <Form.Label className=" text-start w-100">Email</Form.Label>
                <Form.Control
                  ref={(el) => (inputRefs.current[2] = el)}
                  isInvalid={
                    validated &&
                    (user.email.trim() === "" || !isEmailValid(user.email))
                      ? true
                      : false
                  }
                  value={user.email}
                  onChange={(e) =>
                    setUser((prev) => ({ ...prev, email: e.target.value }))
                  }
                  required
                  type="text"
                />
                <Form.Control.Feedback type="invalid">
                  Email is required
                </Form.Control.Feedback>
              </Form.Group>
              <Form.Group controlId="validationCustom01">
                <Form.Label className=" text-start w-100">Password</Form.Label>
                <Form.Control
                  onKeyDown={preventSpaceOnKeyDown}
                  ref={(el) => (inputRefs.current[3] = el)}
                  isInvalid={validated && invalidPassword() ? true : false}
                  value={user.password}
                  onChange={(e) =>
                    setUser((prev) => ({ ...prev, password: e.target.value }))
                  }
                  type="password"
                />
                <Form.Control.Feedback type="invalid">
                  Password must have at least 8 characters with at least one
                  special character, digit and uppercase
                </Form.Control.Feedback>
              </Form.Group>
              <Form.Group controlId="validationCustom01">
                <Form.Label className=" text-start w-100">
                  Confirm Password
                </Form.Label>
                <Form.Control
                  onKeyDown={preventSpaceOnKeyDown}
                  ref={(el) => (inputRefs.current[4] = el)}
                  isInvalid={
                    validated && invalidPasswordConfirm() ? true : false
                  }
                  value={passwordCheck}
                  onChange={(e) => setPasswordCheck(e.target.value)}
                  type="password"
                />
                <Form.Control.Feedback type="invalid">
                  Password mismatch
                </Form.Control.Feedback>
              </Form.Group>
              <Form.Group controlId="validation">
                <Form.Label className=" text-start w-100">
                  Phone Number{" "}
                </Form.Label>
                <Form.Control
                  ref={(el) => (inputRefs.current[5] = el)}
                  isInvalid={
                    validated && user.phone.trim() === "" ? true : false
                  }
                  value={user.phone}
                  onChange={(e) =>
                    setUser((prev) => ({ ...prev, phone: e.target.value }))
                  }
                  required
                  type="text"
                />
                <Form.Control.Feedback type="invalid">
                  Phone is required
                </Form.Control.Feedback>
              </Form.Group>

              <Form.Group controlId="validation">
                <Form.Label className=" text-start w-100">City</Form.Label>
                <Form.Control
                  ref={(el) => (inputRefs.current[6] = el)}
                  isInvalid={validated && user.city == "" ? true : false}
                  value={user.city}
                  onChange={(e) =>
                    setUser((prev) => ({ ...prev, city: e.target.value }))
                  }
                  required
                  type="text"
                />
                <Form.Control.Feedback type="invalid">
                  City is required
                </Form.Control.Feedback>
              </Form.Group>
              <Form.Group controlId="validation">
                <Form.Label className=" text-start w-100">Street </Form.Label>
                <Form.Control
                  ref={(el) => (inputRefs.current[7] = el)}
                  isInvalid={validated && user.street == "" ? true : false}
                  value={user.street}
                  onChange={(e) =>
                    setUser((prev) => ({ ...prev, street: e.target.value }))
                  }
                  required
                  type="text"
                />
                <Form.Control.Feedback type="invalid">
                  Street is required
                </Form.Control.Feedback>
              </Form.Group>

              <Form.Group controlId="validation" className="mt-2">
                <Form.Label className=" text-start w-100">
                  Profile Picture {<ImageLink />}
                </Form.Label>
                <InputGroup className="mb-3">
                  <Form.Control
                    isInvalid={
                      pictureFile != null && pictureFile.size > maxFileSize
                        ? true
                        : false
                    }
                    ref={inputFileRef}
                    type="file"
                    accept="image/*"
                    onChange={(e) => {
                      const formData = (e.target as HTMLInputElement).files[0];
                      setPictureFile(formData);
                    }}
                  />
                  {pictureFile != null && (
                    <Button
                      variant="outline-danger"
                      onClick={() => {
                        setPictureFile(null);
                        inputFileRef.current.value = null;
                      }}
                    >
                      X
                    </Button>
                  )}
                  <Form.Control.Feedback type="invalid">
                    Please upload a picture that is less then 4MB
                  </Form.Control.Feedback>
                </InputGroup>
              </Form.Group>
              <Button
                disabled={disabledSubmit}
                className="mt-3 submit-btn"
                type="submit"
              >
                {IsEdit ? "Edit" : "Create"}
                {disabledSubmit && (
                  <Spinner size="sm" animation="border" className=" ms-2" />
                )}
              </Button>
            </Card.Body>
          </Card>
        </Col>
      </Row>
    </Form>
  );
};

export default Registration;
