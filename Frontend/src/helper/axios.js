import axios from "axios";
import config from "./config";
import useUserStore from '../store/userStore';

const instance = axios.create({
    baseURL: config.get("BASE_API_URL"),
    timeout: 10000,
    headers: {
      "Content-Type": "application/json",
      "Accept": "application/json",
    },
  });
  
  instance.interceptors.request.use((req) => {
    const { userId } = useUserStore.getState();
    if (userId) {
      req.headers["x-userid"] = userId;
    } else {
      delete req.headers["x-userid"];
    }
    return req;
  });

export default instance;