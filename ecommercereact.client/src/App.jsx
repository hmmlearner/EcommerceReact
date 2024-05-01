import RootLayout from "./pages/Root";
import { RouterProvider, createBrowserRouter } from "react-router-dom";
import HomePage from "./pages/HomePage";
import CartProvider from "./context/CartProvider";
import CustomerProvider from "./context/CustomerProvider";
import Product, { loader as productLoader } from "./pages/product/Product";
import OrderConfirmation from "./pages/checkout/OrderConfirmation";
import Category, { loader as productsLoader } from "./pages/Category";
import NotFoundError from "./pages/errors/NotFoundError"
import './App.css'


const router = createBrowserRouter([
    {
        path: "",
        element: <RootLayout />,
        errorElement: <NotFoundError />,
        children: [
            { index: true, element: <HomePage /> },
            { path: "/category/:name", id: "category-products", element: <Category />, loader: productsLoader },
            { path: "/category/:name/:id", element: <Product />, loader: productLoader },
            { path: "/paymentconfirmation", element: <OrderConfirmation /> },
        ],
    },
]);








function App() {

    //require('react-dom');
    //window.React2 = require('react');
    //console.log(window.React1 === window.React2);


    return (
        <div className="">
            <CustomerProvider>
                <CartProvider>
                    <RouterProvider router={router} />
                </CartProvider>
            </CustomerProvider>
        </div>
    );
}

export default App;

