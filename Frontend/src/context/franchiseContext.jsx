import { createContext, useContext, useState } from "react";
import { getFranchiseRequest, getFranchiseByIdRequest, createFranchiseRequest, updateFranchiseRequest, deleteFranchiseRequest } from "../api/apiFranchise.js";

const franchiseContext = createContext() 

export const useFranchise = () => {
    const context = useContext(franchiseContext)

    if(!context) {
        throw new Error('useFranchise must be used within a FranchiseProvider')
    }

    return context
}


export function FranchiseProvider({children}) {
    const [franchises, setFranchises] = useState([])


    // Functions to interact with the API, fetching and creating franchises
    const getFranchises = async () => {
        try {
            const response = await getFranchiseRequest()
            setFranchises(response.data.data)
        } catch (error) {
            console.error(error)
        }
    }

    // Function to get a single franchise by ID
    const getFranchise = async (id) => {
        try {
            const response = await getFranchiseByIdRequest(id)
            return response.data.data
        } catch (error) {
            console.error(error)
        }
    }

    // Function to create a new franchise and refresh the list
    const createFranchise = async (data) => {
            try {
                console.log('Data to be sent to API:', data);
                const response = await createFranchiseRequest(data)
                console.log('Franchise created successfully:', response.data);
                getFranchises() 
            } catch (error) {
                console.error('Error creating franchise:', error)
            }
    }

    // Function to update an existing franchise and refresh the list
    const updateFranchise = async (id, data) => {
        try {
            console.log('Data to be sent to API:', data);
            const response = await updateFranchiseRequest(id, data)
            console.log('Franchise updated successfully:', response.data);
            getFranchises() 
        } catch (error) {
            console.error('Error updating franchise:', error)
        } 
    } 

    // Function to delete a franchise and refresh the list
    const deleteFranchise = async (id) => {
        try {
            await deleteFranchiseRequest(id)
            console.log('Franchise deleted successfully');
            getFranchises()
        } catch (error) {
            console.error('Error deleting franchise:', error)
        }
    } 


    return (
        <franchiseContext.Provider value={{
                franchises,
                getFranchises,
                createFranchise,
                getFranchise,
                updateFranchise,
                deleteFranchise,
        }}>
            {children}
        </franchiseContext.Provider>
    )
}
