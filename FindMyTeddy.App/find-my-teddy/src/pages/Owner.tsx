import React, { useEffect, useRef, useState } from "react";
import { IUserModel } from "../models/userModels";
import { userService } from "../Services/userService";
import { Col, Container, Row, Spinner } from "react-bootstrap";
import { accountUtils } from "../utils/AccountUtils";
import { Button } from "react-bootstrap";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import { faPenToSquare } from "@fortawesome/free-regular-svg-icons";
import { useNavigate } from "react-router-dom";

const Owner = () => {
  const [owner, setOwner] = useState<IUserModel>({
    firstName: "",
    lastName: "",
    city: "",
    email: "",
    profilePicture: "",
    role: "",
    street: "",
    id: "",
    phone: "",
    password: "",
  });
  const navigate = useNavigate();
  const imgRef = useRef<any>();
  const onImageError = () => (imgRef.current.src = "/images/default-user.png");
  const [loading, setloading] = useState(true);

  useEffect(() => {
    (async () => {
      const userId = accountUtils.getUserId();
      if (userId !== null) {
        var data = await userService.getUserById(userId);
        if (data !== null) {
          setOwner(data);
        }
      }
      setloading(false);
    })();
  }, []);

  const navigateToEdit = () => {
    navigate("/owner/edit");
  };
  return (
    <Container>
      <Row className="naslovi">
        <div className="h3">Owner </div>
      </Row>

      {loading ? (
        <Spinner animation="border" className="spinner-color mt-5" />
      ) : (
        <Row className="justify-content-center cards-container py-2">
          <Col md lg="4">
            <img
              ref={imgRef}
              src={owner.profilePicture}
              alt="profil"
              className="profil-img rounded"
              onError={onImageError}
            />
          </Col>
          <Col
            md
            lg={4}
            className="justify-content-md-center d-flex align-items-center"
          >
            <Row>
              <div className="my-1">
                <span className="fw-bold">Name:</span>{" "}
                {owner.firstName + " " + owner.lastName}
              </div>
              <div className="my-1">
                <span className="fw-bold">City:</span> {owner.city}
              </div>
              <div className="my-1">
                <span className="fw-bold">Street:</span> {owner.street}
              </div>

              <div className="my-1">
                <span className="fw-bold">Email:</span> {owner.email}
              </div>
              <div className="my-1">
                <span className="fw-bold">Phone:</span> {owner.phone}
              </div>
              <div className="my-1">
                <span className="fw-bold">Role:</span> {owner.role}
              </div>
              <div className="my-1">
                <Button
                  className="red-btn"
                  title="Edit"
                  onClick={() => navigateToEdit()}
                >
                  <FontAwesomeIcon icon={faPenToSquare} />
                </Button>
              </div>
            </Row>
          </Col>
        </Row>
      )}
    </Container>
  );
};

export default Owner;
