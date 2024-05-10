//create Cart component to display cart page in reactimport React from 'react';

import { useContext, useState, useEffect } from 'react';
import CustomerContext from '../../context/customer-context';
import { setAuthToken, getAuthToken } from "../../utils/AuthService";
import agent from "../../api/agent";
import CartContext from "../../context/cart-context";
import classes from "./Login.module.css";

const Login = ({ loginClose }) => {
    const customerCtx = useContext(CustomerContext);
    console.log(customerCtx.loggedIn, customerCtx.name);
    const cartCtx = useContext(CartContext);

    const [formData, setFormData] = useState({
        username: '',
        password: '',
    });

    // Handler function to update form data on input change
    const handleInputChange = (event) => {
        const { name, value } = event.target;
        setFormData({
            ...formData,
            [name]: value,
        });
    };
    // Handler function to handle form submission
    const handleSubmit = async (event) => {
        event.preventDefault();
        // Add your logic for handling the login here
        let data;
        try {
            const responsePromise = await authenticateCustomer(formData.username, formData.password);
            data = await responsePromise;
            console.log('Data outside data:', JSON.stringify(data));

        } catch (error) {
            console.error('Error outside async/await:', error);
            return error;
        }

        setAuthToken(data.data.token);
        customerCtx.logIn(data.success, data.data.name);
        cartCtx.retrieveCartData();
        loginClose();
    };





    //create a function to call login api
    const authenticateCustomer = async (email, password) => {
        // Simulated API call or fetch to add the item to the server
        try {
            const response = await agent.Authenticate.login({ email, password });
            const data = await response;
            return data;
        } catch (error) {
            console.log("Error:", error.message);
            return error;
        }

    };


    return (
        <div id="customer">
           
           
            {/* Add your cart content here */}
            {customerCtx.loggedIn ? <p>Hello: {customerCtx.name}</p>
                : <form onSubmit={handleSubmit}>
                    <div>
                {/*        <label htmlFor="username">Username:</label>*/}
                        <input 
                            type="text"
                            id="username"
                            name="username"
                            value={formData.username}
                            onChange={handleInputChange}
                            placeholder="Username"
                            className={classes.inputField}
                            required
                        />
                    </div>

                    <div>
                     {/*   <label htmlFor="password">Password:</label>*/}
                        <input
                            type="password"
                            id="password"
                            name="password"
                            value={formData.password}
                            onChange={handleInputChange}
                            placeholder="Password"
                            className={classes.inputField}
                            required
                        />
                    </div>

                    <button className={classes.loginButton} type="submit">Login</button>
                </form>}
            
        </div>
    );
};

export default Login;
