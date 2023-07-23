import React, { useState, useEffect } from "react";
import { IPetModel } from "../models/petModels";
import { petService } from "../Services/petService";
import { Button, Card, Col, Container, Row, Spinner } from "react-bootstrap";
import { useNavigate } from "react-router-dom";
import { Prev } from "react-bootstrap/lib/Pagination";
import PetCard from "../components/PetCard";
import { accountUtils } from "../utils/AccountUtils";

const MyPets = () => {
  const [pets, setPets] = useState<IPetModel[]>([]);
  const [loading, setloading] = useState(true);
  const [disableActionBtns, setDisableActionBtns] = useState(false);

  const navigate = useNavigate();

  const deletePet = async (id: string) => {
    setDisableActionBtns(true);
    const data = await petService.deletePet(id);
    if (data != null) {
      let filteredPets = pets.filter((pet) => pet.id !== id);
      setPets(filteredPets);
    }
    setDisableActionBtns(false);
  };
  const changeLostStatus = async (id: string, lostStatus: boolean) => {
    setDisableActionBtns(true);

    let data = await petService.updatePetStatus(id, lostStatus);
    if (data !== null) {
      const newPetsState = pets.map((pet) => {
        if (pet.id === id) {
          return {
            ...pet,
            lostStatus: data.lostStatus,
            disappearanceDate: data.disappearanceDate,
          };
        }
        return pet;
      });
      setPets(newPetsState);
    }
    setDisableActionBtns(false);
  };

  useEffect(() => {
    (async () => {
      const userId = accountUtils.getUserId();
      if (userId !== null) {
        var data = await petService.getPetsByOwnerId(userId);
        if (data !== null) {
          setPets(data);
        }
      }
      setloading(false);
    })();
  }, []);

  const showPets = pets.map((pet, index) => (
    <Col className="p-3 d-flex align-items-stretch justify-content-center">
      <PetCard
        key={index}
        pet={pet}
        onChangeLostStatus={(id, lostStatus) => {
          changeLostStatus(id, lostStatus);
        }}
        onDeletePet={(id) => {
          deletePet(id);
        }}
        idEditable={true}
        disableActionBtns={disableActionBtns}
      />
    </Col>
  ));

  return (
    <Container>
      <Row className="naslovi">
        <h3 className="header-text">My Pets</h3>
      </Row>
      {loading ? (
        <Spinner animation="border" className="spinner-color mt-5" />
      ) : pets.length == 0 ? (
        <Row className="cards-container">
          <Col className="my-4">
            <h4>You dont have pets</h4>
            <Button className="red-btn" onClick={() => navigate("/add-pet")}>
              Add pet
            </Button>
          </Col>
        </Row>
      ) : (
        <Row xs={1} md={2} lg={3} xl={4} className="cards-container">
          {showPets}
        </Row>
      )}
    </Container>
  );
};

export default MyPets;
