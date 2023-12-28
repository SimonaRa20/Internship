import React, { useState } from 'react';
import { useParams, Link } from 'react-router-dom';
import { useDispatch } from 'react-redux';
import { Box, CardMedia, Typography, Button, TextField, Rating, Dialog, DialogActions, IconButton } from '@mui/material';
import CloseIcon from '@mui/icons-material/Close';
import { addToCart } from '../actions/CartActions';
import { Product } from '../type/productTypes';

interface ProductDetailProps {
  products: Product[];
}

const ProductDetail: React.FC<ProductDetailProps> = ({ products }) => {
  const { id } = useParams<{ id: string }>();
  const product = products.find((p) => p.id === parseInt(id || '', 10));
  const dispatch = useDispatch();
  const [amount, setAmount] = useState<number>(1);
  const [dialogOpen, setDialogOpen] = useState<boolean>(false);
  const [warningMessage, setWarningMessage] = useState<string>('');

  if (!product) {
    return <div>Product not found</div>;
  }

  const handleAmountWasChanged = (event: React.ChangeEvent<HTMLInputElement>) => {
    const newAmount = parseInt(event.target.value, 10);
    setAmount(newAmount);

    if (newAmount < 1 || newAmount > 100) {
      setWarningMessage('Please enter a quantity between 1 and 100.');
    } else {
      setWarningMessage('');
    }
  };

  const handleAddToCart = () => {
    if (amount > 0 && amount <= 100) {
      dispatch(addToCart({ product, amount }));
      setDialogOpen(true);
    } else {
      setWarningMessage('Invalid quantity. Please enter a quantity between 1 and 100.');
    }
  };

  const handleDialogClose = () => {
    setDialogOpen(false);
  };

  return (
    <Box style={{ paddingTop: 6, justifyContent: "center", alignItems: "center", display: "flex", width: "100%" }}>
      <Box style={{ width: "50%" }}>
        <Typography variant="h6" align="center">
          {product.title}
        </Typography>
        <CardMedia
          component="img"
          height="200"
          image={require(`../data/${product.image}`)}
          alt={product.title}
        />
        <Box style={{ display: 'flex', justifyContent: 'space-between' }}>
          <Typography variant="body2" align="center">
            {product.description}
          </Typography>
        </Box>
        <Box style={{ display: 'flex', justifyContent: 'space-between' }}>
          <Typography variant="h6" align="center">
            Amount:
          </Typography>
          <Typography variant="h6" color="textPrimary">
            {product.price}$
          </Typography>
        </Box>
        <Box style={{ display: 'flex', justifyContent: 'space-between' }}>
          <TextField size='small' type='number' value={amount} onChange={handleAmountWasChanged} inputProps={{ min: 1, max: 100 }} />
          <Rating name="read-only" value={product.rating} readOnly max={5} />
        </Box>
        <Typography color="error" variant="body2">
          {warningMessage}
        </Typography>
        <Box style={{ display: 'flex', justifyContent: 'center', paddingTop: '2rem' }}>
          <Button variant="contained" color="inherit" onClick={handleAddToCart}>
            Add to Cart
          </Button>
        </Box>
      </Box>
      <Dialog open={dialogOpen} onClose={handleDialogClose} maxWidth="sm" fullWidth>
        <IconButton
          edge="end"
          color="inherit"
          onClick={handleDialogClose}
          aria-label="close"
          sx={{ position: 'absolute', right: 8, top: 8, color: (theme) => theme.palette.grey[500] }}
        >
          <CloseIcon />
        </IconButton>

        <DialogActions style={{ display: 'flex', flexDirection: 'column', alignItems: 'center' }}>
          <Link to="/cart" style={{ textDecoration: 'none', color: 'black' }}>
            <Button color="inherit" variant="contained" onClick={handleDialogClose}>
              Go to Cart
            </Button>
          </Link>
          <Link to="/" style={{ textDecoration: 'none', color: 'black', marginTop: '1rem' }}>
            <Button color="inherit" variant="contained" onClick={handleDialogClose}>
              Continue to Shop
            </Button>
          </Link>
        </DialogActions>
      </Dialog>
    </Box>
  );
};

export default ProductDetail;
