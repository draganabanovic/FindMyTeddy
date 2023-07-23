import { ILastLocationModel } from "../models/petlastlocationModels";
import { API } from "./axios";
// @ts-ignore
import { NotificationManager } from "react-notifications";

export const PetLastLocationService = {
  getLocationsByPetId,
  addLocation,
  deactivatePreviouslocation,
};

async function getLocationsByPetId(petId: string) {
  return await API.get<ILastLocationModel[]>(`/PetLastLocation/pet/${petId}`)
    .then((res) => {
      return res.data;
    })
    .catch((error) => {
      if (error.response) {
        NotificationManager.error(
          error.response.data.errorMessage,
          "Error",
          1500
        );
        return null;
      } else if (error.request) {
        NotificationManager.error(
          "An error occurred while connecting to the server",
          "Error",
          1500
        );
        return null;
      } else {
        NotificationManager.error("Something went wrong", "Error", 1500);
        return null;
      }
    });
}

async function addLocation(location: ILastLocationModel) {
  const config = {
    headers: {
      "Content-Type": "application/json",
    },
  };

  return await API.post<ILastLocationModel>(
    `/PetLastLocation`,
    location,
    config
  )
    .then((res) => {
      return res.data;
    })
    .catch((error) => {
      if (error.response) {
        NotificationManager.error(
          error.response.data.errorMessage,
          "Error",
          1500
        );
        return null;
      } else if (error.request) {
        NotificationManager.error(
          "An error occurred while connecting to the server",
          "Error",
          1500
        );
        return null;
      } else {
        NotificationManager.error("Something went wrong", "Error", 1500);
        return null;
      }
    });
}

async function deactivatePreviouslocation(petId: string) {
  return await API.put(`/PetLastLocation/deactivate-previous/${petId}`)
    .then((res) => {
      return res.data;
    })
    .catch((error) => {
      if (error.response) {
        NotificationManager.error(
          error.response.data.errorMessage,
          "Error",
          1500
        );
        return null;
      } else if (error.request) {
        NotificationManager.error(
          "An error occurred while connecting to the server",
          "Error",
          1500
        );
        return null;
      } else {
        NotificationManager.error("Something went wrong", "Error", 1500);
        return null;
      }
    });
}
