import './navBar.css'
import { Link } from 'react-router-dom'

function navbar() {
  return (
    <div className='navBar'>
        <div className="navBar-title">
            <h1>Franchise App</h1>
        </div>
        <nav>
            <ul>
                <Link to="/Franchise" className='Link'><li className='Active'><i className="fa-solid fa-shop"/>Franchise</li></Link>
                <Link to="/Branch" className='Link'><li><i className="fa-solid fa-diagram-project"/>Branch</li></Link>
                <Link to="/Product" className='Link'><li><i className="fa-solid fa-tags"/>Product</li></Link>
            </ul>
        </nav>
    </div>
  )
}

export default navbar