import Button from '@mui/material/Button';
const HeaderCartButton = ({onOpenModal}) => {
    const handleOpenCartClick = () => onOpenModal(true);
  return (
      <p>
          {/*<button onClick={handleOpenCartClick}>Cart ({cartQuantity})</button>*/}
          <Button onClick={handleOpenCartClick}>Cart</Button>
      </p>
  );
}

export default HeaderCartButton;
