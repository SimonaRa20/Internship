import { createAction } from '@reduxjs/toolkit';
import { Product } from '../type/productTypes';

export const addToCart = createAction<{ product: Product; amount: number }>('cart/addToCart');
export const removeFromCart = createAction<number>('cart/removeFromCart');
export const cleanCart = createAction('cart/cleanCart');
export const updateCartItem = createAction<{ productId: number; amount: number }>('cart/updateCartItem');
