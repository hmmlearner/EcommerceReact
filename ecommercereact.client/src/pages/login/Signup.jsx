//create Cart component to display cart page in reactimport React from 'react';

import { useContext, useState } from 'react';
import CustomerContext from '../../context/customer-context';
import { setAuthToken } from "../../utils/AuthService";
import agent from "../../api/agent";
import CartContext from "../../context/cart-context";
import classes from "./Signup.module.css";

const Signup = ({ signupClose }) => {
    const customerCtx = useContext(CustomerContext);
    console.log(customerCtx.loggedIn, customerCtx.name);
    const cartCtx = useContext(CartContext);

    const [formData, setFormData] = useState({
        email: '',
        password: '',
        name: '',
        streetAddress: '',
        city: '',
        state: '',
        postalcode: '',
        country: '',
    });

    // Handler function to update form data on input change
    const handleInputChange = (event) => {
        const { name, value } = event.target;
        setFormData({
            ...formData,
            [name]: value,
        });
    };

    const formDataToHTMLFormData = (formData) => {
        const formDataObj = new FormData();
        for (const key in formData) {
            formDataObj.append(key, formData[key]);
        }
        return formDataObj;
    };

    // Handler function to handle form submission
    const handleSubmit = async (event) => {
        event.preventDefault();
        const htmlFormData = formDataToHTMLFormData(formData);
        // Add your logic for handling the sign up here
        let data;
        try {
            const responsePromise = await customerSignUp(htmlFormData);
            console.log('Data outside async/await:', responsePromise);
            data = await responsePromise;
            console.log('Data outside data:', data);

        } catch (error) {
            console.error('Error outside async/await:', error);
            return error;
        }

        setAuthToken(data.data.token);
        customerCtx.logIn(data.success, data.name);
        cartCtx.retrieveCartData();
        signupClose();
    };



    //create a function to call login api
    const customerSignUp = async (htmlFormData) => {
        // Simulated API call or fetch to add the item to the server
        try {
            const responsePromise = agent.Authenticate.signup(htmlFormData);
            const response = await responsePromise;
            console.log('agent.Authenticate.signu:', response);
            return response;
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
                        <input
                            type="text"
                            id="name"
                            name="name"
                            placeholder="Name"
                            value={formData.name}
                            onChange={handleInputChange}
                            className={classes.inputField}
                            required
                        />
                    </div>
                    <div>
                        <input
                            type="text"
                            id="email"
                            name="email"
                            placeholder="Username"
                            value={formData.email}
                            onChange={handleInputChange}
                            className={classes.inputField}
                            required
                        />
                    </div>

                    <div>
                        <input
                            type="password"
                            id="password"
                            name="password"
                            placeholder="Password"
                            value={formData.password}
                            onChange={handleInputChange}
                            className={classes.inputField}
                            required
                        />
                    </div>
                    <div>
                        <input
                            type="text"
                            id="streetAddress"
                            name="streetAddress"
                            placeholder="Street Address"
                            value={formData.streetAddress}
                            onChange={handleInputChange}
                            className={classes.inputField}
                            required
                        />
                    </div>
                    <div>
                        <input
                            type="text"
                            id="city"
                            name="city"
                            placeholder="City"
                            value={formData.city}
                            onChange={handleInputChange}
                            className={classes.inputField}
                            required
                        />
                    </div>
                    <div>
                    <select
                            id="state"
                            name="state"
                            placeholder="State"
                            value={formData.state}
                            onChange={handleInputChange}
                            className={classes.inputField}
                        required
                        >
                            <option value="">Select State</option>
                            <option value="ACT">ACT</option>
                            <option value="NSW">NSW</option>
                            <option value="NT">NT</option>
                            <option value="QLD">QLD</option>
                            <option value="TAS">TAS</option>
                            <option value="VIC">VIC</option>
                            <option value="WA">WA</option>
                    </select>
                    </div>
                    <div>
                        <input
                            type="text"
                            id="postalcode"
                            name="postalcode"
                            placeholder="Postal Code"
                            value={formData.postalcode}
                            onChange={handleInputChange}
                            className={classes.inputField}
                            required
                        />
                    </div>
                    <div>
                        <input
                            type="text"
                            id="country"
                            name="country"
                            placeholder="Country"
                            value={formData.country}
                            onChange={handleInputChange}
                            className={classes.inputField}
                            required
                        />
                    </div>
                    <div>
                        <input
                            type="text"
                            id="phone"
                            name="phone"
                            placeholder="Phone"
                            value={formData.phone}
                            onChange={handleInputChange}
                            className={classes.inputField}
                            required
                        />
                    </div>
                    <button className={classes.SignupButton} type="submit">Sign Up</button>
                </form>}

        </div>
    );
};

export default Signup;
