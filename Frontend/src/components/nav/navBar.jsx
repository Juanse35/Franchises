import './navBar.css'
import { Link, useLocation } from 'react-router-dom'

function Navbar() {

  // Get current route path
  const location = useLocation()

  return (
    <div className='navBar'>

      {/* Application title */}
      <div className="navBar-title">
        <h1>Franchise App</h1>
      </div>

      <nav>
        <ul>

          {/* Franchise page link */}
          <Link to="/Franchise" className='Link'>
            <li className={location.pathname === "/Franchise" ? "Active" : ""}>
              <i className="fa-solid fa-shop"/> Franchise
            </li>
          </Link>

          {/* Branch page link */}
          <Link to="/Branch" className='Link'>
            <li className={location.pathname === "/Branch" ? "Active" : ""}>
              <i className="fa-solid fa-diagram-project"/> Branch
            </li>
          </Link>

          {/* Product page link */}
          <Link to="/Product" className='Link'>
            <li className={location.pathname === "/Product" ? "Active" : ""}>
              <i className="fa-solid fa-tags"/> Product
            </li>
          </Link>

        </ul>
      </nav>

    </div>
  )
}

export default Navbar