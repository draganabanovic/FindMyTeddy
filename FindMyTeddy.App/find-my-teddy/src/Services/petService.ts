import { IAccessToken } from "../models/accessToken";
import { ICharacteristicModel } from "../models/characteristicModels";
import { ICreatePetModel, IPetModel } from "../models/petModels";
import { API } from "./axios";
import jwt_decode from "jwt-decode";
// @ts-ignore
import { NotificationManager } from "react-notifications";
import { accountUtils } from "../utils/AccountUtils";

export const petService = {
  getPets,
  getPetById,
  getPetsByOwnerId,
  getAllLost,
  getLostFromDate,
  addPet,
  updatePet,
  updatePetStatus,
  deletePet,
  getLostForZipCode,
};

async function getPets() {
  return await API.get<IPetModel[]>("/Pet")
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

async function getPetById(petId: string) {
  return await API.get<IPetModel>(`/Pet/${petId}`)
    .then((res) => {
      return res.data;
    })
    .catch((error) => {
      if (error.response) {
        console.log(error.response);
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

async function getPetsByOwnerId(ownerId: string) {
  return await API.get<IPetModel[]>(`/Pet/owner/${ownerId}`)
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

async function getAllLost() {
  return await API.get<IPetModel[]>(`/Pet/lost`)
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

async function getLostFromDate(fromDate: Date) {
  return await API.get<IPetModel[]>(`/Pet/lost/fromdate/${fromDate}`)
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

async function getLostForZipCode(zipCode: string) {
  return await API.get<IPetModel[]>(`/Pet/lost/zipcode/${zipCode}`)
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

async function addPet(pet: ICreatePetModel, pictureFile: File) {
  const userId = accountUtils.getUserId();
  pet.ownerId = userId;

  const formData = new FormData();

  formData.append("PetData", JSON.stringify(pet));
  formData.append("PictureFile", pictureFile);

  return await API.post<IPetModel>(`/Pet`, formData)
    .then((res) => {
      NotificationManager.success("Pet is successfully created", "", 2000);
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

async function updatePetStatus(
  petId: string,
  newStatus: boolean
): Promise<IPetModel | null> {
  const params = {
    id: petId,
    lostStatus: newStatus,
  };
  const config = {
    headers: {
      "Content-Type": "application/json",
    },
  };

  return await API.put<IPetModel>(`/Pet/status`, params, config)
    .then((res) => {
      NotificationManager.success(
        "Pet status is successfully changed",
        "",
        2000
      );
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

async function updatePet(pet: IPetModel, pictureFile: File) {
  const userId = accountUtils.getUserId();
  pet.ownerId = userId;

  const fromData = new FormData();
  fromData.append("PetData", JSON.stringify(pet));
  fromData.append("pictureFile", pictureFile);

  return await API.put<IPetModel>(`/Pet`, fromData)
    .then((res) => {
      NotificationManager.success("Pet is successfully updated", "", 2000);
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

async function deletePet(petId: string) {
  return await API.delete(`/Pet/${petId}`)
    .then((res) => {
      NotificationManager.success("Pet is successfully deleted", "", 2000);
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
