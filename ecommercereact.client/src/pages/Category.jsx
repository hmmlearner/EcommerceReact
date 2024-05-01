import { useLoaderData } from "react-router-dom";
import ProductList from "./ProductList";
import agent from "../api/agent";

const Category = () => {
    console.log("in Catgory : ")
    const products = useLoaderData("category-products");
    console.log("in Catgory : "+products)

 /*   const { name } = useParams();
    console.log(name);
  const [products, setProducts] = useState([]);


    const fetchProductsAsync = async (name) => {
        console.log("fetchProductsAsync " + name);
        try {
            axios.get(`https://localhost:7056/api/category/${name}/products`)
                .then((response) => {
                    console.log(response.data);
                    console.log(response.status);
                    console.log(response.statusText);
                    console.log(response.headers);
                    console.log(response.config);
                });
        }
        catch (error) {
            console.log("Error " + error.message);
        }
    };

    useEffect(() => {
        console.log("in effect "+name);
      fetchProductsAsync(name);
  }, []);*/

  return <ProductList products={products} />;

};

export default Category;

/*
export async function loader({ request, params }) {
    let eod;

       await axios.get(`https://localhost:7056/api/category/${params.name}/products`)
            .then((response) => {
                console.log(response.data.data);
                eod= response.data.data;
                //console.log(response.status);
                //console.log(response.statusText);
                //console.log(response.headers);
                //console.log(response.config);
            })
            .catch(function (error) {
                console.log("Error " + error.message);
                return error
            });
    return eod;
}
*/


export async function loader({ request, params }) {
    let eod;


    await agent.Catalogue.details(params.name)
        .then((response) => {
            console.log("PRD DATA " + response.data);
                eod = response.data;
                //console.log(response.status);
                //console.log(response.statusText);
                //console.log(response.headers);
                //console.log(response.config); })
        //return eod;
    })
         .catch(function (error) {
            console.log("Error " + error.message);
            return error
        });
    return eod;
}

