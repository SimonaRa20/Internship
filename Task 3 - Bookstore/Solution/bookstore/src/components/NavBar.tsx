import * as React from 'react';
import { Link } from 'react-router-dom';
import { useSelector } from 'react-redux';
import { AppBar, Toolbar, IconButton, Grid, Menu, MenuItem, Badge, Box, CardMedia, Button } from '@mui/material';
import ShoppingCartIcon from '@mui/icons-material/ShoppingCart';
import MenuIcon from '@mui/icons-material/Menu';
import { RootState } from '../type/productTypes';

const NavBar: React.FC = () => {
    const cartItems = useSelector((state: RootState) => state.cart);
    const [anchorEl, setAnchorEl] = React.useState<null | HTMLElement>(null);

    const handleClick = (event: React.MouseEvent<HTMLElement>) => {
        setAnchorEl(event.currentTarget);
    };

    const handleClose = () => {
        setAnchorEl(null);
    };

    return (
        <AppBar position="static" style={{ backgroundColor: 'grey' }}>
            <Toolbar>
                <Grid container justifyContent="space-between" alignItems="center">
                    <Grid item>
                        <Link to="/">
                            <CardMedia
                                component="img"
                                height="40"
                                image={require('../data/logo.png')}
                                alt="Book store"
                            />
                        </Link>
                    </Grid>
                    <Grid item>
                        <Box display="flex" alignItems="center">
                            <IconButton
                                size="large"
                                edge="start"
                                aria-label="menu"
                                aria-controls="cart-menu"
                                aria-haspopup="true"
                                onClick={handleClick}
                                style={{ marginRight: '10px' }}
                            >
                                <Badge badgeContent={cartItems.length} color="secondary">
                                    <ShoppingCartIcon />
                                </Badge>
                            </IconButton>

                            <IconButton size="large" edge="start" aria-label="menu">
                                <MenuIcon />
                            </IconButton>
                        </Box>
                        <Menu
                            id="cart-menu"
                            anchorEl={anchorEl}
                            keepMounted
                            open={Boolean(anchorEl)}
                            onClose={handleClose}
                            style={{ marginLeft: '-8px' }}
                        >
                            {cartItems.length === 0 ? (
                                <MenuItem onClick={handleClose}>Cart is empty</MenuItem>
                            ) : (
                                <>
                                    <MenuItem onClick={() => handleClose()}>Cart Products:</MenuItem>
                                    {cartItems.map((cartItem) => (
                                        <MenuItem key={cartItem.product.id} onClick={handleClose}>
                                            <Box display="flex" alignItems="center">
                                                <CardMedia
                                                    component="img"
                                                    height="40"
                                                    image={require(`../data/${cartItem.product.image}`)}
                                                    alt={cartItem.product.title}
                                                    style={{ marginRight: '10px' }}
                                                />
                                                {cartItem.product.title.length > 20
                                                    ? cartItem.product.title.substring(0, 20) + "..."
                                                    : cartItem.product.title} - Amount: {cartItem.amount}
                                            </Box>
                                        </MenuItem>
                                    ))}
                                    <MenuItem>
                                        <Link to="/cart" style={{ textDecoration: 'none', color: 'black' }}>
                                            <Button variant="contained" style={{ backgroundColor: 'grey', margin: 'auto', display: 'block' }}>
                                                Go to Cart
                                            </Button>
                                        </Link>
                                    </MenuItem>
                                </>
                            )}
                        </Menu>
                    </Grid>
                </Grid>
            </Toolbar>
        </AppBar>
    );
};

export default NavBar;
