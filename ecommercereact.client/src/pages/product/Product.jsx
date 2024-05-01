import  {useContext} from "react";
import { useLoaderData } from "react-router-dom";
import classes from "./Product.module.css";
import CartContext from "../../context/cart-context";
import agent from "../../api/agent";


//create product component to display product page

const Product = () => {

    const { addToCart } = useContext(CartContext);
    const product = useLoaderData();


    // Handler function to handle form submission
    const addToCartHandler = async (product, quantity) => {
       
        // Add your logic for handling the login here
        const productId = product.id;
        try {
            const responsePromise = agent.Cart.addtocart({ productId, quantity });
            const response = await responsePromise;
            console.log('addtocart outside async/await:', response);
            //return response;
        } catch (error) {
            console.log("Error:", error.message);
            return error;
        }


        addToCart({ shoppingCartItemId: 0, productId: product.id, productName: product.title, sku: product.sku, price: product.price, salePrice: product.salePrice, wasPrice: product.wasPrice, imageUrl: product.imageUrl, quantity: quantity });

        //console.log('Login submitted with:' + customerCtx.loggedIn, customerCtx.name);
        //loginClose();
    };






    return (

        <article className={classes.articlecontent}>
      <img src={`/public/productimgs/${product.imageUrl}`} alt={product.title} />
            <div className={classes.productcontent}>
        <div>
          <h3>{product.title}</h3>
                    <p className={classes.productPrice}>${product.price}</p>
          <p>{product.description}</p>
        </div>
                <p className='product-actions'>
                    <button className={classes.button} onClick={(e) => { e.preventDefault(); addToCartHandler(product, 1) }} >Add to Cart</button>
        </p>
      </div>
    </article>
    );
};

export default Product;

// create loader function to fetch product data from product controller in Ecommerce.API
export async function loader({ request, params }) {
  /*  let eod;
    console.log("in loader");
    console.log(params.id);
    await axios.get(`https://localhost:7056/api/product/${params.id}`)
        .then((response) => {
            console.log(response.data.data);
            eod = response.data.data;
        })
        .catch(function (error) {
            console.log("Error " + error.message);
            return error
        });
    return eod;*/

    let eod;
    console.log("in loader");
    console.log(params.id);
    await agent.Product.details(params.id)
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

