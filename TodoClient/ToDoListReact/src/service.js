
import axios from 'axios';

const apiUrl = "http://localhost:5212" 

axios.defaults.baseURL = apiUrl;

axios.interceptors.response.use(
    response => response,
    error => {
        console.error("API Error:", error.response ? error.response.data : error.message);
        return Promise.reject(error);
    }
);

export default {
    getTasks: async () => {
        const result = await axios.get(`/tasks`);    
        return result.data;
    },

    addTask: async (name) => {
        const result = await axios.post(`/tasks`, { name: name, isComplete: false });
        return result.data;
    },

    setCompleted: async (id, isComplete) => {
        const result = await axios.put(`/tasks/${id}`, { isComplete: isComplete });
        return result.data;
    },

    deleteTask: async (id) => {
        const result = await axios.delete(`/tasks/${id}`);
        return result.data;
    }
};
