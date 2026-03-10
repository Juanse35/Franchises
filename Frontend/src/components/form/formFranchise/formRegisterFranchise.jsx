import { useForm } from 'react-hook-form';
import { useEffect } from 'react';
import { useFranchise } from '../../../context/franchiseContext.jsx';
import './formRegisterFranchise.css';

function FormRegisterFranchise({ closeModal, id = null }) {
  const { register, handleSubmit, setValue, formState: { errors } } = useForm();
  const { createFranchise, getFranchise, updateFranchise } = useFranchise();

  useEffect(() => {
    const loadFranchise = async () => {
      console.log('Loading franchise with ID:', id);
      if (id) {
        const franchise = await getFranchise(id); 
        if (franchise) {
          setValue("Name", franchise.name);
        }
      }
    };
    loadFranchise();
  }, [id, setValue, getFranchise]);

  const onSubmit = async (data) => {
    if (id) {
      await updateFranchise(id, data);
    } else {
      await createFranchise(data);
    }
    closeModal();
  };

  return (
    <div className="container-form-franchise">
      <h2 className='formTitle'>{id ? "Edit Franchise" : "Register Franchise"}</h2>

      <form className='form' onSubmit={handleSubmit(onSubmit)}>
        <div className="container-inputs">
          <label>Franchise Name:</label><br />
          <input
            type="text"
            placeholder='Franchise Name'
            {...register("Name", { required: true })}
          />
          {errors.Name && <span className="error">Franchise Name is required</span>}
        </div>

        <div className="container-btn">
          <button type='submit' className="btn-register">
            <i className="fa-solid fa-circle-check" /> {id ? "Update" : "Register"}
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

export default FormRegisterFranchise;