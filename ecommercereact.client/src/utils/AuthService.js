// AuthService.js

// Function to store JWT token in localStorage
export const setAuthToken = (token) => {
    localStorage.setItem('jwtToken', token);
};

// Function to get JWT token from localStorage
export const getAuthToken = () => {
    return localStorage.getItem('jwtToken');
};

// Function to remove JWT token from localStorage (logout)
export const removeAuthToken = () => {
    localStorage.removeItem('jwtToken');
};
