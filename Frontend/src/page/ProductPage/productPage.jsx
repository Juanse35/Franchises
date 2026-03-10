import Navbar from '../../components/nav/navBar.jsx'

import "./productPage.css"

function ProductPage() {
    return (
        <div className='container'>

            <div className='navBarComponents'>
                <Navbar />
            </div>

            <div className="Page">

                {/*Header Page*/}
                <header className='header'>
                    <div className="container-title-input">
                        <h2 className='title'>Products</h2>
                        <input className='search-input' type="search" placeholder='Search Product' />
                    </div>
                </header>
                <div className="Line"></div>

                {/*Main Container Franchise*/}
                <div className='data-header'>
                    <h3 className='info-data-header'>id Product</h3>
                    <h3 className='info-data-header'>Product Name</h3>
                    <h3 className='info-data-header'>Branch Name</h3>
                    <h3 className='info-data-header'>Franchise Name</h3>
                    <h3 className='info-data-header'>Product Stock</h3>
                    <h3 className='info-data-header'>Action </h3>
                </div>
                <div className="data-container">
                    <div className="card-data">
                        <div className='data-item'>1</div>
                        <div className='data-item'>Nintendo Switch</div>
                        <div className='data-item'>Japon</div>
                        <div className='data-item'>Nintendo</div>
                        <div className='data-item'>120</div>
                        <div className='data-item'>
                            <button className='action edit'>Edit</button>
                            <button className='action delete'>Delete</button>
                        </div>
                    </div>

                </div>
            </div>
        </div>
    )
}

export default ProductPage