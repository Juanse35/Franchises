import { useEffect, useState } from "react";
import { useProduct } from "../../context/productContext.jsx";
import './infoModal.css'

function ModalInfo({ productId, closeModal }) {
  const { getProduct } = useProduct();
  const [product, setProduct] = useState(null);

  useEffect(() => {
    const fetchProduct = async () => {
      if (productId) {
        const data = await getProduct(productId);
        setProduct(data);
      }
    };
    fetchProduct();
  }, [productId]);

  if (!product) return <p>Loading...</p>;

  return (
    <div className="container-info">
      <h2 className="formTitle">Product Information</h2>
      <div className="container-information">
        <p><strong>Id:</strong> {product.id_product}</p>
        <p><strong>Name:</strong> {product.name_product}</p>
        <p><strong>Branch:</strong> {product.branchName}</p>
        <p><strong>Franchise:</strong> {product.franchiseName}</p>
        <p><strong>Stock:</strong> {product.stock}</p>
        <p className="status"><strong>Status:</strong> {product.stock === 0 ? "Exhausted" : product.stock <= 20 ? "Limited" : "Available"}</p>
      </div>
      <button className="btn-close" onClick={closeModal}>
        Close
      </button>
    </div>
  );
}

export default ModalInfo;