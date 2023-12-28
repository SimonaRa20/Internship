import { configureStore } from '@reduxjs/toolkit';
import cartReducer from './reducers/CartReducer';

const Store = configureStore({
  reducer: {
    cart: cartReducer,
  },
});

export default Store;

export type AppDispatch = typeof Store.dispatch;
export type AppStore = typeof Store;
