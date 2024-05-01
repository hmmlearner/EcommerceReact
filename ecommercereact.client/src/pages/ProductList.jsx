import React from "react";
import ProductCard from "./product/ProductCard";
import classes from "./ProductList.module.css";


const ProductList = ({ products }) => {
    console.log("in productlist " + products);
    return (
        <div className={classes.productcontainer}>

            {products.map((prod) => (
                <ProductCard key={prod.id} product={prod} />
            ))}
        </div>
    );
};

export default ProductList;
