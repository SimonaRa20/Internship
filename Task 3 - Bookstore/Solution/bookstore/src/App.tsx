import React from 'react';
import { BrowserRouter as Router, Route, Routes } from 'react-router-dom';
import NavBar from './components/NavBar';
import ProductList from './components/ProductList';
import ProductDetail from './components/ProductDetail';
import ShoppingCart from './components/ShoppingCart';
import Checkout from './components/Checkout';
import productsData from './data/products.json';

function App() {
  return (
    <div className="App">
      <Router>
        <NavBar />
        <Routes>
          <Route path="/" element={<ProductList products={productsData} />} />
          <Route path="/product/:id" element={<ProductDetail products={productsData} />} />
          <Route path='/cart' Component={ShoppingCart} />
          <Route path='/checkout' Component={Checkout} />
        </Routes>
      </Router>
    </div>
  );
}

export default App;
