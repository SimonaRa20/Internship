import React, { useState } from 'react';
import { Link, useNavigate } from 'react-router-dom';
import { useSelector, useDispatch } from 'react-redux';
import { Box, CardMedia, Typography, Button, IconButton, Input, Grid, Divider } from '@mui/material';
import CloseIcon from '@mui/icons-material/Close';
import ArrowBackIcon from '@mui/icons-material/ArrowBack';
import { removeFromCart, updateCartItem } from '../actions/CartActions';
import { RootState } from '../type/productTypes';

const ShoppingCart: React.FC = () => {
  const cartItems = useSelector((state: RootState) => state.cart);
  const dispatch = useDispatch();
  const [amountError, setAmountError] = useState<string | null>(null);
  
  const navigate = useNavigate();
  
  const handleRemoveItem = (productId: number) => {
    dispatch(removeFromCart(productId));
  };

  const handleAmountChange = (productId: number, newAmount: number) => {
    if (newAmount < 1 || newAmount > 100) {
      setAmountError('Amount must be between 1 and 100');
      return;
    }
    setAmountError(null);
    dispatch(updateCartItem({ productId, amount: newAmount }));
  };

  const goBack = () => {
    navigate(-1);
  };

  const getTotalCost = () => {
    let totalCost = 0;
    cartItems.forEach((cartItem) => {
      totalCost += cartItem.amount * cartItem.product.price;
    });
    return totalCost;
  };

  return (
    <Box style={{ paddingTop: 6 }}>
      <IconButton onClick={goBack} style={{ position: 'absolute', top: '5rem', left: '1rem', padding: '8px' }}>
  <ArrowBackIcon style={{ fontSize: '20px' }} />
</IconButton>

      {cartItems.length === 0 ? (
        <Typography variant="h6" align="center">Your cart is empty.</Typography>
      ) : (
        <Box>
          <Grid container spacing={2}>
            {cartItems.map((cartItem) => (
              <Grid item xs={12} key={cartItem.product.id}>
                <Box style={{ justifyContent: "center", alignItems: "center", display: "flex", width: "100%" }}>
                  <Box
                    display="flex"
                    alignItems="center"
                    justifyContent="space-between"
                    position="relative"
                    border="1px solid #ddd"
                    padding="1rem"
                    width="50%"
                  >
                    <Grid item xs={4} sm={3}>
                      <CardMedia
                        component="img"
                        height="100"
                        image={require(`../data/${cartItem.product.image}`)}
                        alt={cartItem.product.title}
                      />
                    </Grid>
                    <Grid item xs={8} sm={9} paddingLeft={2}>
                      <Box display="flex" flexDirection="column">
                        <Typography variant="h6">
                          {cartItem.product.title}
                        </Typography>
                        <Box display="flex" alignItems="center">
                          <Typography variant="h6">
                            {cartItem.product.price}$
                          </Typography>
                          <Input
                            type="number"
                            value={cartItem.amount}
                            onChange={(e) => handleAmountChange(cartItem.product.id, parseInt(e.target.value))}
                            style={{ width: '4rem', marginLeft: '1rem', border: '1px solid #ddd' }}
                          />
                        </Box>
                        {amountError && (
                          <Typography variant="caption" color="error">
                            {amountError}
                          </Typography>
                        )}
                      </Box>
                    </Grid>
                    <IconButton
                      edge="start"
                      onClick={() => handleRemoveItem(cartItem.product.id)}
                      style={{ position: 'absolute', top: '0', right: '0' }}
                    >
                      <CloseIcon />
                    </IconButton>
                  </Box>
                </Box>
              </Grid>
            ))}
          </Grid>
          <Divider style={{ paddingTop: "2rem", justifyContent: "center", alignItems: "center", display: "flex" }} />
          <Typography variant="h5" align="center">
            Total Cost: ${getTotalCost().toFixed(2)}
          </Typography>
          <Box style={{ display: 'flex', justifyContent: 'center' }}>
            <Link to="/checkout" style={{ textDecoration: 'none', color: 'black', marginTop: '1rem' }}>
              <Button color="inherit" variant="contained">
                Checkout
              </Button>
            </Link>
          </Box>
        </Box>
      )}
    </Box>
  );
};

export default ShoppingCart;
