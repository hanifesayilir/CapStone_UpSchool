import axios from "axios";
import LocalJwt from "../../../models/LocalJwt"

const BASE_URL = import.meta.env.VITE_API_URL


const api = axios.create({
    baseURL: BASE_URL
});


api.interceptors.request.use((config) =>{

    const jwtJson  = localStorage.getItem("userTokenStorage");
    if(jwtJson) {

        const localJwt: LocalJwt = JSON.parse(jwtJson);

        config.headers["Authorization"] = `Bearer ${localJwt.accessToken}`
    }

    return config;
})

export default api;
