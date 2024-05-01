//create Cart component to display cart page in reactimport React from 'react';
import classes from "./Cart.module.css";
import { useContext, useState, useEffect } from 'react';
import CartContext from '../../context/cart-context';
import agent from "../../api/agent";
//import { loadStripe } from "@stripe/stripe-js";
//import { Elements } from "@stripe/react-stripe-js";

//import CheckoutForm from "./CheckoutForm";

//const stripePromise = loadStripe("pk_test_51OZ1gKJokwjQRWN4CgclxHpxyekj4oEX1ppaxVZrdJulPYUeobnVNTi9MAiOxGrDMJlLLSNw9xlr4C9LirR4xoHA00s1EiLVXy");

const Cart = ({ cartClose }) => {
    const cartCtx = useContext(CartContext);
    console.log(cartCtx.cartItems, cartCtx.totalPrice, cartCtx.totalItems);
    const buildShoppingCart = { ProductTotal: cartCtx.totalItems, ShoppingCartId: cartCtx.shoppingCartId, CartTotal: cartCtx.totalPrice, Items: cartCtx.cartItems }

    console.log("Custom shoppingcart "+JSON.stringify(buildShoppingCart));

    //const [clientSecret, setClientSecret] = useState("");
    // Handler function to handle form submission
    const addToCartHandler = async (product, quantity) => {

        // Add your logic for handling the login here
        const productId = product.productId;
        try {
            const responsePromise = agent.Cart.addtocart({ productId, quantity });
            const response = await responsePromise;
            console.log('addtocart outside async/await:', response);
            //return response;
        } catch (error) {
            console.log("Error:", error.message);
            return error;
        }


        cartCtx.addToCart({ shoppingCartItemId: buildShoppingCart.ShoppingCartId, productId: product.productId, productName: product.productName, sku: product.sku, price: product.price, salePrice: product.salePrice, wasPrice: product.wasPrice, imageUrl: product.imageUrl, quantity: quantity });

        //console.log('Login submitted with:' + customerCtx.loggedIn, customerCtx.name);
        //loginClose();
    };

    const removeFromCartHandler = async (product, quantity) => {

       // console.log('removeFromCart :quantity ' + quantity);
        // Add your logic for handling the login here
        const productId = product.productId;
        try {
            const responsePromise = agent.Cart.removeFromcart({ productId, quantity });
            const response = await responsePromise;
           
            //return response;
        } catch (error) {
            console.log("Error:", error.message);
            return error;
        }

       
        cartCtx.removeFromCart({ shoppingCartItemId: buildShoppingCart.ShoppingCartId, productId: product.productId, productName: product.productName, sku: product.sku, price: product.price, salePrice: product.salePrice, wasPrice: product.wasPrice, imageUrl: product.imageUrl, quantity: quantity });

        //console.log('Login submitted with:' + customerCtx.loggedIn, customerCtx.name);
        if (buildShoppingCart.ProductTotal - quantity === 0) {
            cartClose();
        }
    };

    
    const handleCheckout = async (event) => {
        event.preventDefault();
        try {
            const responsePromise = agent.Checkout.submitpayment();
            const response = await responsePromise;
            console.log(response.data);
            window.location.href = response.data;
        } catch (error) {
            console.log("Error:", error.message);
            return error;
        }
    };

    
  return (
      <div id="cart">
          {buildShoppingCart.Items.length <= 0 ? (<p>The cart is empty.</p>)
              : (
                  <>
                      <ul className={classes.cart_items}>
                          {buildShoppingCart.Items.map((item) => {
                              console.log(JSON.stringify(item)); //console.log(item.salePrice.toFixed(2) );
                              const formattedPrice = `$${item.salePrice.toFixed(2)}`;
                              return (<li key={item.shoppingCartItemId}>
                                  <div className={classes.item}>

                                      <div className={classes.item_details}>
                                          <div className={classes.item_title}>{item.productName}</div>
                                          <div className={classes.item_price}>{formattedPrice}</div>
                                          <div className={classes.cart_item_actions}>
                                              <div className={classes.item_quantity}>
                                                  <button onClick={(e) => { e.preventDefault(); removeFromCartHandler(item, 1); } }>-</button>
                                                  <span>{item.quantity}</span>
                                                  <button onClick={(e) => { e.preventDefault(); addToCartHandler(item, 1); } }>+</button>
                                              </div>
                                              <button className={classes.item_remove} onClick={(e) => { e.preventDefault(); removeFromCartHandler(item, item.quantity); } }>Remove</button>
                                          </div>
                                      </div>
                                  </div>
                              </li>
                              );
                          })}

                      </ul>
                      <p className={classes.cart_total_price}>Cart Total: ${cartCtx.totalPrice.toFixed(2)}</p>
                      <form onSubmit={handleCheckout}>
                          <button className={classes.checkoutButton} type="submit">
                              Checkout
                          </button>
                      </form>
                  </>
          )}
    </div>
  );
};

export default Cart;
