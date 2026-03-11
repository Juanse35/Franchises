import { useForm } from 'react-hook-form';
import { useEffect } from 'react';

import { useBranch } from '../../../context/branchContext.jsx';
import { useFranchise } from '../../../context/franchiseContext.jsx';

import './formBranch.css';

function FormBranch({ closeModal, id = null, franchiseId = null, franchiseName = null }) {

    const { register, handleSubmit, setValue } = useForm();

    const { createBranch, updateBranch, getBranchById } = useBranch();

    const { franchises, getFranchises } = useFranchise();

    useEffect(() => {
        const loadFranchises = async () => {

            if (!franchises || franchises.length === 0) {
                await getFranchises();
            }

        };
        loadFranchises();

    }, []);

    useEffect(() => {

        const loadBranch = async () => {
            if (id) {
                const branch = await getBranchById(id);
                if (branch) {
                    setValue("Name_branch", branch.name_branch);
                    setValue("franchiseId", branch.franchiseId);
                }
            } else if (franchiseId) {

                setValue("franchiseId", franchiseId);

            }
        };

        loadBranch();
    }, [id]);

    const onSubmit = async (data) => {

        const branchData = {
            Name_branch: data.Name_branch,
            franchiseId: franchiseId ? franchiseId : data.franchiseId
        };

        console.log(branchData);

        if (id) {
            await updateBranch(id, branchData);
        } else {
            await createBranch(branchData);
        }

        closeModal();
    };


    return (
        <div className="container-form-branch">

            <h2 className='formTitle'>
                {id ? "Edit Branch" : "Register Branch"}
            </h2>

            <form onSubmit={handleSubmit(onSubmit)}>

                <div className="container-inputs">

                    <label>Branch Name:</label><br />

                    <input
                        type="text"
                        placeholder='Branch Name'
                        {...register("Name_branch", { required: true })}
                    />

                    <br /><br />

                    <label>Franchise:</label><br />

                    {franchiseName ? (

                        <>
                    
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

                        <select className='select-form' {...register("franchiseId", { required: true })}>

                            <option value="">
                                Select Franchise
                            </option>

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

                <div className="container-btn">

                    <button
                        type='submit'
                        className="btn-register"
                    >
                        <i className="fa-solid fa-circle-check" />
                        {id ? " Update" : " Register"}
                    </button>

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