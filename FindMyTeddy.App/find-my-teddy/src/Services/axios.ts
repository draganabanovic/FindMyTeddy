import axios from "axios";

const API = axios.create({
  baseURL: "https://localhost:7199",
});

API.interceptors.request.use(
  (config) => {
    const access_token = localStorage.getItem("access_token");
    if (access_token) {
      config.headers.authorization = access_token;
    }
    return config;
  },
  (error) => Promise.reject(error)
);

export { API };
