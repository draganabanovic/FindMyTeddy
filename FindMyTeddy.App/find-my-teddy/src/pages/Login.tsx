import React, { useRef, useState } from "react";
import { ICreateUserModel } from "../models/userModels";
import { Button, Card, Col, Form, Row, Spinner } from "react-bootstrap";
import { userService } from "../Services/userService";
import { useNavigate } from "react-router-dom";

const Login = () => {
  const navigate = useNavigate();
  const [login, setLogin] = useState({
    email: "",
    password: "",
  });
  const [disabledSubmit, setDisabledSubmit] = useState(false);
  const [validated, setValidated] = useState(false);
  const inputRefs = useRef<HTMLInputElement[]>([]);

  const hasInvalidInput = (): boolean => {
    return inputRefs.current.some((inputRef) =>
      inputRef.classList.contains("is-invalid")
    );
  };
  function isEmailValid(email: string) {
    const regex = /^[A-Z0-9._%+-]+@[A-Z0-9.-]+\.[A-Z]{2,}$/i;
    if (regex.test(email)) {
      return true;
    }
    return false;
  }

  const handleSubmit = async (event: any) => {
    const form = event.currentTarget;
    event.preventDefault();
    event.stopPropagation();
    console.log(form.checkValidity());
    if (form.checkValidity() !== false && !hasInvalidInput()) {
      setDisabledSubmit(true);
      var data = await userService.login(login.email, login.password);
      if (data !== null) {
        navigate("/");
      }
      setDisabledSubmit(false);
    }
    setValidated(true);
  };

  const preventSpaceOnKeyDown = (event) => {
    if (event.key === " ") {
      event.preventDefault();
    }
  };

  return (
    <Form noValidate onSubmit={async (e) => await handleSubmit(e)}>
      <Row className="justify-content-center bg-white  mb-3">
        <Col xs sm="9" md="7" lg="4">
          <Card className="add-pet">
            <Card.Header>Login </Card.Header>
            <Card.Body className="p-3">
              <Form.Group controlId="validationCustom01">
                <Form.Label className=" text-start w-100">Email </Form.Label>
                <Form.Control
                  ref={(el) => (inputRefs.current[0] = el)}
                  isInvalid={
                    validated &&
                    (login.email.trim() === "" || !isEmailValid(login.email))
                      ? true
                      : false
                  }
                  value={login.email}
                  onChange={(e) =>
                    setLogin((prev) => ({ ...prev, email: e.target.value }))
                  }
                  required
                  type="text"
                />
              </Form.Group>
              <Form.Group controlId="validationCustom01">
                <Form.Label className=" text-start w-100">Password </Form.Label>
                <Form.Control
                  ref={(el) => (inputRefs.current[1] = el)}
                  isInvalid={validated && login.password === "" ? true : false}
                  onKeyDown={preventSpaceOnKeyDown}
                  value={login.password}
                  onChange={(e) =>
                    setLogin((prev) => ({ ...prev, password: e.target.value }))
                  }
                  required
                  type="password"
                />
              </Form.Group>
              <Button
                disabled={disabledSubmit}
                className="mt-3 submit-btn"
                type="submit"
              >
                Login{" "}
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

export default Login;
