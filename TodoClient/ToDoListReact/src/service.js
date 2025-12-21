// import axios from 'axios';

// const apiUrl = "http://localhost:5212"

// export default {
//   getTasks: async () => {
//     const result = await axios.get(`${apiUrl}/items`)    
//     return result.data;
//   },

//   addTask: async(name)=>{
//     console.log('addTask', name)
//     //TODO
//     return {};
//   },

//   setCompleted: async(id, isComplete)=>{
//     console.log('setCompleted', {id, isComplete})
//     //TODO
//     return {};
//   },

//   deleteTask:async()=>{
//     console.log('deleteTask')
//   }
// };
import axios from 'axios';

// 1. הגדרת כתובת השרת - ודאי שהפורט (5xxx) תואם למה שמופיע לך ב-C#
const apiUrl = "http://localhost:5212" 

// הגדרת ברירת מחדל כך שלא נצטרך לכתוב את כל הכתובת בכל פעם
axios.defaults.baseURL = apiUrl;

// הוספת interceptor כדי לתפוס שגיאות ולראות אותן בלוג (עוזר מאוד בדיבאגינג)
axios.interceptors.response.use(
    response => response,
    error => {
        console.error("API Error:", error.response ? error.response.data : error.message);
        return Promise.reject(error);
    }
);

export default {
    // שליפת כל המשימות
    getTasks: async () => {
        const result = await axios.get(`/tasks`);    
        return result.data;
    },

    // הוספת משימה חדשה
    addTask: async (name) => {
        // שימי לב: ב-POST אנחנו שולחים אובייקט עם שם המשימה וסטטוס התחלתי
        const result = await axios.post(`/tasks`, { name: name, isComplete: false });
        return result.data;
    },

    // עדכון סטטוס משימה (נניח שמישהו לחץ על ה-V)
    setCompleted: async (id, isComplete) => {
        const result = await axios.put(`/tasks/${id}`, { isComplete: isComplete });
        return result.data;
    },

    // מחיקת משימה
    deleteTask: async (id) => {
        const result = await axios.delete(`/tasks/${id}`);
        return result.data;
    }
};
