import React, { useState } from 'react';
import { Link } from 'react-router-dom';
import { useSelector, useDispatch } from 'react-redux';
import { Box, Grid, CardMedia, Typography, Button, Input, Divider, Dialog, DialogActions } from '@mui/material';
import { cleanCart } from '../actions/CartActions';
import { RootState } from '../type/productTypes';


const Checkout: React.FC = () => {
    const cartItems = useSelector((state: RootState) => state.cart);
    const dispatch = useDispatch();

    const [email, setEmail] = useState<string>('');
    const [confirmEmail, setConfirmEmail] = useState<string>('');
    const [fullName, setFullName] = useState<string>('');
    const [emailError, setEmailError] = useState<boolean>(false);
    const [confirmEmailError, setConfirmEmailError] = useState<boolean>(false);
    const [fullNameError, setFullNameError] = useState<boolean>(false);
    const [orderSuccess, setOrderSuccess] = useState<boolean>(false);

    const handleOrder = () => {
        const emailRegex = /^[a-zA-Z0-9._-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,4}$/;
        if (!emailRegex.test(email)) {
            setEmailError(true);
            return;
        }

        if (email !== confirmEmail) {
            setConfirmEmailError(true);
            return;
        }

        const nameRegex = /^[A-Za-z]+\s[A-Za-z]+$/;
        if (!nameRegex.test(fullName)) {
            setFullNameError(true);
            return;
        }

        setOrderSuccess(true);
    };

    const getTotalCost = (): string => {
        const totalCost = cartItems.reduce((acc, cartItem) => {
            return acc + cartItem.amount * cartItem.product.price;
        }, 0);
    
        return totalCost.toFixed(2);
    };
    

    return (
        <Box style={{ paddingTop: 6 }}>
            <Box>
                {cartItems.length === 0 ? (
                    <Typography variant="h6" align="center">Your cart is empty.</Typography>
                ) : (
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
                                                    <Typography variant="body2">
                                                        Price: {cartItem.product.price}$
                                                    </Typography>
                                                </Box>
                                                <Box display="flex" alignItems="center">
                                                    <Typography variant="body2">
                                                        Amount: {cartItem.amount}
                                                    </Typography>
                                                </Box>
                                                <Box display="flex" alignItems="center">
                                                    <Typography variant="body2">
                                                        Total cost: {cartItem.product.price * cartItem.amount}$
                                                    </Typography>
                                                </Box>
                                            </Box>
                                        </Grid>
                                    </Box>
                                </Box>
                            </Grid>
                        ))}
                    </Grid>
                )}
            </Box>
            <Divider style={{ paddingTop: '2rem' }} />
            <Typography variant="h5" align="center">
                Total Cost: ${getTotalCost()}
            </Typography>
            <Box style={{ justifyContent: "center", alignItems: "center", display: "flex", width: "100%" }}>
                <Box style={{ width: "50%" }}>
                    <Input
                        type="email"
                        value={email}
                        onChange={(e) => {
                            setEmail(e.target.value);
                            setEmailError(false);
                        }}
                        placeholder="Email"
                        fullWidth
                        error={emailError}
                        style={{ marginBottom: '1rem' }}
                    />
                    {emailError && <Typography variant="caption" color="error">Invalid email address</Typography>}
                    <Input
                        type="email"
                        value={confirmEmail}
                        onChange={(e) => {
                            setConfirmEmail(e.target.value);
                            setConfirmEmailError(false);
                        }}
                        placeholder="Confirm Email"
                        fullWidth
                        error={confirmEmailError}
                        style={{ marginBottom: '1rem' }}
                    />
                    {confirmEmailError && <Typography variant="caption" color="error">Emails do not match</Typography>}
                    <Input
                        type="text"
                        value={fullName}
                        onChange={(e) => {
                            setFullName(e.target.value);
                            setFullNameError(false);
                        }}
                        placeholder="Full Name"
                        fullWidth
                        error={fullNameError}
                        style={{ marginBottom: '1rem' }}
                    />
                    {fullNameError && <Typography variant="caption" color="error">Invalid full name format</Typography>}
                </Box>
            </Box>
            <Box style={{ display: 'flex', justifyContent: 'center', marginTop: '1rem' }}>
                <Button variant="contained" color="inherit" onClick={handleOrder}>
                    Place Order
                </Button>
            </Box>
            <Dialog open={orderSuccess} >
                <Typography variant="h6" align="center">Thank you for your order!</Typography>
                <DialogActions style={{ display: 'flex', justifyContent: 'center', marginTop: '1rem' }}>
                    <Link to="/" style={{ textDecoration: 'none' }}>
                        <Button variant="contained" color="inherit" onClick={() => {
            dispatch(cleanCart());
            setOrderSuccess(false);
        }}>
                            Home Page
                        </Button>
                    </Link>
                </DialogActions>
            </Dialog>
        </Box>
    );
};

export default Checkout;
