import React, { createContext } from 'react';

const CustomerContext = createContext({
    loggedIn: false,
    name: "",
    logIn: (cloggedIn, name) => { },
    logOut: () => { }
});

export default CustomerContext;
