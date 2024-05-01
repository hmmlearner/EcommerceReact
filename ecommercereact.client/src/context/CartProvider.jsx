import React, { useReducer, useEffect } from 'react';
import CartContext from './cart-context';
import agent from "../api/agent";
import { getAuthToken } from "../utils/AuthService";

// Define the initial state
const initialState = {
    cartItems: [],
    totalItems: 0,
    totalPrice: 0,
};

// Define actions
const ADD_TO_CART = 'ADD_TO_CART';
const REMOVE_FROM_CART = 'REMOVE_FROM_CART';
const SET_CART_DATA = 'SET_CART_DATA';


// Reducer function
const cartReducer = (state, action) => {
    console.log(action.type + " in CartReducer " + JSON.stringify(action.payload));

    switch (action.type) {
        case ADD_TO_CART:
            console.log(state.totalPrice + " ADD_TO_CART " + JSON.stringify(action.payload));
            //const updatedTotalAmount = state.totalPrice + action.payload.item.product.price * action.payload.quantity;
            const updatedTotalAmount = state.totalPrice + action.payload.salePrice * action.payload.quantity;
            const updatedCartCount = state.totalItems + action.payload.quantity;
            console.log("ADD_TO_CART " + updatedTotalAmount);
            const existingCartItems = [...state.cartItems];
            const itemExists = existingCartItems.find(item => item.productId === action.payload.productId);
            console.log(JSON.stringify(itemExists)+" existingCartItems1 " + JSON.stringify(existingCartItems));
            if (itemExists) {
                
                let index = existingCartItems.findIndex(item => item.productId === action.payload.productId);
                console.log(existingCartItems[index].quantity + " existingCartItems2 " + JSON.stringify(action.payload.quantity));
                existingCartItems[index].quantity = existingCartItems[index].quantity+ action.payload.quantity;
                console.log(state.totalItems + " existingCartItems3 " + JSON.stringify(existingCartItems[index]));

                return {
                    //...state,
                    //cartItems: [...state.cartItems, action.payload],
                    //totalItems: state.totalItems + 1,
                    //totalPrice: state.totalPrice + action.payload.price,
                    ...state,
                    cartItems: existingCartItems,
                    totalItems: updatedCartCount, //state.totalItems ,
                    totalPrice: updatedTotalAmount,
                    shoppingCartId: state.shoppingCartId,
                };
            } else {
                // Example: You might use fetch or an API call to add the item to the server
                //addToServerCart(action.payload);
                return {
                        ...state,
                        cartItems: [...state.cartItems, action.payload],
                        totalItems: updatedCartCount,//state.totalItems ,
                        totalPrice: updatedTotalAmount,
                        shoppingCartId: state.shoppingCartId,
                        };
            }
        case SET_CART_DATA:
            const cartItems = action.payload.cartItems;
            const totalPrice = cartItems.reduce((acc, item) => acc + item.salePrice * item.quantity, 0);
            const totalItems = cartItems.reduce((acc, item) => acc + item.quantity, 0);

            //console.log("in SET_CART_DATA " + JSON.stringify(cartItems), totalPrice, totalItems);
            return {
                ...state,
                cartItems: action.payload.cartItems,
                totalItems: totalItems,
                totalPrice: totalPrice,
                shoppingCartId: state.shoppingCartId,
            };

        case REMOVE_FROM_CART:
            const existingItems = [...state.cartItems];
            const itemIndex = existingItems.findIndex(item => item.productId === action.payload.productId);
            //console.log(action.payload.item.id + " in REMOVE_FROM_CART " + JSON.stringify(state.cartItems));
            const updatedTotalAmount2 = state.totalPrice - existingItems[itemIndex].salePrice;
            const updatedCartCount2 = state.totalItems - action.payload.quantity;
            console.log(state.totalPrice + " in REMOVE_FROM_CART " + existingItems[itemIndex].salePrice);
            const existingItem = existingItems[itemIndex];
            console.log(" Item to Remove in REMOVE_FROM_CART " + JSON.stringify(existingItem));
            if (existingItem.quantity > 1) {
                //state.cartItems[itemIndex].quantity -= 1; //dont manipulate state directly
                existingItems[itemIndex].quantity = existingItems[itemIndex].quantity - action.payload.quantity;
                console.log(" in REMOVE_FROM_CART "+ existingItems[itemIndex].quantity);
                return {
                    //cartItems: state.cartItems,
                    ...state,
                    cartItems: existingItems,
                    totalItems: updatedCartCount2,//state.totalItems - action.payload.quantity,
                    totalPrice: updatedTotalAmount2,
                    shoppingCartId: state.shoppingCartId,
                };
            }
            else {
                //remove item from the cart
                if (state.totalItems > 1) {
                    return {
                        ...state,
                        //cartItems: state.cartItems.filter(item => item.id !== action.payload.id), //dont manupulate state directly
                        cartItems: existingItems.filter(item => item.productId !== action.payload.productId),
                        totalItems: updatedCartCount2,//state.totalItems - 1,
                        totalPrice: updatedTotalAmount2,
                        shoppingCartId: state.shoppingCartId,
                    };
                }
                else {
                    return {
                        cartItems: [],
                        totalItems: 0,
                        totalPrice: 0,
                        shoppingCartId: 0,
                    }
                }
            }
            
        default:
            return state;
    }
};



const CartProvider = (props) => {
    const [cartState, cartDispatch] = useReducer(cartReducer, initialState);

    const fetchCartData = async () => {
        try {
            if (getAuthToken() !== null) {
                const response = await agent.Cart.retrieveCart();
                const cartData = response.data;
                cartDispatch({
                    type: SET_CART_DATA,
                    payload: {
                        cartItems: cartData.items,
                        totalItems: cartData.totalItems,
                        totalPrice: cartData.totalPrice,
                        shoppingCartId: cartData.shoppingCartId,
                    },
                });
            }
            else {
                cartDispatch({ type: SET_CART_DATA, payload: { cartItems: [], totalItems: 0, totalPrice: 0, shoppingCartId: 0 } });
            }
        } catch (error) {
            //console.error('Error fetching cart data:'+error);
        }
        /*
        try {
            const response = await agent.Cart.retrieveCart();
            const cartData = response.data;//response;//
            //console.log("after agent.Cart.retrieveCart " + JSON.stringify(cartData))
            // Dispatch an action to set the cart data in the state
            cartDispatch({
                type: SET_CART_DATA,
                payload: {
                    cartItems: cartData.items,//cartData.cartItems,
                    //totalItems: totlaItems,
                    //totalPrice: totalPrice,
                },
            });
        } catch (error) {
            if (getAuthToken() === null) {
                console.error('Error fetching cart data:');
                cartDispatch({ type: SET_CART_DATA, payload: { cartItems: [], totalItems: 0, totalPrice: 0 } });
            }
           // 
        }
        */
    };

    useEffect(() => {
        // Fetch cart data when the component mounts
       
        console.log("in useEffect CartProvider");
        fetchCartData();
    }, []); // Empty dependency array ensures the effect runs only once on mount


    const onAddToCartHandler = (item) => {
        //console.log("ADD_TO_CART: " + JSON.stringify(item));
        cartDispatch({
            type: ADD_TO_CART, payload: {
                shoppingCartItemId: item.shoppingCartItemId, productId: item.productId, productName: item.productName,
                sku: item.sku, price: item.price, salePrice: item.salePrice, wasPrice: item.wasPrice, imageUrl: item.imageUrl, quantity: item.quantity
            }
        });
    };

    const onRemoveFromCartHandler = (item, quantity) => {
        //console.log(quantity+" onRemoveFromCartHandler REMOVE_FROM_CART: " + JSON.stringify(item));
        cartDispatch({
            type: REMOVE_FROM_CART, payload: {
                shoppingCartItemId: item.shoppingCartItemId, productId: item.productId, productName: item.productName,
                sku: item.sku, price: item.price, salePrice: item.salePrice, wasPrice: item.wasPrice, imageUrl: item.imageUrl, quantity: item.quantity } });
    };

    const onClearCartHandler = () => {
        cartDispatch({ type: SET_CART_DATA, payload: { cartItems: [], totalItems: 0, totalPrice: 0 } });
    };

    const cartContext = {
        cartItems: cartState.cartItems,
        totalItems: cartState.totalItems,
        totalPrice: cartState.totalPrice,
        shoppingCartId: cartState.shoppingCartId,
        addToCart: onAddToCartHandler,
        clearCart: onClearCartHandler,
        removeFromCart: onRemoveFromCartHandler,
        retrieveCartData: fetchCartData,
    };
    return (
        <CartContext.Provider value={cartContext} > {props.children}</CartContext.Provider>
    )
}
export default CartProvider;
