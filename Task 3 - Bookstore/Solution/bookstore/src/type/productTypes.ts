export interface Product {
    id: number;
    title: string;
    description: string;
    price: number;
    image: string;
    rating: number;
  }

export interface CartItem {
    product: Product;
    amount: number;
  }
  
export interface RootState {
    cart: CartItem[];
  }