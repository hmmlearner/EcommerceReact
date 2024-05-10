import React, { useState, useEffect, useRef, useContext } from "react";
import classes from "./Header.module.css";
import { NavLink, useLocation } from "react-router-dom";
import CartModal from "../../pages/cart/CartModal";
import Button from '@mui/material/Button';
/*import { createTheme, ThemeProvider } from "@mui/material/styles";*/
import CartContext from "../../context/cart-context";
import CustomerContext from "../../context/customer-context";
import LoginModal from "../../pages/login/LoginModal";
import SignupModal from "../../pages/login/SignupModal";
import { removeAuthToken } from "../../utils/AuthService";
import agent from "../../api/agent";
import Logo from "../../assets/logo.png";

//const theme = createTheme({
//    palette: {
//        primary: blue,
//        secondary: "#090150FF"
//    }
//});

const Header = () => {

    //const customclasses = {

    //    inactiveLink: 'inactive-link-class',
    //    active: 'active-class',
    //};


    //const { loggedIn, name } = useContext(CustomerContext);
    const customerCtx = useContext(CustomerContext);
    const cartCtx = useContext(CartContext);

    console.log("CustomerContext in Header " + JSON.stringify(customerCtx));
    console.log("cartCtx in Header " + JSON.stringify(cartCtx));
    const loggedIn = customerCtx.loggedIn;
    const name = customerCtx.name;
    const [categories, setCategories] = useState([]);

    const modal = useRef();
    const loginModal = useRef();
    const signUpModal = useRef();
    const fetchCategoriesAsync = async () => {
        console.log("fetchCategoriesAsync ");

       try {
            const response = await agent.Category.list()
           console.log(response);
           setCategories(response.data);
        } catch (error) {
            console.error(error);
        }

    };

    useEffect(() => {
         fetchCategoriesAsync();
    }, []);

    const location = useLocation();
    console.log(location.pathname +" "+ cartCtx.totalItems);
    const isNavLinkActive = (match, location) => !match;
    const openCartHandler = () => {
        // Call the onOpen method from the child component
        modal.current.onOpen();
    };
    const openLoginHandler = () => {
        // Call the onOpen method from the child component
        loginModal.current.onOpen();
    };
    const openSignUpHandler = () => {
        // Call the onOpen method from the child component
        signUpModal.current.onOpen();
    };

    const logoutHandler = () => {
        removeAuthToken();
        // Call the onOpen method from the child component
        customerCtx.logOut();
        // Update CartCtx here to clear cart
        cartCtx.clearCart();
    }



    return (
        <>

            <CartModal ref={modal} />
            <LoginModal ref={loginModal} />
            <SignupModal ref={signUpModal} />
            <header className={classes.header}>
                <div className={classes.title}>
                    <NavLink className={classes.logoName} end to="/">
                        <img src={Logo} alt="EcommerceStore" className={classes.headerimage} />
                        <h1>Ecommerce Store</h1>
                    </NavLink>
                </div>
                <div className={classes.headerleft}>
                    {loggedIn ? <div>Hi, {customerCtx.name} <Button color="secondary" onClick={logoutHandler}> Log out</Button></div> : <div><Button color="secondary" onClick={openLoginHandler}> Login</Button>/<Button color="secondary" onClick={openSignUpHandler}>Sign Up</Button></div>}               
                    {location.pathname !== "/paymentconfirmation" ? <Button color="secondary" onClick={openCartHandler}>Cart ({cartCtx.totalItems})</Button> : ""}
                </div>
            </header> 
            <nav className={classes.nav}>
                    <ul className={classes.ul}>

                    {categories.map(item => (<li className={classes.li} key={item.id}>
                            <NavLink className={({ isActive }) => isActive ? classes.active : undefined} end to={`/category/${item.name}`}>{item.name}</NavLink>
                        </li>))}
                    </ul>
                </nav>
        






        </>
    );
}

export default Header;

