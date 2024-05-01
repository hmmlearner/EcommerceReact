import { useReducer, useEffect } from 'react';
import CustomerContext from './customer-context';
import { getAuthToken } from "../utils/AuthService";
import agent from "../api/agent";

// Define the initial state
const initialState = {
    loggedIn: false,
    name: ""
};

//Define customer actions
const LOG_IN = 'LOG_IN';
const LOG_OUT = 'LOG_OUT';
const SET_CUSTOMER_DATA = 'SET_CUSTOMER_DATA';

//create customer reducer
const customerReducer = (state, action) => {
    console.log(action.type + " in customerReducer " + JSON.stringify(action.payload));
    switch (action.type) {
        case LOG_IN:
            console.log(action.payload.name+" LOG_IN " + action.payload.loggedIn );
            return {
                ...state,
                loggedIn: action.payload.loggedIn,
                name: action.payload.name
            };  
        case SET_CUSTOMER_DATA:
            
            //console.log("in SET_CART_DATA " + JSON.stringify(cartItems), totalPrice, totalItems);
            return {
                ...state,
                loggedIn: action.payload.loggedIn,
                name: action.payload.name,
            };

        case LOG_OUT:
            return {
                ...state,
                loggedIn: false,
                name: ""
            };
        default:
            return state;
    }
};


 /*    //create a function to call login api
const authenticateCustomer = async  (email, password) => {
    // Simulated API call or fetch to add the item to the server
    let eod;
        try {
        const response = await axios.post(`https://localhost:7056/api/customer/Login?username=${email}&password=${password}`);
        const data = response.data;
        eod = data;
        console.log('Data from async/await:', data);
        return eod;

        // You can work with the data here
    } catch (error) {
        console.error('Error:', error);
        return error;
    }  
};*/

//create customer provider with useReducer hook
const CustomerProvider = (props) => {
    const [customerState, dispatchCustomerAction] = useReducer(customerReducer, initialState);


    const fetchCustomerData = async () => {
        try {
            console.log("in useEffect getAuthToken() " + getAuthToken()); 
            if (getAuthToken() !== null) {
                console.log("in useEffect AFTER getAuthToken() check " + getAuthToken()); 
                const response = await agent.Authenticate.retrieveCustomer();
                const customerData = response.data;
                console.log("in useEffect fetchCustomerData response " + customerData);
                dispatchCustomerAction({
                    type: SET_CUSTOMER_DATA,
                    payload: {
                        loggedIn: customerData.name !== null ? true :false,
                        name: customerData.name,
                    },
                });
            }
            else {
                dispatchCustomerAction({ type: SET_CUSTOMER_DATA, payload: { loggedIn: false, name: "" } });
            }
        } catch (error) {
            console.error('Error fetchCustomer data:'+error);
        }

    };

    useEffect(() => {
        // Fetch customer data when the component mounts

        console.log("in useEffect fetchCustomerData");
        fetchCustomerData();
    }, []); // Empty dependency array ensures the effect runs only once on mount

    const logInHandler = (loggedIn, name) => {
        console.log(name+" logInHandler " + loggedIn);
        //call login api
        dispatchCustomerAction({ type: LOG_IN, payload: { loggedIn: loggedIn, name: name } });

        console.log("logInHandler " + customerState.loggedIn);
    };

    const logOutHandler = () => {
        //call logout api
        console.log("logOutHandler");
        dispatchCustomerAction({ type: LOG_OUT });
    };

    const customerContext = {
        loggedIn: getAuthToken()!== null? true : customerState.loggedIn,
        name: customerState.name,
        logIn: logInHandler,
        logOut: logOutHandler
    };

    return (
        <CustomerContext.Provider value={customerContext}>
            {props.children}
        </CustomerContext.Provider>
    );
};

export default CustomerProvider;