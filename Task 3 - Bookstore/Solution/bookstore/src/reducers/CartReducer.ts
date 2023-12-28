import { createReducer } from '@reduxjs/toolkit';
import { addToCart, removeFromCart, updateCartItem, cleanCart } from '../actions/CartActions';
import { Product } from '../type/productTypes';

interface CartItem {
  product: Product;
  amount: number;
}

const initialState: CartItem[] = [];

const CartReducer = createReducer(initialState, (builder) => {
  builder
    .addCase(addToCart, (state, action) => {
      const { product, amount } = action.payload;
      const existingCartItemIndex = state.findIndex((item) => item.product.id === product.id);

      if (existingCartItemIndex !== -1) {
        state[existingCartItemIndex].amount += amount;
      } else {
        state.push({ product, amount });
      }
    })
    .addCase(removeFromCart, (state, action) => {
      const productId = action.payload;
      return state.filter((item) => item.product.id !== productId);
    })
    .addCase(updateCartItem, (state, action) => {
      const { productId, amount } = action.payload;
      const cartItem = state.find((item) => item.product.id === productId);
      if (cartItem) {
        cartItem.amount = amount;
      }
    })
    .addCase(cleanCart, () => []);
});

export default CartReducer;
