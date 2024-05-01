/* eslint-disable react/no-unescaped-entities */
import { Link } from "react-router-dom";
import { Typography, Divider, Container, Paper, Button } from "@material-ui/core";

const NotFoundError = () => {
 /*   const error = useRouterError();*/
  return (
    <Container component={Paper}>
      <Typography variant="h5" gutterBottom>
              Oops! can't find what you looking for.
     {/*         <p>{error.statusText || error.message} </p>*/}
        <Divider />
        <Button fullWidth component={Link} to={"/category"}>
          Go back
        </Button>
      </Typography>
    </Container>
  );
};

export default NotFoundError;
