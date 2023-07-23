import React, { useState, useEffect, useRef } from "react";
import { Button, Card, Col, Spinner } from "react-bootstrap";
import { Row } from "react-bootstrap";
import { Form } from "react-bootstrap";
import { ICreatePetModel, IPetModel } from "../models/petModels";
import { InputGroup } from "react-bootstrap";
import { ICharacteristicModel } from "../models/characteristicModels";
import { characteristicService } from "../Services/characteristicService";
import Select from "react-select";
import { MultiValue, ActionMeta, InputActionMeta } from "react-select";
import { petService } from "../Services/petService";
import { useLocation, useNavigate } from "react-router-dom";
import { faImage } from "@fortawesome/free-solid-svg-icons";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";

interface ISelectOptions {
  label: string;
  value: string;
}
const petEmptyState: IPetModel = {
  id: "",
  ownerId: "",
  characteristicIds: [],
  characteristics: [],
  petLastLocationIds: [],
  petLastLocations: [],
  picture: "",
  name: "",
  type: "",
  breed: "",
  lostStatus: false,
  description: "",
  isSubscribed: true,
  disappearanceDate: "0001-01-01T00:00:00",
  zipCode: "",
};

const AddPet = () => {
  const [validated, setValidated] = useState(false);
  const [isEdited, setIsEdited] = useState(false);
  const [disabledAdd, setDisabledAdd] = useState(false);
  const [pet, setPet] = useState<IPetModel>(petEmptyState);
  const [pictureFile, setPictureFile] = useState<File>(null);
  const [characteritic, setCharacteristics] = useState<ICharacteristicModel[]>(
    []
  );
  const [otherTypeChecked, setOtherTypeChecked] = useState<Boolean>(false);
  const location = useLocation();
  const navigate = useNavigate();
  const inputFileRef = useRef(null);
  const inputRefs = useRef<HTMLInputElement[]>([]);
  const maxFileSize = 4 * 1024 * 1024; //4MB
  const [loadingCharacteristics, setloadingCharacteristics] = useState(true);

  useEffect(() => {
    (async () => {
      if (location.state !== null && location.state.petId !== null) {
        const data = await petService.getPetById(location.state.petId);
        if (data !== null) {
          setPet(data);
          setOtherTypeChecked(pet.type !== "Cat" && pet.type !== "Dog");
          setIsEdited(true);
        }
      } else {
        setPet(petEmptyState);
        setIsEdited(false);
      }
    })();
  }, [location.state]);

  useEffect(() => {
    (async () => {
      var data = await characteristicService.getCharacteristics();
      if (data !== null) {
        setCharacteristics(data);
      }
      setloadingCharacteristics(false);
    })();
  }, []);

  const hasInvalidInput = (): boolean => {
    return (
      inputRefs.current.some((inputRef) =>
        inputRef.classList.contains("is-invalid")
      ) || inputFileRef.current.classList.contains("is-invalid")
    );
  };

  const handleSubmit = async (event) => {
    const form = event.currentTarget;
    event.preventDefault();
    event.stopPropagation();
    setDisabledAdd(true);
    if (form.checkValidity() !== false) {
      if (isEdited) {
        var editedData = await petService.updatePet(pet, pictureFile);
        if (editedData != null) {
          navigate("/my-pets");
        }
      } else {
        var data = await petService.addPet(pet, pictureFile);
        if (data != null) {
          navigate("/my-pets");
        }
      }
    } else {
      window.scrollTo({
        top: 0,
        behavior: "smooth",
      });
    }
    setDisabledAdd(false);
    setValidated(true);
  };

  const characteristicsOptions = characteritic.map<ISelectOptions>((c) => ({
    label: c.name,
    value: c.id,
  }));
  const characteristicValues = characteristicsOptions.filter((c) =>
    pet.characteristicIds.includes(c.value)
  );

  const onSelectChange = (selected: MultiValue<ISelectOptions>) => {
    let characteristicId = selected.map((s) => s.value);
    setPet((prev) => ({ ...prev, characteristicIds: characteristicId }));
  };

  const ImageLink = () => {
    if (pet.picture !== "" && pictureFile == null) {
      return (
        <a
          className="mx-2 btn btn-outline-success btn-sm"
          href={pet.picture}
          target="_blank"
        >
          <FontAwesomeIcon icon={faImage} /> Current image
        </a>
      );
    } else {
      return <> </>;
    }
  };

  return (
    <Form noValidate onSubmit={async (e) => await handleSubmit(e)}>
      <Row className="justify-content-center bg-white  mb-3">
        <Col xs sm="9" md="7" lg="5">
          <Card className="add-pet">
            <Card.Header>
              {" "}
              {isEdited ? "Update your pet data" : "Add your pet"}
            </Card.Header>
            <Card.Body className="p-3">
              <Form.Group controlId="validationCustom01">
                <Form.Label className=" text-start w-100">Name</Form.Label>
                <Form.Control
                  ref={(el) => (inputRefs.current[0] = el)}
                  isInvalid={validated && pet.name.trim() === "" ? true : false}
                  value={pet.name}
                  onChange={(e) =>
                    setPet((prev) => ({ ...prev, name: e.target.value }))
                  }
                  required
                  type="text"
                />
                <Form.Control.Feedback type="invalid">
                  Name is required
                </Form.Control.Feedback>
              </Form.Group>
              <Form.Group controlId="validation">
                <Form.Label className=" text-start w-100">Type </Form.Label>
                <div className="mb-1 text-start">
                  <Form.Check
                    ref={(el) => (inputRefs.current[1] = el)}
                    inline
                    checked={pet.type === "Dog"}
                    isInvalid={validated && pet.type == "" ? true : false}
                    label="Dog"
                    name="radio"
                    type={"radio"}
                    onChange={(e) => {
                      setOtherTypeChecked(false);
                      setPet((prev) => ({ ...prev, type: "Dog" }));
                    }}
                    id={`radio-1`}
                    feedbackType="invalid"
                  />
                  <Form.Check
                    ref={(el) => (inputRefs.current[2] = el)}
                    inline
                    label="Cat"
                    checked={pet.type === "Cat"}
                    isInvalid={validated && pet.type == "" ? true : false}
                    name="radio"
                    type={"radio"}
                    onChange={(e) => {
                      setOtherTypeChecked(false);

                      setPet((prev) => ({ ...prev, type: "Cat" }));
                    }}
                    id={`radio-2`}
                    feedbackType="invalid"
                  />
                  <Form.Check
                    ref={(el) => (inputRefs.current[3] = el)}
                    inline
                    label="Other"
                    checked={
                      pet.type !== "Cat" &&
                      pet.type !== "Dog" &&
                      pet.type !== ""
                    }
                    isInvalid={validated && pet.type == "" ? true : false}
                    name="radio"
                    type={"radio"}
                    onChange={(e) => {
                      setOtherTypeChecked(true);
                      setPet((prev) => ({ ...prev, type: "" }));
                    }}
                    id={`radio-3`}
                    feedbackType="invalid"
                  />
                </div>
                <Form.Control
                  ref={(el) => (inputRefs.current[4] = el)}
                  className={otherTypeChecked ? "" : "d-none"}
                  isInvalid={validated && pet.type.trim() === "" ? true : false}
                  value={pet.type}
                  onChange={(e) =>
                    setPet((prev) => ({ ...prev, type: e.target.value }))
                  }
                  required
                  type="text"
                />
                <Form.Control.Feedback type="invalid">
                  Type is required
                </Form.Control.Feedback>
              </Form.Group>

              <Form.Group controlId="validation">
                <Form.Label className=" text-start w-100">Breed</Form.Label>
                <Form.Control
                  value={pet.breed}
                  onChange={(e) =>
                    setPet((prev) => ({ ...prev, breed: e.target.value }))
                  }
                  type="text"
                />
              </Form.Group>

              <Form.Group controlId="validation">
                <Form.Label className=" text-start w-100">Zip Code </Form.Label>
                <Form.Control
                  ref={(el) => (inputRefs.current[5] = el)}
                  isInvalid={
                    validated &&
                    (pet.zipCode.trim() === "" ||
                      isNaN(Number(pet.zipCode)) ||
                      pet.zipCode.length !== 5)
                      ? true
                      : false
                  }
                  value={pet.zipCode}
                  onChange={(e) =>
                    setPet((prev) => ({ ...prev, zipCode: e.target.value }))
                  }
                  required
                  type="text"
                />
                <Form.Control.Feedback type="invalid">
                  Zip Code is required and it needs to be 5 numbers
                </Form.Control.Feedback>
              </Form.Group>

              <Form.Group controlId="validation">
                <Form.Label className=" text-start w-100">
                  Description
                </Form.Label>
                <Form.Control
                  onChange={(e) =>
                    setPet((prev) => ({ ...prev, description: e.target.value }))
                  }
                  as="textarea"
                  aria-describedby="petDescription"
                  value={pet.description}
                />

                <Form.Text
                  className="w-100  text-start"
                  id="petDescription"
                  muted
                >
                  Please describe the physical characteristics of your pet in a
                  short description so that your pet is recognizable to the
                  potencial finder.
                </Form.Text>
              </Form.Group>

              <Form.Group
                controlId="validationCharacteristicIds"
                className="mt-2"
              >
                <Form.Label className=" text-start w-100">
                  Characteristics
                </Form.Label>
                <Select
                  isLoading={loadingCharacteristics}
                  name="characteristicIds"
                  value={characteristicValues}
                  options={characteristicsOptions}
                  className="basic-multi-select  w-100"
                  classNamePrefix="selectPrefix"
                  onChange={(newValue) => onSelectChange(newValue)}
                  isMulti
                />
              </Form.Group>

              <Form.Group className="mt-3">
                <Form.Check
                  type={"checkbox"}
                  label={`Is your pet lost`}
                  className="w-100  text-start"
                  checked={pet.lostStatus}
                  onChange={(e) => {
                    setPet((prev) => ({
                      ...prev,
                      lostStatus: e.target.checked,
                      disappearanceDate: e.target.checked
                        ? prev.disappearanceDate
                        : "0001-01-01T00:00:00",
                    }));
                  }}
                />
              </Form.Group>

              {pet.lostStatus && (
                <Form.Group controlId="validationDate">
                  <Form.Label className=" text-start w-100">
                    Disaperiance date
                  </Form.Label>
                  <Form.Control
                    ref={(el) => (inputRefs.current[6] = el)}
                    isInvalid={
                      validated &&
                      pet.disappearanceDate == "0001-01-01T00:00:00" &&
                      pet.lostStatus == true
                        ? true
                        : false
                    }
                    onChange={(e) =>
                      setPet((prev) => ({
                        ...prev,
                        disappearanceDate: e.target.value,
                      }))
                    }
                    required={pet.lostStatus == true}
                    type="date"
                    value={
                      new Date(pet.disappearanceDate)
                        .toISOString()
                        .split("T")[0]
                    }
                    max={new Date().toISOString().split("T")[0]} // poslednji datum koji moze da se izabere je danasnji dan
                  />
                  <Form.Control.Feedback type="invalid">
                    Disaperiance date is required
                  </Form.Control.Feedback>
                </Form.Group>
              )}

              <Form.Group controlId="validation" className="mt-2">
                <Form.Label className=" text-start w-100">
                  Image {<ImageLink />}
                </Form.Label>

                <InputGroup className="mb-3">
                  <Form.Control
                    isInvalid={
                      validated &&
                      pictureFile != null &&
                      pictureFile.size > maxFileSize
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
                disabled={disabledAdd}
                className="mt-3 submit-btn"
                type="submit"
              >
                {isEdited ? "Update" : "Add"}
                {disabledAdd && (
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

export default AddPet;
