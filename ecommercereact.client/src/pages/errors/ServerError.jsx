import { useLocation } from "react-router-dom";
import { Container, Paper, Typography, Divider } from "@material-ui/core";

const ServerError = () => {
  const { state } = useLocation();
  return (
    <Container component={Paper}>
      {state.error ? (
        <>
          <Typography variant="h3" gutterBottom>
            Error Title
          </Typography>
          <Divider />
          <Typography variant="body1" gutterBottom>
            Internal Server Error
          </Typography>
        </>
      ) : (
        <>
          <Typography variant="h5">Server Error</Typography>
        </>
      )}
    </Container>
  );
};

export default ServerError;
