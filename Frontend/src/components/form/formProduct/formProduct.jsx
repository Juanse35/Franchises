import { useForm } from 'react-hook-form';
import { useEffect } from 'react';

import { useBranch } from '../../../context/branchContext.jsx';
import { useProduct } from '../../../context/productContext.jsx';

function FormProduct({ closeModal, productId = null, branchId = null, branchName = null }) {

    // React Hook Form setup
    const { register, handleSubmit, setValue } = useForm();

    // Contexts for branches and products
    const { branches, getBranches } = useBranch();
    const { createProduct, updateProduct, getProduct } = useProduct();

    // Load available branches for select dropdown
    useEffect(() => {
        const loadBranches = async () => {
            if (!branches || branches.length === 0) {
                await getBranches();
            }
        };
        loadBranches();
    }, []);

    // Load product data if editing
    useEffect(() => {
        const loadProduct = async () => {
            if (productId) {
                // Fetch product by ID
                const product = await getProduct(productId);
                if (product) {
                    setValue("name_product", product.name_product);
                    setValue("stock", product.stock);
                    setValue("branchId", product.branchId);
                }
            } else if (branchId) {
                // Pre-fill branchId if provided
                setValue("branchId", branchId);
            }
        };
        loadProduct();
    }, [productId]);

    // Submit form (create or update product)
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

        // Close modal after submit
        closeModal();
    };

    return (
        <div className="container-form-product">

            {/* Form title */}
            <h2 className='formTitle'>
                {productId ? "Edit Product" : "Register Product"}
            </h2>

            <form onSubmit={handleSubmit(onSubmit)}>
                <div className="container-inputs">

                    {/* Product Name input */}
                    <label>Product Name:</label><br />
                    <input
                        type="text"
                        placeholder='Product Name'
                        {...register("name_product", { required: true })}
                    />
                    <br /><br />

                    {/* Branch selection */}
                    <label>Branch:</label><br />
                    {branchName ? (
                        <>
                            {/* Display branch name if provided */}
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
                        // Dropdown to select branch
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

                    {/* Stock input */}
                    <label>Stock:</label><br />
                    <input
                        type="number"
                        min="0"
                        step="1"
                        placeholder="Stock"
                        {...register("stock", { required: true, min: 0 })}
                    />

                </div>

                {/* Form buttons */}
                <div className="container-btn">
                    {/* Submit button */}
                    <button type='submit' className="btn-register">
                        <i className="fa-solid fa-circle-check" />
                        {productId ? " Update" : " Register"}
                    </button>

                    {/* Cancel button */}
                    <button type="button" className="btn-cancel" onClick={closeModal}>
                        <i className="fa-solid fa-circle-xmark" /> Cancel
                    </button>
                </div>
            </form>
        </div>
    );
}

export default FormProduct;