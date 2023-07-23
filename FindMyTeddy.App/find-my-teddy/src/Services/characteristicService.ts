import { ICharacteristicModel } from "../models/characteristicModels";
import { API } from "./axios";
// @ts-ignore
import { NotificationManager } from "react-notifications";

export const characteristicService = {
  getCharacteristics,
  getByPetId,
  createCharacteristic,
  deleteCharacteristic,
};

async function getCharacteristics() {
  return await API.get<ICharacteristicModel[]>("/Characteristic")
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

async function getByPetId(petId: string) {
  return await API.get<ICharacteristicModel[]>(`Characteristic/Pet/${petId}`)
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

async function createCharacteristic(characteristic: ICharacteristicModel) {
  const config = {
    headers: {
      "Content-Type": "application/json",
    },
  };

  return await API.post<ICharacteristicModel>(
    `/Characteristic`,
    characteristic,
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

async function deleteCharacteristic(characteristicId: string) {
  return await API.delete(`/Characteristic/${characteristicId}`)
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
