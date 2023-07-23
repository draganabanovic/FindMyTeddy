import { useLocation, useNavigate, useParams } from "react-router-dom";
import React, { useState, useEffect, useRef } from "react";
import { IPetModel } from "../models/petModels";
import { petService } from "../Services/petService";
import QRCode from "react-qr-code";

import { NotificationManager } from "react-notifications";
import GoogleMaps from "../components/GoogleMaps";
import { Button, Col, Row, Spinner } from "react-bootstrap";
import { PetLastLocationService } from "../Services/petLastLocationService";
import { IUserModel } from "../models/userModels";
import { userService } from "../Services/userService";
import { format } from "date-fns";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import { faMapLocationDot } from "@fortawesome/free-solid-svg-icons";
import DownloadQRButton from "../components/DownloadQRButton";
import { accountUtils } from "../utils/AccountUtils";

interface IMapPOsition {
  lat: number;
  lng: number;
}

const Pet = () => {
  let { petId } = useParams();
  const navigate = useNavigate();
  const [loading, setloading] = useState(true);

  const [pet, setPet] = useState<IPetModel>({
    ownerId: "",
    characteristicIds: [],
    id: "",
    name: "",
    type: "",
    breed: "",
    picture: "",
    lostStatus: false,
    description: "",
    isSubscribed: true,
    disappearanceDate: "",
    characteristics: [],
    petLastLocationIds: [],
    petLastLocations: [],
    zipCode: "",
  });
  const [owner, setOwner] = useState<IUserModel>({
    id: "",
    role: "",
    firstName: "",
    lastName: "",
    email: "",
    city: "",
    street: "",
    profilePicture: "",
    phone: "",
    password: "",
  });
  const [isOwnerLooking, setisOwnerLooking] = useState(false);
  const [showShereLocation, setShowShereLocation] = useState(true);
  const [disableShereLocation, setDisableShereLocation] = useState(false);

  const [mapPosition, setMapPosition] = useState<IMapPOsition>({
    lat: 0,
    lng: 0,
  });
  const [selectedMapIndex, setSelectedMapIndex] = useState<number>(0);

  const imgRef = useRef<any>();
  const onImageError = () => (imgRef.current.src = "/images/oglasavanje2.png");

  useEffect(() => {
    (async () => {
      if (petId !== null && petId !== undefined) {
        const data: IPetModel = await petService.getPetById(petId);
        if (data !== null) {
          setPet(data);
          if (data.petLastLocations.length !== 0) {
            setMapPosition({
              lat: data.petLastLocations[0].latitude,
              lng: data.petLastLocations[0].longitude,
            });
          }
        } else {
          navigate("/");
        }
        const userId = accountUtils.getUserId();
        const owner: IUserModel = await userService.getUserByPetId(petId);
        if (owner !== null) {
          if (userId === owner.id) {
            setisOwnerLooking(true);
          }
          setOwner(owner);
        }
      }
      setloading(false);
    })();
  }, [petId]);

  const shareLocation = () => {
    setDisableShereLocation(true);
    if (
      !/Android|webOS|iPhone|iPad|iPod|BlackBerry|IEMobile|Opera Mini/i.test(
        navigator.userAgent
      )
    ) {
      NotificationManager.info(
        "Location sharing is only enabled on mobile devices due to the accuracy of location sharing.",
        "",
        5000
      );
      setShowShereLocation(false);
      setDisableShereLocation(false);
      return;
    }
    if ("geolocation" in navigator) {
      navigator.geolocation.getCurrentPosition(
        (position) => {
          PetLastLocationService.addLocation({
            id: "",
            isRelevant: true,
            latitude: position.coords.latitude,
            longitude: position.coords.longitude,
            petId: pet.id,
            lastLocationDateTime: new Date(),
          }).then((location) => {
            if (location !== null) {
              setPet((prev) => ({
                ...prev,
                petLastLocations: [location, ...prev.petLastLocations],
              }));
              setShowShereLocation(false);
              NotificationManager.success(
                "Thank you for sharing location",
                "",
                2000
              );
            }
            setDisableShereLocation(false);
          });
        },
        (error) => {
          if (error.code === 1) {
            if (
              window.confirm(
                "Please enable location services in your device settings and open app again."
              )
            ) {
              if (/android/i.test(navigator.userAgent.toLowerCase())) {
                window.location.href =
                  "content://com.android.settings/com.android.settings.Settings$LocationSettingsActivity";
              } else if (
                /iphone|ipad|ipod/i.test(navigator.userAgent.toLowerCase())
              ) {
                window.location.href = "App-Prefs:root=Privacy&path=LOCATION";
              }
            }
          } else {
            NotificationManager.info(
              "Geolocation information is unavailable",
              "",
              2000
            );
            setShowShereLocation(false);
            setDisableShereLocation(false);
          }
        }
      );
    } else {
      NotificationManager.info(
        "Geolocation information is unavailable",
        "",
        2000
      );
      setShowShereLocation(false);
      setDisableShereLocation(false);
    }
  };

  if (loading) {
    return <Spinner animation="border" className="spinner-color mt-5" />;
  } else
    return (
      <div className="container bg-white">
        <Row className="mb-4 justify-content-center">
          <Col sm={8} md={6} lg={4} className="align-self-center">
            <img
              ref={imgRef}
              onError={onImageError}
              src={pet.picture}
              alt="profil"
              className="pet-img rounded"
            />
          </Col>
          <Col
            md={6}
            lg={4}
            className="align-self-center pet-info mt-3 mt-md-0"
          >
            {showShereLocation && (
              <Button className="red-btn mb-3" onClick={shareLocation}>
                <FontAwesomeIcon icon={faMapLocationDot} className="me-2" />
                Share current location
              </Button>
            )}

            {pet.type && pet.type != "" && (
              <p>
                {" "}
                <span className="fw-bold">Name:</span> {pet.name}
              </p>
            )}
            {pet.type && pet.type != "" && (
              <p>
                {" "}
                <span className="fw-bold">Type:</span> {pet.type}
              </p>
            )}

            {pet.breed && pet.breed != "" && (
              <p>
                {" "}
                <span className="fw-bold">Bread:</span> {pet.breed}
              </p>
            )}

            {pet.characteristics && pet.characteristics.length > 0 && (
              <p>
                <span className="fw-bold">Characteristics:</span>

                {pet.characteristics.map((c) => c.name).join(", ")}
              </p>
            )}

            {pet.zipCode && pet.zipCode != "" && (
              <p>
                <span className="fw-bold">ZipCode:</span> {pet.zipCode}
              </p>
            )}
            {pet.description && pet.description != "" && (
              <p>
                <span className="fw-bold">Description:</span>
                {pet.description}
              </p>
            )}

            {pet.lostStatus && pet.disappearanceDate && (
              <p>
                <p className="fw-bold">Disappearance date:</p>

                {format(new Date(pet.disappearanceDate), "dd.MM.yyyy HH:mm")}
              </p>
            )}
            {isOwnerLooking && (
              <p>
                <span className="fw-bold">Download Pet QR code:</span>
                <DownloadQRButton
                  className="red-btn ms-1"
                  url={`https://findmyteddy.azurewebsites.net/pet/${pet.id}`}
                />
              </p>
            )}
          </Col>
          <Col
            md={7}
            lg={4}
            className="justify-content-center align-self-center mt-md-3"
          >
            <h2 className="nasloviopisa">
              {" "}
              Please contact my owner and help me get home!{" "}
            </h2>
            <div className="advantages">
              <div> First Name: {owner.firstName}</div>
              <div> Last Name: {owner.lastName} </div>
              <div> City: {owner.city}</div>
              <div> Street: {owner.street}</div>
              <div> Phone Number: {owner.phone} </div>
              <div> Email address: {owner.email} </div>
            </div>
          </Col>
        </Row>
        {pet.petLastLocations.length !== 0 && (
          <Row className="my-3">
            <Col md="8">
              <GoogleMaps
                lastLocations={pet.petLastLocations}
                positionMapLat={mapPosition.lat}
                positionMapLng={mapPosition.lng}
                selectedMapIndex={selectedMapIndex}
              />
            </Col>
            <Col md="4" className="overflow-auto" style={{ maxHeight: "60vh" }}>
              {pet.petLastLocations.map((location, index) => (
                <div
                  className="last-location border border-secondary"
                  onClick={() => {
                    setSelectedMapIndex(index);
                    setMapPosition({
                      lat: location.latitude,
                      lng: location.longitude,
                    });
                  }}
                >
                  <div className="">{index + 1}. Location</div>
                  <div className="">
                    Time located:{" "}
                    {format(
                      new Date(location.lastLocationDateTime),
                      "dd.MM.yyyy HH:mm"
                    )}
                  </div>
                  <div className="">
                    Coordinates:{" "}
                    {location.latitude + " - " + location.longitude}
                  </div>
                </div>
              ))}
            </Col>
          </Row>
        )}{" "}
        {pet.petLastLocations.length == 0 && (
          <h1 className="my-3"> No one has located me yet </h1>
        )}
      </div>
    );
};

export default Pet;
