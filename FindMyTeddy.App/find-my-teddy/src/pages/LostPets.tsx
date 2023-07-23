import React, { useEffect, useState } from "react";
import { Button, Col, Form, InputGroup, Row, Spinner } from "react-bootstrap";
import { Container } from "react-bootstrap";
import { IPetModel } from "../models/petModels";
import { petService } from "../Services/petService";
import PetCard from "../components/PetCard";

const LostPets = () => {
  const [pets, setPets] = useState<IPetModel[]>([]);
  const [loading, setloading] = useState(true);
  const [zipCode, setZipCode] = useState("");

  useEffect(() => {
    (async () => {
      var data = await petService.getAllLost();
      if (data !== null) {
        setPets(data);
      }
      setloading(false);
    })();
  }, []);

  const showWithZipCode = () => {
    setloading(true);
    (async () => {
      if (zipCode.trim() === "") {
        var data = await petService.getAllLost();
      } else {
        var data = await petService.getLostForZipCode(zipCode);
      }
      if (data !== null) {
        setPets(data);
      }
      setloading(false);
    })();
  };

  const handleKeyDown = (event) => {
    if (event.key === "Enter") {
      showWithZipCode();
    }
  };

  const showPets = pets.map((pet, index) => (
    <Col className="p-3 d-flex align-items-stretch justify-content-center">
      <PetCard key={index} pet={pet} idEditable={false} />
    </Col>
  ));

  return (
    <Container>
      <Row className="naslovi">
        <h3 className="header-text">Lost Pets</h3>

        <InputGroup className="pb-3">
          <Form.Control
            placeholder="Pet Zip Code"
            aria-label="PET ZIP CODE"
            value={zipCode}
            onKeyDown={handleKeyDown}
            onChange={(e) => setZipCode(e.target.value)}
          />
          <Button
            variant="outline-secondary"
            id="button-addon2"
            onClick={() => showWithZipCode()}
          >
            Search
          </Button>
        </InputGroup>
      </Row>
      {loading ? (
        <Spinner animation="border" className="spinner-color mt-5" />
      ) : (
        <Row xs={1} md={2} lg={3} xl={4} className="cards-container">
          {showPets}
        </Row>
      )}
    </Container>
  );
};

export default LostPets;
