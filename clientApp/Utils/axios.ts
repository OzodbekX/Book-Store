import axios from 'axios';

const axiosClient = axios.create({
    headers: {
        'Accept': 'application/json',
        'Content-Type': 'application/json',
    },
    timeout: 10000

});

const getSession = () => {
    if (typeof window !== "undefined") {
        return localStorage.getItem("accessToken")
    }
    return ""
}
axiosClient.interceptors.request.use(async config => {
    const session = getSession();
    if (config?.headers && session) config.headers["Authorization"] = `Bearer ${session}`;
    return config;
});
axiosClient.defaults.baseURL = 'https://localhost:8000/api/';


//All request will wait 2 seconds before timeout
axiosClient.defaults.timeout = 10000;

// axiosClient.defaults.withCredentials = true;
export default axiosClient