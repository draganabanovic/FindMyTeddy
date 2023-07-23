import React, { useRef, useState } from "react";
import { Card, Col, Row, Button, Modal } from "react-bootstrap";
import { IPetModel } from "../models/petModels";
import { useNavigate } from "react-router-dom";
import { format } from "date-fns";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import { faTrash } from "@fortawesome/free-solid-svg-icons";
import { faPenToSquare } from "@fortawesome/free-regular-svg-icons";
import DownloadQRButton from "./DownloadQRButton";

interface IProps {
  pet: IPetModel;
  onDeletePet?: (id: string) => void;
  onChangeLostStatus?: (id: string, lostStatus: boolean) => void;
  idEditable: boolean;
  disableActionBtns?: boolean;
}

const PetCard = ({
  pet,
  onChangeLostStatus,
  onDeletePet,
  idEditable,
  disableActionBtns,
}: IProps) => {
  const navigate = useNavigate();
  const [showModal, setShowModal] = useState(false);

  const handleCloseDeleteModal = () => setShowModal(false);
  const handleShowDeleteModal = () => setShowModal(true);

  const navigateToPetInfo = (petId: string) => {
    navigate(`/pet/${petId}`);
  };
  const navigateToEditPet = (id: string) => {
    navigate("/edit-pet", { state: { petId: id } });
  };

  const imgRef = useRef<any>();
  const onImageError = () => (imgRef.current.src = "/images/oglasavanje2.png");

  return (
    <Card>
      <Card.Img
        ref={imgRef}
        variant="top"
        src={pet.picture}
        onError={onImageError}
        className="p-1 rounded clickable"
        onClick={() => navigateToPetInfo(pet.id)}
      />

      <Card.Body className="d-flex flex-column">
        <Card.Title>{pet.name}</Card.Title>
        <Card.Text>
          {pet.type && pet.type != "" && <p> Type: {pet.type}</p>}
          {pet.zipCode && pet.zipCode != "" && <p> ZipCode: {pet.zipCode}</p>}
          {pet.lostStatus && pet.disappearanceDate && (
            <p>
              Disappearance date:
              {format(new Date(pet.disappearanceDate), "dd.MM.yyyy HH:mm")}
            </p>
          )}
          {idEditable && (
            <p>
              Change status to :
              <Button
                disabled={disableActionBtns}
                size="sm"
                variant="outline-secondary"
                className="ms-2"
                onClick={() => onChangeLostStatus(pet.id, !pet.lostStatus)}
              >
                {pet.lostStatus ? "Founded" : "Lost"}
              </Button>
            </p>
          )}
        </Card.Text>
        <Row className="mt-auto justify-content-center">
          <Row>
            <Button
              className="red-btn"
              onClick={() => navigateToPetInfo(pet.id)}
            >
              More details
            </Button>
          </Row>

          {idEditable && (
            <Row className="mt-2 align-items-center">
              <Col>
                <Button
                  disabled={disableActionBtns}
                  className="red-btn"
                  title="Delete"
                  onClick={handleShowDeleteModal}
                >
                  <FontAwesomeIcon icon={faTrash} />
                </Button>
              </Col>
              <Col>
                <Button
                  className="red-btn"
                  title="Edit"
                  onClick={() => navigateToEditPet(pet.id)}
                >
                  <FontAwesomeIcon icon={faPenToSquare} />
                </Button>
              </Col>

              <Col>
                <DownloadQRButton
                  className="red-btn ms-1"
                  url={`https://findmyteddy.azurewebsites.net/pet/${pet.id}`}
                />
              </Col>
            </Row>
          )}
        </Row>
      </Card.Body>

      <Modal show={showModal} onHide={handleCloseDeleteModal} centered>
        <Modal.Header closeButton>
          <Modal.Title>Are you sure?</Modal.Title>
        </Modal.Header>
        <Modal.Body>Do you wont to delete this pet?</Modal.Body>
        <Modal.Footer>
          <Button variant="secondary" onClick={handleCloseDeleteModal}>
            Cancel
          </Button>
          <Button
            variant="danger"
            onClick={() => {
              onDeletePet(pet.id);
              handleCloseDeleteModal();
            }}
          >
            Delete
          </Button>
        </Modal.Footer>
      </Modal>
    </Card>
  );
};

export default PetCard;
