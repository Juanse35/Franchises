import axios from "axios"
const API = 'https://localhost:5001/'

export const getProductRequest = () => axios.get(`${API}getProducts`)
export const getProductByIdRequest = (id) => axios.get(`${API}getProduct/${id}`)
export const createProductRequest = (data) => axios.post(`${API}createProduct`, data)
export const updateProductRequest = (id, data) => axios.put(`${API}updateProduct/${id}`, data)
export const deleteProductRequest = (id) => axios.delete(`${API}deleteProduct/${id}`)