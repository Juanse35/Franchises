import { useParams } from 'react-router-dom'
import { useEffect, useState, useMemo } from 'react'

import Navbar from '../../components/nav/navBar.jsx'
import { useBranch } from '../../context/branchContext.jsx'

import FormBranch from '../../components/form/formBranch/formBranch.jsx'
import FormProduct from '../../components/form/formProduct/formProduct.jsx'

import "./branchPage.css"

function BranchPage() {

  // Get branches data and actions from context
  const { branches, getBranches, getBranchByIdFranchise, deleteBranch } = useBranch()

  // Get franchise ID from URL params
  const { id } = useParams()

  // Modal visibility state
  const [showModal, setShowModal] = useState(false)

  // Defines which modal form will be displayed
  const [modalType, setModalType] = useState(null)

  // Stores selected branch ID for editing
  const [selectedId, setSelectedId] = useState(null)

  // Stores selected branch data for product form
  const [selectedBranchId, setSelectedBranchId] = useState(null)
  const [selectedBranchName, setSelectedBranchName] = useState(null)

  // Search and filter states
  const [searchTerm, setSearchTerm] = useState("")
  const [branchFilter, setBranchFilter] = useState("")
  const [franchiseFilter, setFranchiseFilter] = useState("")

  // Load branches when component mounts or ID changes
  useEffect(() => {

    const fetchBranches = async () => {

      if (id) {
        // Get branches from a specific franchise
        await getBranchByIdFranchise(id)
      } else {
        // Get all branches
        await getBranches()
      }

    }

    fetchBranches()

  }, [id])

  // Open modal
  const openModal = () => setShowModal(true)

  // Close modal and reset selected data
  const closeModal = () => {

    setShowModal(false)

    setSelectedId(null)

    setSelectedBranchId(null)
    setSelectedBranchName(null)

    setModalType(null)

  }

  // Edit branch
  const handleEdit = (branchId) => {

    setSelectedId(branchId)

    setModalType("branch")

    openModal()

  }

  // Delete branch
  const handleDelete = (branchId) => {
      deleteBranch(branchId)
  }

  // Open product form for selected branch
  const handleProducts = (branchId, branchName) => {

    setSelectedBranchId(branchId)
    setSelectedBranchName(branchName)

    setModalType("product")

    openModal()

  }

  // Normalize text to ignore accents
  const normalizeText = (text) =>
    text.toString().normalize("NFD").replace(/[\u0300-\u036f]/g, "").toLowerCase()

  // Filter branches by search and selects
  const filteredBranches = useMemo(() => {

    return branches.filter(branch => {

      const search = normalizeText(searchTerm)
      const name = normalizeText(branch.name_branch)
      const franchise = normalizeText(branch.franchiseName)

      return (
        (name.includes(search) || franchise.includes(search)) &&
        (branchFilter === "" || name === normalizeText(branchFilter)) &&
        (franchiseFilter === "" || franchise === normalizeText(franchiseFilter))
      )

    })

  }, [branches, searchTerm, branchFilter, franchiseFilter])

  // Unique branch names for select filter
  const branchOptions = [...new Set(branches.map(b => b.name_branch))]

  // Unique franchise names for select filter
  const franchiseOptions = [...new Set(branches.map(b => b.franchiseName))]

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

            <h2 className='title'>Branches</h2>

            {/* Search input */}
            <input
              className='search-input'
              type="search"
              placeholder='Search Branch or Franchise'
              value={searchTerm}
              onChange={(e) => setSearchTerm(e.target.value)}
            />

          </div>

          {/* Filter by branch */}
          <select
            value={branchFilter}
            onChange={(e) => setBranchFilter(e.target.value)}
            className='select'
          >

            <option value="">All Branches</option>

            {branchOptions.map((b, i) => (
              <option key={i} value={b}>{b}</option>
            ))}

          </select>

          {/* Filter by franchise */}
          <select
            value={franchiseFilter}
            onChange={(e) => setFranchiseFilter(e.target.value)}
            className='select'
          >

            <option value="">All Franchises</option>

            {franchiseOptions.map((f, i) => (
              <option key={i} value={f}>{f}</option>
            ))}

          </select>

          {/* Button to create branch */}
          <button
            className='create-button'
            onClick={() => {

              setModalType("branch")

              openModal()

            }}
          >
            <i className="fa-solid fa-circle-plus" /> Create Branch
          </button>

        </header>

        <div className="Line"></div>

        {/* Table header */}
        <div className='data-header'>

          <h3 className='info-data-header'>Id Branch</h3>
          <h3 className='info-data-header'>Branch Name</h3>
          <h3 className='info-data-header'>Franchise Name</h3>
          <h3 className='info-data-header'>Registration Date</h3>
          <h3 className='info-data-header'>Action</h3>

        </div>

        {/* Branch data */}
        <div className="data-container">

          {filteredBranches.length > 0 ? (

            filteredBranches.map((branch) => (

              <div className="card-data" key={branch.id_branch}>

                {/* Branch ID */}
                <div className='data-item'>{branch.id_branch}</div>

                {/* Branch name */}
                <div className='data-item'>{branch.name_branch}</div>

                {/* Franchise name */}
                <div className='data-item'>{branch.franchiseName}</div>

                {/* Registration date */}
                <div className='data-item'>
                  {new Date(branch.registrationDate).toLocaleDateString()}
                </div>

                {/* Action buttons */}
                <div className='data-item'>

                  {/* Edit branch */}
                  <button
                    className='action edit'
                    onClick={() => handleEdit(branch.id_branch)}
                  >
                    Edit
                  </button>

                  {/* Delete branch */}
                  <button
                    className='action delete'
                    onClick={() => handleDelete(branch.id_branch)}
                  >
                    Delete
                  </button>

                  {/* Open products of this branch */}
                  <button
                    className='action navigate'
                    onClick={() =>
                      handleProducts(branch.id_branch, branch.name_branch)
                    }
                  >
                    Products
                  </button>

                </div>

              </div>

            ))

          ) : (

            // Message if no results found
            <p>No branches found.</p>

          )}

        </div>

      </div>

      {/* Modal container */}
      {showModal && (

        <div className="modal-overlay" onClick={closeModal}>

          {/* Prevent modal close when clicking inside */}
          <div
            className="modal-content"
            onClick={(e) => e.stopPropagation()}
          >

            {/* Branch form */}
            {modalType === "branch" && (

              <FormBranch
                closeModal={closeModal}
                id={selectedId}
                franchiseId={id}
              />

            )}

            {/* Product form */}
            {modalType === "product" && (

              <FormProduct
                closeModal={closeModal}
                branchId={selectedBranchId}
                branchName={selectedBranchName}
              />

            )}

          </div>

        </div>

      )}

    </div>

  )

}

export default BranchPage