import axios from 'axios';
const baseUrl = 'http://localhost:56374/api';

const getAllTrainers = () => new Promise((resolve, reject) => {
    axios
      .get(`${baseUrl}/trainers`)
      .then(res => resolve(res.data))
      .catch(err => reject(err));
});

export default {getAllTrainers};
