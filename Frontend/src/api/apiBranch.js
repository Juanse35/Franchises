import axios from "axios"
const API = 'https://localhost:5001/'

export const getBranchRequest = () => axios.get(`${API}getBranches`)
export const getBranchByIdFranchiseRequest = (id) => axios.get(`${API}getBranchByIdFranchise/${id}`)