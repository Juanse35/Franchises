import { useState, useEffect, useMemo } from "react";
import Navbar from '../../components/nav/navBar.jsx';
import FormProduct from '../../components/form/formProduct/formProduct.jsx';
import ModalInfo from "../../components/modal/infoModal.jsx";
import { useProduct } from "../../context/productContext.jsx";

import "./productPage.css";

function ProductPage() {
  const { products, getProducts, deleteProduct } = useProduct();

  const [showModal, setShowModal] = useState(false);
  const [modalType, setModalType] = useState(null);
  const [selectedProductId, setSelectedProductId] = useState(null);

  const [searchTerm, setSearchTerm] = useState("");
  const [branchFilter, setBranchFilter] = useState("");
  const [statusFilter, setStatusFilter] = useState("");

  useEffect(() => {
    getProducts();
  }, []);

  const openModal = () => setShowModal(true);
  const closeModal = () => {
    setShowModal(false);
    setModalType(null);
    setSelectedProductId(null);
  };

  const handleEdit = (productId) => {
    setSelectedProductId(productId);
    setModalType("product");
    openModal();
  };

  const handleInfo = (productId) => {
    setSelectedProductId(productId);
    setModalType("info");
    openModal();
  };

  const handleDelete = (productId) => {
      deleteProduct(productId)
  };

  const getStockStatus = (stock) => {
    if (stock === 0) return "exhausted";
    if (stock >= 1 && stock <= 20) return "limited";
    return "available";
  };

  // Función para obtener color según status
  const getStatusColor = (status) => {
    switch (status) {
      case "available":
        return "#1fa121"; 
      case "limited":
        return "#d8ae15"; 
      case "exhausted":
        return "#cc1515"; 
      default:
        return "#000000"; 
    }
  };

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

  const branchOptions = [...new Set(products.map(p => p.branchName))];

  return (
    <div className="container">
      <div className="navBarComponents">
        <Navbar />
      </div>

      <div className="Page">
        {/* Header */}
        <header className="header">
          <div className="container-title-input">
            <h2 className="title">Products</h2>
            <input
              type="search"
              placeholder="Search Product or Branch"
              className="search-input"
              value={searchTerm}
              onChange={(e) => setSearchTerm(e.target.value)}
            />
          </div>

          {/* Branch Filter */}
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

          {/* Status Filter */}
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

        {/* Table Header */}
        <div className="data-header">
          <h3 className="info-data-header">Id Product</h3>
          <h3 className="info-data-header">Product Name</h3>
          <h3 className="info-data-header">Branch Name</h3>
          <h3 className="info-data-header">Product Stock</h3>
          <h3 className="info-data-header">Status</h3>
          <h3 className="info-data-header">Action</h3>
        </div>

        {/* Data */}
        <div className="data-container">
          {filteredProducts.length > 0 ? (
            filteredProducts.map((product) => {
              const status = getStockStatus(product.stock);
              return (
                <div className="card-data" key={product.id_product}>
                  <div className="data-item">{product.id_product}</div>
                  <div className="data-item">{product.name_product}</div>
                  <div className="data-item">{product.branchName}</div>
                  <div className="data-item">{product.stock}</div>
                  <span
                    className="data-item status-product"
                    style={{ color: getStatusColor(status), fontWeight: 'bold' }}
                  >
                    {status}
                  </span>
                  <div className="data-item">
                    <button className="action edit" onClick={() => handleEdit(product.id_product)}>Edit</button>
                    <button className="action delete" onClick={() => handleDelete(product.id_product)}>Delete</button>
                    <button className="action navigate" onClick={() => handleInfo(product.id_product)}>Info</button>
                  </div>
                </div>
              );
            })
          ) : (
            <p>No products found.</p>
          )}
        </div>
      </div>

      {/* Modal */}
      {showModal && (
        <div className="modal-overlay" onClick={closeModal}>
          <div className="modal-content" onClick={(e) => e.stopPropagation()}>
            {modalType === "product" && (
              <FormProduct
                closeModal={closeModal}
                productId={selectedProductId} 
              />
            )}
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