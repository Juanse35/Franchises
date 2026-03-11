import { useForm } from 'react-hook-form';
import { useEffect } from 'react';

import { useBranch } from '../../../context/branchContext.jsx';
import { useProduct } from '../../../context/productContext.jsx';


function FormProduct({ closeModal, productId = null, branchId = null, branchName = null }) {

    const { register, handleSubmit, setValue } = useForm();

    const { branches, getBranches } = useBranch();
    const { createProduct, updateProduct, getProduct } = useProduct();

    // Cargar ramas disponibles
    useEffect(() => {
        const loadBranches = async () => {
            if (!branches || branches.length === 0) {
                await getBranches();
            }
        };
        loadBranches();
    }, []);

    // Cargar producto si hay productId
    useEffect(() => {
        const loadProduct = async () => {
            if (productId) {
                const product = await getProduct(productId);
                if (product) {
                    setValue("name_product", product.name_product);
                    setValue("stock", product.stock);
                    setValue("branchId", product.branchId);
                }
            } else if (branchId) {
                setValue("branchId", branchId);
            }
        };
        loadProduct();
    }, [productId]);

    const onSubmit = async (data) => {
        const productData = {
            name_product: data.name_product,
            stock: data.stock,
            branchId: branchId ? branchId : data.branchId
        };

        if (productId) {
            await updateProduct(productId, productData);
        } else {
            await createProduct(productData);
        }

        closeModal();
    };

    return (
        <div className="container-form-product">
            <h2 className='formTitle'>
                {productId ? "Edit Product" : "Register Product"}
            </h2>

            <form onSubmit={handleSubmit(onSubmit)}>
                <div className="container-inputs">

                    {/* Nombre del Producto */}
                    <label>Product Name:</label><br />
                    <input
                        type="text"
                        placeholder='Product Name'
                        {...register("name_product", { required: true })}
                    />
                    <br /><br />

                    {/* Selección de Branch */}
                    <label>Branch:</label><br />
                    {branchName ? (
                        <>
                            <input
                                type="text"
                                value={branchName}
                                disabled
                            />
                            <input
                                type="hidden"
                                value={branchId}
                                {...register("branchId")}
                            />
                        </>
                    ) : (
                        <select
                            className='select-form'
                            {...register("branchId", { required: true })}
                        >
                            <option value="">Select Branch</option>
                            {branches?.map(branch => (
                                <option key={branch.id_branch} value={branch.id_branch}>
                                    {branch.name_branch}
                                </option>
                            ))}
                        </select>
                    )}
                    <br /><br />

                    {/* Stock */}
                    <label>Stock:</label><br />
                    <input
                        type="number"
                        min="0"
                        step="1"
                        placeholder="Stock"
                        {...register("stock", { required: true, min: 0 })}
                    />

                </div>

                <div className="container-btn">
                    <button type='submit' className="btn-register">
                        <i className="fa-solid fa-circle-check" />
                        {productId ? " Update" : " Register"}
                    </button>

                    <button type="button" className="btn-cancel" onClick={closeModal}>
                        <i className="fa-solid fa-circle-xmark" /> Cancel
                    </button>
                </div>
            </form>
        </div>
    );
}

export default FormProduct