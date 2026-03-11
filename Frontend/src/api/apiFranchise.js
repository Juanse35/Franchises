import axios from "axios"
const API = 'https://localhost:5001/'

export const getFranchiseRequest = () => axios.get(`${API}getFranchises`)
export const getFranchiseByIdRequest = (id) => axios.get(`${API}getFranchise/${id}`)
export const createFranchiseRequest = (data) => axios.post(`${API}createFranchise`, data)
export const updateFranchiseRequest = (id, data) => axios.put(`${API}updateFranchise/${id}`, data)
export const deleteFranchiseRequest = (id) => axios.delete(`${API}deleteFranchise/${id}`)