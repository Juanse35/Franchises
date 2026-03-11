import React from 'react'

// Import reaact dependencies
import { BrowserRouter, Routes, Route, Navigate } from 'react-router-dom'

import './App.css'

// Import Pages
import FranchisePage from './page/FranchisePage/franchisePage.jsx'
import BranchPage from './page/BranchPage/branchPage.jsx'
import ProductPage from './page/ProductPage/productPage.jsx'

// Import Context
import { FranchiseProvider } from './context/franchiseContext.jsx'
import { BranchProvider } from './context/branchContext.jsx'
import { ProductProvider } from './context/productContext.jsx'

function App() {
  return (
    <FranchiseProvider>
      <BranchProvider>
        <ProductProvider>
          <BrowserRouter>
            <Routes>
              <Route path='/Franchise' element={<FranchisePage />} />
              <Route path='/Branch' element={<BranchPage />} />
              <Route path='/Product' element={<ProductPage />} />
              <Route path='*' element={<Navigate to="/Franchise" />} />
            </Routes>
          </BrowserRouter>
        </ProductProvider>
      </BranchProvider>
    </FranchiseProvider>
  )
}

export default App
