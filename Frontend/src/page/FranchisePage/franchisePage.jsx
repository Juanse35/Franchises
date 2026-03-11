import { useState, useEffect } from "react";
import Navbar from '../../components/nav/navBar.jsx';
import FormRegisterFranchise from '../../components/form/formFranchise/formRegisterFranchise.jsx';
import FormBranch from '../../components/form/formBranch/formBranch.jsx';

import { useFranchise } from '../../context/franchiseContext.jsx';

import "./franchisePage.css";

function FranchisePage() {

  // Get franchise data and actions from context
  const { franchises, getFranchises, deleteFranchise } = useFranchise();

  // Modal visibility state
  const [showModal, setShowModal] = useState(false);

  // Determines which modal form will be displayed
  const [modalType, setModalType] = useState(null); 

  // Stores selected franchise ID for edit or branch creation
  const [selectedId, setSelectedId] = useState(null);

  // Stores selected franchise name for branch form
  const [selectedName, setSelectedName] = useState(null);

  // Search input state
  const [searchTerm, setSearchTerm] = useState("");

  // Load franchises when component mounts
  useEffect(() => {
    const fetchFranchises = async () => {
      await getFranchises();
    };

    fetchFranchises();
  }, []);

  // Open modal
  const openModal = () => setShowModal(true);

  // Close modal and reset selection
  const closeModal = () => {
    setShowModal(false);
    setSelectedId(null);
    setSelectedName(null);
    setModalType(null);
  };

  // Open modal to edit a franchise
  const handleEdit = (id) => {
    setSelectedId(id);
    setModalType("franchise");
    openModal();
  };

  // Open modal to create or view branches
  const handleBranch = (id, name) => {
    setSelectedId(id);
    setSelectedName(name);
    setModalType("branch");
    openModal();
  };

  // Delete selected franchise
  const handleDelete = (id) => {
      deleteFranchise(id);
  };

  // Normalize text to ignore accents in search
  const normalizeText = (text) => {
    return text
      .normalize("NFD")
      .replace(/[\u0300-\u036f]/g, "")
      .toLowerCase();
  };

  // Filter franchises by search term
  const filteredFranchises = franchises.filter(f =>
    normalizeText(f.name).includes(normalizeText(searchTerm))
  );

  return (
    <div className='container'>

      {/* Navigation bar */}
      <div className='navBarComponents'>
        <Navbar />
      </div>

      <div className="Page">

        {/* Page header */}
        <header className='header'>
          <div className="container-title-input">
            <h2 className='title'>Franchise</h2>

            {/* Search input */}
            <input
              className='search-input'
              type="search"
              placeholder='Search Franchise'
              value={searchTerm}
              onChange={(e) => setSearchTerm(e.target.value)}
            />
          </div>

          {/* Button to create a new franchise */}
          <button
            className='create-button'
            onClick={() => {
              setModalType("franchise");
              openModal();
            }}
          >
            <i className="fa-solid fa-circle-plus" /> Create Franchise
          </button>
        </header>

        <div className="Line"></div>

        {/* Table header */}
        <div className='data-header'>
          <h3 className='info-data-header'>ID Franchise</h3>
          <h3 className='info-data-header'>Franchise Name</h3>
          <h3 className='info-data-header'>Registration Date</h3>
          <h3 className='info-data-header'>Action</h3>
        </div>

        {/* Franchise data list */}
        <div className="data-container">

          {filteredFranchises.length > 0 ? (

            filteredFranchises.map((franchise) => (

              <div className="card-data" key={franchise.id}>

                {/* Franchise ID */}
                <div className='data-item'>{franchise.id}</div>

                {/* Franchise name */}
                <div className='data-item'>{franchise.name}</div>

                {/* Creation date */}
                <div className='data-item'>
                  {new Date(franchise.createdAt).toLocaleDateString()}
                </div>

                {/* Action buttons */}
                <div className='data-item'>

                  {/* Edit franchise */}
                  <button
                    className='action edit'
                    onClick={() => handleEdit(franchise.id)}
                  >
                    Edit
                  </button>

                  {/* Delete franchise */}
                  <button
                    className='action delete'
                    onClick={() => handleDelete(franchise.id)}
                  >
                    Delete
                  </button>

                  {/* Manage branches */}
                  <button
                    className='action navigate'
                    onClick={() => handleBranch(franchise.id, franchise.name)}
                  >
                    Branchs
                  </button>

                </div>

              </div>

            ))

          ) : (

            // Message if no results found
            <p>No franchises found.</p>

          )}

        </div>

      </div>

      {/* Modal container */}
      {showModal && (

        <div className="modal-overlay" onClick={closeModal}>

          {/* Prevent modal from closing when clicking inside */}
          <div
            className="modal-content"
            onClick={(e) => e.stopPropagation()}
          >

            {/* Franchise form */}
            {modalType === "franchise" && (

              <FormRegisterFranchise
                closeModal={closeModal}
                id={selectedId}
              />

            )}

            {/* Branch form */}
            {modalType === "branch" && (

              <FormBranch
                closeModal={closeModal}
                franchiseId={selectedId}
                franchiseName={selectedName}
              />

            )}

          </div>

        </div>

      )}

    </div>
  );
}

export default FranchisePage;