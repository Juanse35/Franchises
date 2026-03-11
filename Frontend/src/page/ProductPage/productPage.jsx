import { useState, useEffect, useMemo } from "react";
import Navbar from '../../components/nav/navBar.jsx';
import FormProduct from '../../components/form/formProduct/formProduct.jsx';
import ModalInfo from "../../components/modal/infoModal.jsx";
import { useProduct } from "../../context/productContext.jsx";

import "./productPage.css";

function ProductPage() {

  // Get products data and actions from context
  const { products, getProducts, deleteProduct } = useProduct();

  // Modal visibility state
  const [showModal, setShowModal] = useState(false);

  // Defines which modal will be displayed
  const [modalType, setModalType] = useState(null);

  // Stores selected product ID
  const [selectedProductId, setSelectedProductId] = useState(null);

  // Search and filter states
  const [searchTerm, setSearchTerm] = useState("");
  const [branchFilter, setBranchFilter] = useState("");
  const [statusFilter, setStatusFilter] = useState("");

  // Load products when component mounts
  useEffect(() => {
    getProducts();
  }, []);

  // Open modal
  const openModal = () => setShowModal(true);

  // Close modal and reset data
  const closeModal = () => {
    setShowModal(false);
    setModalType(null);
    setSelectedProductId(null);
  };

  // Edit product
  const handleEdit = (productId) => {
    setSelectedProductId(productId);
    setModalType("product");
    openModal();
  };

  // Open product info modal
  const handleInfo = (productId) => {
    setSelectedProductId(productId);
    setModalType("info");
    openModal();
  };

  // Delete product
  const handleDelete = (productId) => {
      deleteProduct(productId)
  };

  // Determine stock status based on quantity
  const getStockStatus = (stock) => {
    if (stock === 0) return "exhausted";
    if (stock >= 1 && stock <= 20) return "limited";
    return "available";
  };

  // Return color for each stock status
  const getStatusColor = (status) => {
    switch (status) {
      case "available":
        return "#1fa121"; 
      case "limited":
        return "#e7d40e"; 
      case "exhausted":
        return "#cc1515"; 
      default:
        return "#000000"; 
    }
  };

  // Filter products by search, branch and status
  const filteredProducts = useMemo(() => {
    return products.filter((p) => {

      const name = p.name_product?.toLowerCase() || "";
      const branch = p.branchName?.toLowerCase() || "";
      const search = searchTerm.toLowerCase();

      const matchesSearch = name.includes(search) || branch.includes(search);
      const matchesBranch = branchFilter ? branch === branchFilter.toLowerCase() : true;
      const matchesStatus = statusFilter ? getStockStatus(p.stock) === statusFilter : true;

      return matchesSearch && matchesBranch && matchesStatus;

    });
  }, [products, searchTerm, branchFilter, statusFilter]);

  // Unique branch names for filter select
  const branchOptions = [...new Set(products.map(p => p.branchName))];

  return (
    <div className="container">

      {/* Navigation bar */}
      <div className="navBarComponents">
        <Navbar />
      </div>

      <div className="Page">

        {/* Page header */}
        <header className="header">

          <div className="container-title-input">

            <h2 className="title">Products</h2>

            {/* Search input */}
            <input
              type="search"
              placeholder="Search Product or Branch"
              className="search-input"
              value={searchTerm}
              onChange={(e) => setSearchTerm(e.target.value)}
            />

          </div>

          {/* Filter by branch */}
          <select
            className="select"
            value={branchFilter}
            onChange={(e) => setBranchFilter(e.target.value)}
          >
            <option value="">All Branches</option>
            {branchOptions.map((b, i) => (
              <option key={i} value={b}>{b}</option>
            ))}
          </select>

          {/* Filter by product status */}
          <select
            className="select"
            value={statusFilter}
            onChange={(e) => setStatusFilter(e.target.value)}
          >
            <option value="">All Status</option>
            <option value="available">Available</option>
            <option value="limited">Limited</option>
            <option value="exhausted">Exhausted</option>
          </select>

          {/* Button to create product */}
          <button
            className="create-button"
            onClick={() => {
              setModalType("product");
              openModal();
            }}
          >
            <i className="fa-solid fa-circle-plus" /> Create Product
          </button>

        </header>

        <div className="Line"></div>

        {/* Table header */}
        <div className="data-header">
          <h3 className="info-data-header">Id Product</h3>
          <h3 className="info-data-header">Product Name</h3>
          <h3 className="info-data-header">Branch Name</h3>
          <h3 className="info-data-header">Product Stock</h3>
          <h3 className="info-data-header">Status</h3>
          <h3 className="info-data-header">Action</h3>
        </div>

        {/* Product data */}
        <div className="data-container">

          {filteredProducts.length > 0 ? (

            filteredProducts.map((product) => {

              // Calculate stock status
              const status = getStockStatus(product.stock);

              return (

                <div className="card-data" key={product.id_product}>

                  {/* Product ID */}
                  <div className="data-item">{product.id_product}</div>

                  {/* Product name */}
                  <div className="data-item">{product.name_product}</div>

                  {/* Branch name */}
                  <div className="data-item">{product.branchName}</div>

                  {/* Product stock */}
                  <div className="data-item">{product.stock}</div>

                  {/* Stock status */}
                  <span
                    className="data-item status-product"
                    style={{ backgroundColor: getStatusColor(status), fontWeight:400 }}
                  >
                    {status}
                  </span>

                  {/* Action buttons */}
                  <div className="data-item">

                    {/* Edit product */}
                    <button className="action edit" onClick={() => handleEdit(product.id_product)}>Edit</button>

                    {/* Delete product */}
                    <button className="action delete" onClick={() => handleDelete(product.id_product)}>Delete</button>

                    {/* View product info */}
                    <button className="action navigate" onClick={() => handleInfo(product.id_product)}>Info</button>

                  </div>

                </div>

              );
            })

          ) : (

            // Message when no products are found
            <p>No products found.</p>

          )}

        </div>

      </div>

      {/* Modal container */}
      {showModal && (

        <div className="modal-overlay" onClick={closeModal}>

          {/* Prevent modal close when clicking inside */}
          <div className="modal-content" onClick={(e) => e.stopPropagation()}>

            {/* Product form */}
            {modalType === "product" && (
              <FormProduct
                closeModal={closeModal}
                productId={selectedProductId} 
              />
            )}

            {/* Product info modal */}
            {modalType === "info" && (
              <ModalInfo
                closeModal={closeModal}
                productId={selectedProductId}
              />
            )}

          </div>

        </div>

      )}

    </div>
  );
}

export default ProductPage;