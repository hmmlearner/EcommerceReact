import React, { createContext } from 'react';

const CartContext = createContext({
    cartItems: [],
    totalItems: 0,
    totalPrice: 0,
    shoppingCartId: 0,
    addToCart: (item) => { },
    removeFromCart: (id) => { },
    clearCart: () => { },
    retrieveCartData: () => { },
});

export default CartContext;
