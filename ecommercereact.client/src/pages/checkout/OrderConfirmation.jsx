import React, { useState, useEffect, useContext } from "react";
import CartContext from "../../context/cart-context";
import agent from "../../api/agent";
//import { loadStripe } from "@stripe/stripe-js";
//import {
//    Elements,
//    useStripe,
//    useElements
//} from "@stripe/react-stripe-js";

//const stripePromise = loadStripe("pk_test_51OZ1gKJokwjQRWN4CgclxHpxyekj4oEX1ppaxVZrdJulPYUeobnVNTi9MAiOxGrDMJlLLSNw9xlr4C9LirR4xoHA00s1EiLVXy");

const OrderConfirmation = () => {
    //const stripe = useStripe();
    //const elements = useElements();
    const [message, setMessage] = useState(null);
    const cartCtx = useContext(CartContext);

   // setMessage(clientSecret);
   // console.log(clientSecret);

    const orderConfirmation = async (sessionid, orderId) => {

        try {
            const responsePromise = await agent.Checkout.confirmation({ orderId, sessionid });
            const response = await responsePromise;
            console.log('confirmed order:', response);
            setMessage("Order Confirmation " + orderId);
            //return response;
        } catch (error) {
            console.log("Error:", error.message);
            return error;
        }

    };
    cartCtx.clearCart();

    useEffect(() => {
        const params = new URLSearchParams(window.location.search);
        const sessionid = params.get(
            "session_id"
        );
        const orderId = params.get(
            "orderId"
        );

        orderConfirmation(sessionid, orderId);



    }, []);

    return (
        <div>
            {message && <div id="payment-message">{message}</div>}
        </div>
    );
};

/*
    //const { name } = useParams();
    useEffect(() => {
        if (!stripe) {
            return;
        }

        const clientSecret = new URLSearchParams(window.location.search).get(
            "payment_intent_client_secret"
        );

        if (!clientSecret) {
            return;
        }

        stripe.retrievePaymentIntent(clientSecret).then(({ paymentIntent }) => {
            switch (paymentIntent.status) {
                case "succeeded":
                    setMessage("Payment succeeded!");
                    break;
                case "processing":
                    setMessage("Your payment is processing.");
                    break;
                case "requires_payment_method":
                    setMessage("Your payment was not successful, please try again.");
                    break;
                default:
                    setMessage("Something went wrong.");
                    break;
            }
        });
    }, [stripe]);


    //const appearance = {
    //    theme: 'stripe',
    //};
    //const options = {
    //    clientSecret,
    //    appearance,
    //};

    return (
         
            <Elements stripe={stripePromise}>
                {message && <div id="payment-message">{message}</div>}
            </Elements>
        */




export default OrderConfirmation;


