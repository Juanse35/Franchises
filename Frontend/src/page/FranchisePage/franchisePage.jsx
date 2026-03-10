// FranchisePage.jsx
import { useState, useEffect } from "react";
import Navbar from '../../components/nav/navBar.jsx';
import FormRegisterFranchise from '../../components/form/formFranchise/formRegisterFranchise.jsx';
import { useFranchise } from '../../context/franchiseContext.jsx';
import { useNavigate } from 'react-router-dom';
import "./franchisePage.css";

function FranchisePage() {
  const [showModal, setShowModal] = useState(false);
  const { franchises, getFranchises, deleteFranchise} = useFranchise();
  const navigate = useNavigate();
  const [selectedId, setSelectedId] = useState(null);
  const [searchTerm, setSearchTerm] = useState("");

  useEffect(() => {
    const fetchFranchises = async () => {
      await getFranchises();
    };
    fetchFranchises();
  }, []);

  const openModal = () => setShowModal(true);
  const closeModal = () => {
    setShowModal(false);
    setSelectedId(null);
  }

  const handleEdit = (id) => {
    setSelectedId(id);
    openModal();
  }

  const handleDelete = (id) => {
    if (window.confirm("Are you sure you want to delete this franchise?")) {
      deleteFranchise(id);
    }
  };

  const handleNavigate = (id) => {
    console.log(`Navigate to branches of franchise with ID: ${id}`);
    navigate(`/Branch/${id}`);
  }
  

  const normalizeText = (text) => {
    return text
      .normalize("NFD")        
      .replace(/[\u0300-\u036f]/g, "") 
      .toLowerCase();
  }

  const filteredFranchises = franchises.filter(f => 
    normalizeText(f.name).includes(normalizeText(searchTerm))
  );

  return (
    <div className='container'>

      <div className='navBarComponents'>
        <Navbar />
      </div>

      <div className="Page">

        {/*Header Page*/}
        <header className='header'>
          <div className="container-title-input">
            <h2 className='title'>Franchise</h2>
            <input
              className='search-input'
              type="search"
              placeholder='Search Franchise'
              value={searchTerm}
              onChange={(e) => setSearchTerm(e.target.value)}
            />
          </div>

          <button 
            className='create-button'
            onClick={openModal}
          >
            <i className="fa-solid fa-circle-plus"/> Create Franchise
          </button>
        </header>

        <div className="Line"></div>

        {/*Main Container Franchise*/}
        <div className='data-header'>
          <h3 className='info-data-header'>ID Franchise</h3>
          <h3 className='info-data-header'>Franchise Name</h3>
          <h3 className='info-data-header'>Registration Date</h3>
          <h3 className='info-data-header'>Action</h3>
        </div>

        <div className="data-container">
          {filteredFranchises.length > 0 ? (
            filteredFranchises.map((franchise) => (
              <div className="card-data" key={franchise.id}>
                <div className='data-item'>{franchise.id}</div>
                <div className='data-item'>{franchise.name}</div>
                <div className='data-item'>
                  {new Date(franchise.createdAt).toLocaleDateString()}
                </div>

                <div className='data-item'>
                  <button className='action edit' onClick={() => handleEdit(franchise.id)}>Edit</button>
                  <button className='action delete' onClick={() => handleDelete(franchise.id)}>Delete</button>
                  <button className='action navigate' onClick={() => handleNavigate(franchise.id)}>Branchs</button>
                </div>
              </div>
            ))
          ) : (
            <p>No franchises found.</p>
          )}
        </div>
      </div>

      {/* MODAL */}
      {showModal && (
        <div className="modal-overlay" onClick={closeModal}>
          <div 
            className="modal-content"
            onClick={(e) => e.stopPropagation()}
          >
            <FormRegisterFranchise
              closeModal={closeModal}
              id={selectedId}  
            />
          </div>
        </div>
      )}

    </div>
  );
}

export default FranchisePage;