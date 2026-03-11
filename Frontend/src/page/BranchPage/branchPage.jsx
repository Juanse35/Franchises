import { useParams } from 'react-router-dom'
import { useEffect, useState, useMemo } from 'react'

import Navbar from '../../components/nav/navBar.jsx'
import { useBranch } from '../../context/branchContext.jsx'

import FormBranch from '../../components/form/formBranch/formBranch.jsx'
import FormProduct from '../../components/form/formProduct/formProduct.jsx'

import "./branchPage.css"

function BranchPage() {

  const { branches, getBranches, getBranchByIdFranchise, deleteBranch } = useBranch()
  const { id } = useParams()

  const [showModal, setShowModal] = useState(false)

  const [modalType, setModalType] = useState(null)

  const [selectedId, setSelectedId] = useState(null)

  const [selectedBranchId, setSelectedBranchId] = useState(null)
  const [selectedBranchName, setSelectedBranchName] = useState(null)

  const [searchTerm, setSearchTerm] = useState("")
  const [branchFilter, setBranchFilter] = useState("")
  const [franchiseFilter, setFranchiseFilter] = useState("")

  useEffect(() => {

    const fetchBranches = async () => {

      if (id) {
        await getBranchByIdFranchise(id)
      } else {
        await getBranches()
      }

    }

    fetchBranches()

  }, [id])

  const openModal = () => setShowModal(true)

  const closeModal = () => {

    setShowModal(false)

    setSelectedId(null)

    setSelectedBranchId(null)
    setSelectedBranchName(null)

    setModalType(null)

  }

  // EDITAR BRANCH
  const handleEdit = (branchId) => {

    setSelectedId(branchId)

    setModalType("branch")

    openModal()

  }

  const handleDelete = (branchId) => {
      deleteBranch(branchId)
  }

  // ABRIR FORMULARIO PRODUCTO
  const handleProducts = (branchId, branchName) => {

    setSelectedBranchId(branchId)
    setSelectedBranchName(branchName)

    setModalType("product")

    openModal()

  }

  const normalizeText = (text) =>
    text.toString().normalize("NFD").replace(/[\u0300-\u036f]/g, "").toLowerCase()

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

  const branchOptions = [...new Set(branches.map(b => b.name_branch))]
  const franchiseOptions = [...new Set(branches.map(b => b.franchiseName))]

  return (

    <div className='container'>

      <div className='navBarComponents'>
        <Navbar />
      </div>

      <div className="Page">

        {/* Header */}
        <header className='header'>

          <div className="container-title-input">

            <h2 className='title'>Branches</h2>

            <input
              className='search-input'
              type="search"
              placeholder='Search Branch or Franchise'
              value={searchTerm}
              onChange={(e) => setSearchTerm(e.target.value)}
            />

          </div>

          {/* Select Branch */}
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

          {/* Select Franchise */}
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

        {/* Table Header */}
        <div className='data-header'>

          <h3 className='info-data-header'>Id Branch</h3>
          <h3 className='info-data-header'>Branch Name</h3>
          <h3 className='info-data-header'>Franchise Name</h3>
          <h3 className='info-data-header'>Registration Date</h3>
          <h3 className='info-data-header'>Action</h3>

        </div>

        {/* Data */}
        <div className="data-container">

          {filteredBranches.length > 0 ? (

            filteredBranches.map((branch) => (

              <div className="card-data" key={branch.id_branch}>

                <div className='data-item'>{branch.id_branch}</div>

                <div className='data-item'>{branch.name_branch}</div>

                <div className='data-item'>{branch.franchiseName}</div>

                <div className='data-item'>
                  {new Date(branch.registrationDate).toLocaleDateString()}
                </div>

                <div className='data-item'>

                  <button
                    className='action edit'
                    onClick={() => handleEdit(branch.id_branch)}
                  >
                    Edit
                  </button>

                  <button
                    className='action delete'
                    onClick={() => handleDelete(branch.id_branch)}
                  >
                    Delete
                  </button>

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

            <p>No branches found.</p>

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

            {modalType === "branch" && (

              <FormBranch
                closeModal={closeModal}
                id={selectedId}
                franchiseId={id}
              />

            )}

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