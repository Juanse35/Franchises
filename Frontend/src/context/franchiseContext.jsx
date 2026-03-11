import Swal from 'sweetalert2'
import { createContext, useContext, useState } from "react";
import { 
    getFranchiseRequest, 
    getFranchiseByIdRequest, 
    createFranchiseRequest, 
    updateFranchiseRequest, 
    deleteFranchiseRequest 
} from "../api/apiFranchise.js";

const franchiseContext = createContext() 

export const useFranchise = () => {
    const context = useContext(franchiseContext)
    if(!context) throw new Error('useFranchise must be used within a FranchiseProvider')
    return context
}

export function FranchiseProvider({children}) {
    const [franchises, setFranchises] = useState([])

    // Fetch all franchises from API
    const getFranchises = async () => {
        try {
            const response = await getFranchiseRequest()
            setFranchises(response.data.data)
        } catch (error) {
            console.error(error)
        }
    }

    // Fetch a single franchise by ID
    const getFranchise = async (id) => {
        try {
            const response = await getFranchiseByIdRequest(id)
            return response.data.data
        } catch (error) {
            console.error(error)
        }
    }

    // Create a new franchise and refresh list
    const createFranchise = async (data) => {
        try {
            console.log('Data to be sent to API:', data);
            await createFranchiseRequest(data);

            // Show success alert
            Swal.fire({
                title: 'Success!',
                text: 'The franchise was created successfully.',
                icon: 'success',
                showConfirmButton: false, 
                timer: 2000,              
                timerProgressBar: true    
            });

            getFranchises(); // Refresh list
        } catch (error) {
            console.error('Error creating franchise:', error);

            // Show error alert
            Swal.fire({
                title: "Error!",
                text: "Failed to create the franchise. Please try again.",
                icon: "error",
            });
        }
    };

    // Update a franchise and refresh list
    const updateFranchise = async (id, data) => {
        try {
            await updateFranchiseRequest(id, data);

            // Show success alert
            Swal.fire({
                title: 'Success!',
                text: 'The franchise was updated successfully.',
                icon: 'success',
                showConfirmButton: false, 
                timer: 2000,              
                timerProgressBar: true    
            });

            getFranchises(); // Refresh list
        } catch (error) {
            console.error('Error updating franchise:', error);
            Swal.fire({
                title: "Error!",
                text: "Failed to update the franchise. Please try again.",
                icon: "error",
            });
        } 
    } 

    // Delete a franchise with confirmation
    const deleteFranchise = async (id) => {
        try {
            // Confirm before deleting
            const result = await Swal.fire({
                title: 'Are you sure?',
                text: 'Deleting this franchise will permanently remove all associated branches and products. This action cannot be undone.',
                icon: 'info',
                showCancelButton: true,
                confirmButtonText: 'Yes, delete everything',
                cancelButtonText: 'Cancel',
                confirmButtonColor: '#37297b',
                cancelButtonColor: '#cc1515'
            });

            if (result.isConfirmed) {
                await deleteFranchiseRequest(id);

                // Show success alert
                Swal.fire({
                    title: 'Deleted!',
                    text: 'The franchise has been deleted.',
                    icon: 'success',
                    showConfirmButton: false,
                    timer: 2000,
                    timerProgressBar: true
                });

                getFranchises(); // Refresh list
            }

        } catch (error) {
            console.error('Error deleting franchise:', error);

            // Show error alert
            Swal.fire({
                title: 'Error!',
                text: 'Failed to delete the franchise. Please try again.',
                icon: 'error',
                showConfirmButton: false,
                timer: 2000,
                timerProgressBar: true
            });
        }
    };

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