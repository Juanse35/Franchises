import { createContext, useContext, useState } from "react";

import { getBranchRequest, getBranchByIdFranchiseRequest } from "../api/apiBranch.js";

const branchContext = createContext() 

export const useBranch = () => {
    const context = useContext(branchContext)

    if(!context) {
        throw new Error('useBranch must be used within a BranchProvider')
    }

    return context
}

export function BranchProvider({children}) {
    const [branches, setBranches] = useState([])

    const getBranches = async () => {
        try {
            const response = await getBranchRequest()
            setBranches(response.data.data)
            console.log("Branches fetched successfully:", response.data.data);
        }
        catch (error) {
            console.error(error)
        }
    }


    const getBranchByIdFranchise = async (id) => {
        try{
            const response = await getBranchByIdFranchiseRequest(id)
            setBranches(response.data.data)
            console.log("Branches fetched successfully:", response.data.data);
        }
        catch (error) {
            console.error(error)
        }
    }

    return (
        <branchContext.Provider value={{
                branches,
                getBranches,
                getBranchByIdFranchise,

        }}>
            {children}
        </branchContext.Provider>
    )
}