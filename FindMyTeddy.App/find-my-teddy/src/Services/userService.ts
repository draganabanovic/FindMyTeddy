import { IJwtAuthResultViewModel } from "../models/JwtAuthResultViewModel";
import { ICreateUserModel, IUserModel } from "../models/userModels";
import { API } from "./axios";
// @ts-ignore
import { NotificationManager } from "react-notifications";

export const userService = {
  getUserById,
  getUsers,
  deleteUser,
  getUserByPetId,
  register,
  login,
  update,
};

async function update(user: ICreateUserModel, pictureFile: File) {
  const formData = new FormData();
  formData.append("UserData", JSON.stringify(user));
  formData.append("PictureFile", pictureFile);

  return await API.put<IUserModel>(`/User`, formData)
    .then((res) => {
      NotificationManager.success("Account updated", "", 2000);
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

async function login(email: string, password: string) {
  const params = {
    email: email,
    password: password,
  };
  const config = {
    headers: {
      "Content-Type": "application/json",
    },
  };
  return await API.post<IJwtAuthResultViewModel>(`/User/login`, params, config)
    .then((res) => {
      localStorage.setItem("access_token", "Bearer " + res.data.accessToken);
      localStorage.setItem("refresh_token", res.data.refreshToken);
      NotificationManager.success("You are logged in", "", 2000);
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

async function register(user: ICreateUserModel, pictureFile: File) {
  const formData = new FormData();

  formData.append("UserData", JSON.stringify(user));
  formData.append("PictureFile", pictureFile);

  return await API.post<IUserModel>(`/User`, formData)
    .then((res) => {
      NotificationManager.success("Account registered", "", 2000);
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

async function getUserById(userId: string) {
  return await API.get<IUserModel>(`/User/${userId}`)
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

async function getUsers() {
  return await API.get<IUserModel[]>("/User")
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

async function getUserByPetId(petId: string) {
  return await API.get<IUserModel>(`/User/petId/${petId}`)
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

async function addUser(user: ICreateUserModel) {
  const config = {
    headers: {
      "Content-Type": "application/json",
    },
  };

  return await API.post<IUserModel>(`/User`, user, config)
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

async function deleteUser(userId: string) {
  return await API.delete(`/user/${userId}`)
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
