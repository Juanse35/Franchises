import { useForm } from 'react-hook-form';
import { useEffect } from 'react';

import { useBranch } from '../../../context/branchContext.jsx';
import { useFranchise } from '../../../context/franchiseContext.jsx';

import './formBranch.css';

function FormBranch({ closeModal, id = null, franchiseId = null, franchiseName = null }) {

    // React Hook Form setup
    const { register, handleSubmit, setValue } = useForm();

    // Branch actions from context
    const { createBranch, updateBranch, getBranchById } = useBranch();

    // Franchise actions from context
    const { franchises, getFranchises } = useFranchise();

    // Load franchises for the select dropdown
    useEffect(() => {
        const loadFranchises = async () => {
            if (!franchises || franchises.length === 0) {
                await getFranchises();
            }
        };
        loadFranchises();
    }, []);

    // Load branch data when editing
    useEffect(() => {
        const loadBranch = async () => {
            if (id) {
                // Fetch branch by ID
                const branch = await getBranchById(id);
                if (branch) {
                    setValue("Name_branch", branch.name_branch);
                    setValue("franchiseId", branch.franchiseId);
                }
            } else if (franchiseId) {
                // Set franchiseId when creating branch from a specific franchise
                setValue("franchiseId", franchiseId);
            }
        };

        loadBranch();
    }, [id]);

    // Submit form (create or update branch)
    const onSubmit = async (data) => {

        const branchData = {
            Name_branch: data.Name_branch,
            franchiseId: franchiseId ? franchiseId : data.franchiseId
        };

        console.log(branchData);

        if (id) {
            // Update existing branch
            await updateBranch(id, branchData);
        } else {
            // Create new branch
            await createBranch(branchData);
        }

        // Close modal after submit
        closeModal();
    };

    return (
        <div className="container-form-branch">

            {/* Form title */}
            <h2 className='formTitle'>
                {id ? "Edit Branch" : "Register Branch"}
            </h2>

            <form onSubmit={handleSubmit(onSubmit)}>

                <div className="container-inputs">

                    {/* Branch Name input */}
                    <label>Branch Name:</label><br />
                    <input
                        type="text"
                        placeholder='Branch Name'
                        {...register("Name_branch", { required: true })}
                    />

                    <br /><br />

                    {/* Franchise selection */}
                    <label>Franchise:</label><br />

                    {franchiseName ? (
                        <>
                            {/* Display franchise name when provided */}
                            <input
                                type="text"
                                value={franchiseName}
                                disabled
                            />
                            <input
                                type="hidden"
                                value={franchiseId}
                                {...register("franchiseId")}
                            />
                        </>
                    ) : (
                        /* Dropdown to select franchise */
                        <select className='select-form' {...register("franchiseId", { required: true })}>
                            <option value="">Select Franchise</option>
                            {franchises?.map((franchise) => (
                                <option
                                    key={franchise.id}
                                    value={franchise.id}
                                >
                                    {franchise.name}
                                </option>
                            ))}
                        </select>
                    )}

                </div>

                {/* Form buttons */}
                <div className="container-btn">

                    {/* Submit button */}
                    <button
                        type='submit'
                        className="btn-register"
                    >
                        <i className="fa-solid fa-circle-check" />
                        {id ? " Update" : " Register"}
                    </button>

                    {/* Cancel button */}
                    <button
                        type="button"
                        className="btn-cancel"
                        onClick={closeModal}
                    >
                        <i className="fa-solid fa-circle-xmark" /> Cancel
                    </button>

                </div>

            </form>

        </div>
    );
}

export default FormBranch;