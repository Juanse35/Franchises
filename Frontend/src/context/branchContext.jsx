import Swal from 'sweetalert2'
import { createContext, useContext, useState } from "react";
import { 
    getBranchRequest, 
    getBranchByIdFranchiseRequest, 
    getBranchByIdRequest, 
    createBranchRequest, 
    updateBranchRequest, 
    deleteBranchRequest 
} from "../api/apiBranch.js";

const branchContext = createContext() 

export const useBranch = () => {
    const context = useContext(branchContext)
    if (!context) throw new Error('useBranch must be used within a BranchProvider')
    return context
}

export function BranchProvider({children}) {
    const [branches, setBranches] = useState([])

    // Fetch all branches from API
    const getBranches = async () => {
        try {
            const response = await getBranchRequest()
            setBranches(response.data.data)
        } catch (error) {
            console.error(error)
        }
    }

    // Fetch branches belonging to a specific franchise
    const getBranchByIdFranchise = async (id) => {
        try {
            const response = await getBranchByIdFranchiseRequest(id)
            setBranches(response.data.data)
        } catch (error) {
            console.error(error)
        }
    }

    // Fetch a single branch by ID
    const getBranchById = async (id) => {
        try {
            const response = await getBranchByIdRequest(id)
            return response.data.data
        } catch (error) {
            console.error(error)
        }
    }

    // Create a new branch with success/error alerts
    const createBranch = async (data) => {
        try {
            await createBranchRequest(data)

            // Show success alert
            Swal.fire({
                title: 'Success!',
                text: 'The branch was created successfully.',
                icon: 'success',
                showConfirmButton: false,
                timer: 2000,
                timerProgressBar: true
            });

            getBranches(); // Refresh list
        } catch (error) {
            console.error("Error creating branch:", error);

            // Show error alert
            Swal.fire({
                title: 'Error!',
                text: 'Failed to create the branch. Please try again.',
                icon: 'error'
            });
        }
    }

    // Update an existing branch with alerts
    const updateBranch = async (id, data) => {
        try {
            await updateBranchRequest(id, data)

            // Show success alert
            Swal.fire({
                title: 'Success!',
                text: 'The branch was updated successfully.',
                icon: 'success',
                showConfirmButton: false,
                timer: 2000,
                timerProgressBar: true
            });

            getBranches(); // Refresh list
        } catch (error) {
            console.error("Error updating branch:", error);

            // Show error alert
            Swal.fire({
                title: 'Error!',
                text: 'Failed to update the branch. Please try again.',
                icon: 'error'
            });
        }
    }

    // Delete a branch with confirmation and alerts
    const deleteBranch = async (id) => {
        try {
            // Ask for confirmation before deleting
            const result = await Swal.fire({
                title: 'Are you sure?',
                text: 'Deleting this Branch will permanently remove all associated products. This action cannot be undone.',
                icon: 'info',
                showCancelButton: true,
                confirmButtonText: 'Yes, delete it!',
                cancelButtonText: 'Cancel',
                confirmButtonColor: '#37297b', 
                cancelButtonColor: '#cc1515'   
            });

            if (result.isConfirmed) {
                await deleteBranchRequest(id)

                // Show success alert
                Swal.fire({
                    title: 'Deleted!',
                    text: 'The branch has been deleted.',
                    icon: 'success',
                    showConfirmButton: false,
                    timer: 2000,
                    timerProgressBar: true
                });

                getBranches(); // Refresh list
            }
        } catch (error) {
            console.error("Error deleting branch:", error);

            // Show error alert
            Swal.fire({
                title: 'Error!',
                text: 'Failed to delete the branch. Please try again.',
                icon: 'error',
                showConfirmButton: false,
                timer: 2000,
                timerProgressBar: true
            });
        }
    }

    return (
        <branchContext.Provider value={{
            branches,
            getBranches,
            getBranchByIdFranchise,
            getBranchById,
            createBranch,
            updateBranch,
            deleteBranch,
        }}>
            {children}
        </branchContext.Provider>
    )
}