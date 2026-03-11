import axios from "axios"
const API = 'https://localhost:5001/'

export const getBranchRequest = () => axios.get(`${API}getBranches`)
export const getBranchByIdFranchiseRequest = (id) => axios.get(`${API}getBranchByIdFranchise/${id}`)
export const getBranchByIdRequest = (id) => axios.get(`${API}getBranchById/${id}`)
export const createBranchRequest = (data) => axios.post(`${API}createBranch`, data)
export const updateBranchRequest = (id, data) => axios.put(`${API}updateBranch/${id}`, data)
export const deleteBranchRequest = (id) => axios.delete(`${API}deleteBranch/${id}`)