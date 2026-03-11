import Swal from 'sweetalert2'
import { createContext, useContext, useState } from "react";
import { 
    getProductRequest, 
    createProductRequest, 
    updateProductRequest, 
    getProductByIdRequest, 
    deleteProductRequest
} from "../api/apiProduct"; 

const productContext = createContext() 

export const useProduct = () => {
    const context = useContext(productContext)
    if (!context) throw new Error('useProduct must be used within a ProductProvider')
    return context
}

export function ProductProvider({children}) {
    const [products, setProducts] = useState([])

    // Fetch all products from API
    const getProducts = async () => {
        try {
            const response = await getProductRequest()
            setProducts(response.data.data)
        } catch (error) {
            console.error("Error fetching products:", error)
        }
    }

    // Fetch single product by ID
    const getProduct = async (id) => {
        try {
            const response = await getProductByIdRequest(id)
            return response.data.data
        } catch (error) {
            console.error("Error fetching product:", error)
        }
    }

    // Create new product with success/error alerts
    const createProduct = async (data) => {
        try {
            await createProductRequest(data)

            // Show success alert
            Swal.fire({
                title: 'Success!',
                text: 'The product was created successfully.',
                icon: 'success',
                showConfirmButton: false,
                timer: 2000,
                timerProgressBar: true
            });

            getProducts() // Refresh list
        } catch (error) {
            console.error("Error creating product:", error)

            // Show error alert
            Swal.fire({
                title: 'Error!',
                text: 'Failed to create the product. Please try again.',
                icon: 'error'
            });
        }
    }

    // Update existing product with alerts
    const updateProduct = async (id, data) => {
        try {
            await updateProductRequest(id, data)

            // Show success alert
            Swal.fire({
                title: 'Success!',
                text: 'The product was updated successfully.',
                icon: 'success',
                showConfirmButton: false,
                timer: 2000,
                timerProgressBar: true
            });

            getProducts() // Refresh list
        } catch (error) {
            console.error("Error updating product:", error)

            // Show error alert
            Swal.fire({
                title: 'Error!',
                text: 'Failed to update the product. Please try again.',
                icon: 'error'
            });
        }
    }

    // Delete product with confirmation and alerts
    const deleteProduct = async (id) => {
        try {
            // Confirm deletion
            const result = await Swal.fire({
                title: 'Are you sure?',
                text: 'Do you really want to delete this product?',
                icon: 'info',
                showCancelButton: true,
                confirmButtonText: 'Yes, delete it!',
                cancelButtonText: 'Cancel',
                confirmButtonColor: '#37297b', 
                cancelButtonColor: '#cc1515'   
            });

            if (result.isConfirmed) {
                await deleteProductRequest(id)

                // Show success alert
                Swal.fire({
                    title: 'Deleted!',
                    text: 'The product has been deleted.',
                    icon: 'success',
                    showConfirmButton: false,
                    timer: 2000,
                    timerProgressBar: true
                });

                getProducts() 
            }
        } catch (error) {
            console.error("Error deleting product:", error)

            // Show error alert
            Swal.fire({
                title: 'Error!',
                text: 'Failed to delete the product. Please try again.',
                icon: 'error',
                showConfirmButton: false,
                timer: 2000,
                timerProgressBar: true
            });
        }
    }

    return (
        <productContext.Provider value={{
            products,
            getProducts,
            getProduct,
            createProduct,
            updateProduct,
            deleteProduct,
        }}>
            {children}
        </productContext.Provider>
    )
}