import { useParams } from 'react-router-dom'
import { useEffect } from 'react'

import Navbar from '../../components/nav/navBar.jsx'
import { useBranch } from '../../context/branchContext.jsx'

import "./branchPage.css"

function BranchPage() {

  const { branches, getBranches, getBranchByIdFranchise } = useBranch()
  const { id } = useParams()

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

  return (
    <div className='container'>

      <div className='navBarComponents'>
        <Navbar />
      </div>

      <div className="Page">

        {/*Header Page*/}
        <header className='header'>
          <div className="container-title-input">
            <h2 className='title'>Branchs</h2>
            <input
              className='search-input'
              type="search"
              placeholder='Search Branch'
            />
          </div>

          <button 
            className='create-button'
          >
            <i className="fa-solid fa-circle-plus"/> Create Branches
          </button>
        </header>

        <div className="Line"></div>

        {/*Header Table*/}
        <div className='data-header'>
          <h3 className='info-data-header'>Id Branch</h3>
          <h3 className='info-data-header'>Branch Name</h3>
          <h3 className='info-data-header'>Franchise Name</h3>
          <h3 className='info-data-header'>Registration Date</h3>
          <h3 className='info-data-header'>Action</h3>
        </div>

        <div className="data-container">

          {branches && branches.length > 0 ? (
            branches.map((branch) => (
              <div className="card-data" key={branch.id_branch}>

                <div className='data-item'>
                  {branch.id_branch}
                </div>

                <div className='data-item'>
                  {branch.name_branch}
                </div>

                <div className='data-item'>
                  {branch.franchiseName}
                </div>

                <div className='data-item'>
                  {new Date(branch.registrationDate).toLocaleDateString()}
                </div>

                <div className='data-item'>
                  <button className='action edit'>
                    Edit
                  </button>

                  <button className='action delete'>
                    Delete
                  </button>

                  <button className='action navigate'>
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
    </div>
  )
}

export default BranchPage