import { useForm } from 'react-hook-form';
import { useEffect } from 'react';
import { useFranchise } from '../../../context/franchiseContext.jsx';
import './formRegisterFranchise.css';

function FormRegisterFranchise({ closeModal, id = null }) {

  // React Hook Form configuration
  const { register, handleSubmit, setValue, formState: { errors } } = useForm();

  // Get franchise actions from context
  const { createFranchise, getFranchise, updateFranchise } = useFranchise();

  // Load franchise data when editing
  useEffect(() => {

    const loadFranchise = async () => {

      console.log('Loading franchise with ID:', id);

      if (id) {

        // Fetch franchise data by ID
        const franchise = await getFranchise(id); 

        // Fill form with franchise data
        if (franchise) {
          setValue("Name", franchise.name);
        }

      }

    };

    loadFranchise();

  }, [id, setValue, getFranchise]);

  // Submit form (create or update)
  const onSubmit = async (data) => {

    if (id) {

      // Update existing franchise
      await updateFranchise(id, data);

    } else {

      // Create new franchise
      await createFranchise(data);

    }

    // Close modal after submit
    closeModal();

  };

  return (
    <div className="container-form-franchise">

      {/* Form title changes depending on action */}
      <h2 className='formTitle'>{id ? "Edit Franchise" : "Register Franchise"}</h2>

      <form className='form' onSubmit={handleSubmit(onSubmit)}>

        {/* Franchise name input */}
        <div className="container-inputs">

          <label>Franchise Name:</label><br />

          <input
            type="text"
            placeholder='Franchise Name'
            {...register("Name", { required: true })}
          />

          {/* Validation error */}
          {errors.Name && <span className="error">Franchise Name is required</span>}

        </div>

        {/* Form buttons */}
        <div className="container-btn">

          {/* Submit button */}
          <button type='submit' className="btn-register">
            <i className="fa-solid fa-circle-check" /> {id ? "Update" : "Register"}
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

export default FormRegisterFranchise;