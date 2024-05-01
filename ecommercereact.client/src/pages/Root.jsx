import { Outlet } from "react-router-dom";
import Header from "../components/Layout/Header";
import axios from "axios";
import classes from "./Root.module.css";

const RootLayout = () => {
   // const categories = useRouteLoaderData();
    //console.log("in RootLayout categories " + categories)
    return (
        <>
            <Header />
            <main className={classes.container}>
                <Outlet/>
            </main>
        </>
    );
};

export default RootLayout;

export async function categoriesLoader({ request, params }) {
    try {
        axios.get(`https://localhost:7056/api/category/all`)
            .then((response) => {
                console.log('response.data ' + response.data.data);
                console.log(response.data.statusCode);
                console.log(response.data);
                console.log(response.headers);
                console.log(response.config);
                //setCategories(response.data.data);
                //console.log("in fetchCategoriesAsync " + categories);
                return response.data.data;
            })
            .catch(error => console.error('Error fetching categories', error.message));
    }
    catch (error) {
        console.log("Error " + error.message);
    }

}