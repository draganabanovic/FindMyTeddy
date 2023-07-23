import React from "react";
import { Button, Col, Container, Row } from "react-bootstrap";
import { useNavigate } from "react-router-dom";

const Home = () => {
  const navigate = useNavigate();

  const navigateToAddPet = () => {
    navigate(`/add-pet`);
  };

  return (
    <div className="container bg-white">
      <div className="my-5 d-flex flex-column flex-md-row  align-items-start">
        <div className="col align-self-center">
          {" "}
          <img
            className="img-fluid"
            src="/images/naslovna2.jpg"
            width={400}
            alt="logo"
          />
        </div>
        <div className="col justify-content-center align-self-center">
          <h1 className="my-5">
            {" "}
            Our <b> PERFECT COMPANIONS </b>never have fewer than{" "}
            <b> FOUR FEET </b>{" "}
          </h1>
          <Button className="red-btn" onClick={navigateToAddPet}>
            JOIN YOUR PET
          </Button>
        </div>
      </div>

      <div className=" d-flex flex-column flex-md-row  align-items-start">
        <div className="mx-5 col align-self-center">
          <p>
            {" "}
            A large number of pets are lost, strayed, or stolen every year, and
            many owners never find them. A microchip is a permanent
            identification of dogs and a legal obligation of all owners.
            However, this is not an effective way to return a pet to its owner.
            Only the veterinary station and animal hygiene service have access
            to the database. Passers-by often do not have the time and
            conditions to hand over the pet to the competent service. In such
            systems, information about the owner is not always relevant, so the
            owner is often not found, because changing the data is complicated.{" "}
          </p>
          <img
            className="img-fluid"
            src="/images/lost.jpg"
            width={200}
            alt="lost"
          />
        </div>
        <div className="mx-5 col align-self-center">
          <div className="advantages content">
            <h1>WHY WE ARE BETTER THAN MICROCHIP?</h1>
            <p> FindMyTeddy offers its users the following options:</p>
            <p> 1. Fast advertising of lost pets </p>
            <p> 2. Relevant information about the owner </p>
            <p> 3. Continuous monitoring of pet movements </p>
            <p> 4. Easy and fast change of data </p>
            <p> 5. Saving time </p>
            <p>6. Painless pet identification </p>
          </div>
        </div>
      </div>
      <div className=" my-4 d-flex flex-column flex-md-row  align-items-start">
        <div className="col align-self-center">
          {" "}
          <h1 className="my-3"> HOW IT WORKS?</h1>
        </div>
      </div>

      <div className=" d-flex flex-column flex-md-row  align-items-start">
        <div className="col justify-content-center  align-self-start my-2">
          <img
            className="infopic img-fluid"
            src="/images/oglasavanje2.png"
            width={200}
            alt="annoucing"
          />
          <h2 className=" my-2 nasloviopisa"> Annoucing </h2>
          <p className="mx-3">
            {" "}
            By registering the pet and changing its status, your pet will be on
            the lost pets page.
          </p>
        </div>
        <div className="col justify-content-center align-self-start my-2">
          <img
            className="infopic img-fluid"
            src="/images/lociranje.jpg"
            width={200}
            alt="locating"
          />
          <h2 className="my-2 nasloviopisa"> Locating </h2>
          <p className="mx-3">
            {" "}
            When the finder scans the pet's QR, he gets the owner's contact
            information. On the other hand, if the finder shares his location,
            the owner can follow the movement of the lost pet.
          </p>
        </div>
        <div className="col justify-content-center  align-self-start my-2">
          <img
            className="infopic img-fluid"
            src="/images/reunion.jpg"
            width={200}
            alt="reunion"
          />
          <h2 className="my-2 nasloviopisa"> Reunion </h2>
          <p className="mx-3">
            {" "}
            Happy reunions! Get your pet home sooner. He will thank you.
          </p>
        </div>
      </div>
    </div>
  );
};

export default Home;
