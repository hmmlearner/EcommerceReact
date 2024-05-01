import axios from "axios";
import { getAuthToken } from "../utils/AuthService";

/*import { Agent, request } from "https";*/
/*import { router } from "../router/Routes";*/

const sleep = () => new Promise(resolve => setTimeout(resolve, 500));

axios.defaults.baseURL = "https://localhost:7076/api/";
axios.defaults.withCredentials = true;
const authToken = 'Bearer ' + getAuthToken();

const responseBody = (response) =>  response.data;

axios.interceptors.response.use(async response => {
    //await sleep();

    console.log('response from API call ' + JSON.stringify(response));
        return response
    
}, (error) => {
    const { data, status } = error.response;
    switch (status) {
        case 400:
            if (data.errors) {
                const errorString = [];
                for (const key in data.errors) {
                    if (data.errors[key]) {
                        errorString.push(data.errors[key]);
                    }
                }
                throw errorString.flat(); // flattens array [[1,2],[3,4]] becomes [1,2,3,4]
            }
            break;
        case 500:
            router.navigate('/server-error', { state: { error: data } });
            break;
        default:

    }

    return Promise.reject(error.response);
})

const requests = {
    //get: (url) => axios.get(url, { timeout: 5000 }).then(responseBody => console.log(responseBody)),
    get: (url) => axios.get(url, { headers: { Authorization: authToken } }).then(responseBody ),
    // post: (url, body) => axios.post(url, body, { headers: { Authorization: authToken, 'Content-Type': 'application/json' }}).then(responseBody),
    post: (url, body) => axios.post(url, body, { headers: { Authorization: authToken, 'Content-Type': 'application/json' } }).then(responseBody),
    put: (url, body) => axios.put(url, body).then(responseBody),
    delete: (url) => axios.delete(url, { headers: { Authorization: authToken } }).then(responseBody),
}


const Catalogue = {
    list: () => {
        //console.log(`Name: ${name}`);
        //requests.get(`category/${name}`)
        requests.get("category/all")
    },
    details: (name) =>  requests.get(`category/${name}/products`) 
}

const Category = {
    list: () => {
        //console.log("Category list");
        return requests.get("category/all")
    },
    details: (name) => requests.get(`category/${name}/products`)
}

const Product = {
    list: () => {
        //console.log("Category list");
        requests.get("product/all")
    },
    details: (id) => requests.get(`product/${id}`)
}

const Authenticate = {
    login: ({email,password }) => {
        //console.log(email, password);
        var formdata = new FormData();
        //add three variable to form
        formdata.append("Username", email);
        formdata.append("Password", password);
        //return requests.post(`customer/Login?username=${email}&password=${password}`, formdata)
        //console.log("formdata  " + formdata.get("Username"));
        return requests.post("customer/login", formdata)
    },
    signup: (htmlFormData) => {

        //var formdata = new FormData();

        //formdata.append("username", email);
        //formdata.append("password", password);
         return requests.post("customer/CreateCustomer", htmlFormData)
    },
    retrieveCustomer: () => {
        console.log("retrieveCustomer " + authToken);

        return requests.get("customer/retrievecustomer")

    },
}

const Cart = {
    addtocart: ({ productId, quantity }) => {
        console.log(productId, quantity);
        var formdata = new FormData();
        //add three variable to form
        //formdata.append("email", email);
        //formdata.append("password", password);
        return requests.post(`cart/addToCart?productId=${productId}&quantity=${quantity}`, formdata)
    },
    retrieveCart: () => {
        console.log("retrieveCart " + authToken);
     
            return requests.get("cart/retrievecart")
       
    },
    removeFromcart: ({ productId, quantity }) => {
        //console.log(productId, quantity);
        var formdata = new FormData();
        //add three variable to form
        //formdata.append("email", email);
        //formdata.append("password", password);
        return requests.delete(`cart/deleteItem?productId=${productId}&quantity=${quantity}`, formdata)
    },

    //details: (id) => requests.get(`product/${id}`)
}

const Checkout = {
    submitpayment: () => {
        //console.log(productId, quantity);
        var formdata = new FormData();
        //add three variable to form
        //formdata.append("email", email);
        //formdata.append("password", password);
        return requests.post("order/OrderSubmit", formdata)
    },
    confirmation: ({ orderId, sessionid }) => {
        //console.log(productId, quantity);
        var formdata = new FormData();
        //add three variable to form
        //formdata.append("orderNumber", orderId);
        //formdata.append("sessionid", sessionid);
        return requests.post(`order/OrderConfirmation?orderNumber=${orderId}&sessionid=${sessionid}`, formdata)
    },
}

const agent = {
    Catalogue,
    Category,
    Product,
    Authenticate,
    Cart,
    Checkout
}

export default agent; 