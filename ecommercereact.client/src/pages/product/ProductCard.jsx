/*import React from "react";*/
import { useLocation, useNavigate } from 'react-router-dom';
import classes from "./ProductCard.module.css";
import {
  Button,
  Typography,
  CardMedia,
  CardContent,
  CardActions,
  Card,
} from "@material-ui/core";

const ProductCard = ({ product }) => {
    const location = useLocation();
    let navigate = useNavigate();
    const currentPath = location.pathname // to get current route
    const handleClick = (e, prodid) => {
        e.preventDefault();
        navigate(`${currentPath}/${prodid}`)
    }



    console.log("inside productcard " + `/public/productimgs/${product.imageUrl}`);//JSON.stringify(product));
    return (

        <Card className={classes.productcard}>
      <CardMedia
                sx={{ height: 140 }}
                image="/static/images/cards/contemplative-reptile.jpg"
                title={product.title}
      />
      <CardContent>
        <Typography gutterBottom variant="h5" component="div">
                    {product.title}
                </Typography>
                <CardMedia
                    className={classes.media}
                    component="img"
                    src={`/public/productimgs/${product.imageUrl}`}
                    alt={product.title}
                    title={product.title}
                />
  
      </CardContent>
      <CardActions>
                <Button size="small" href={`${currentPath}/${product.id}`} onClick={(e) => handleClick(e, product.id)} >View</Button> 
        <Button size="small">Learn More</Button>
      </CardActions>
    </Card>
  );
};

export default ProductCard;
