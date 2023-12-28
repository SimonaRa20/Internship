import React, { useState, useRef, useCallback, useEffect } from 'react';
import { Link } from 'react-router-dom';
import { Box, Grid, Card, CardContent, CardMedia, Typography } from '@mui/material';
import { Product } from '../type/productTypes';

interface ProductListProps {
    products: Product[];
}

const ProductList: React.FC<ProductListProps> = ({ products }) => {
    const [visibleProducts, setVisibleProducts] = useState(4);
    const productsRef = useRef<HTMLDivElement | null>(null);

    const observerCallback: IntersectionObserverCallback = useCallback(
        (entries) => {
            const target = entries[0];
            if (target.isIntersecting) {
                if (visibleProducts < products.length) {
                    const loadAmount = Math.min(4, products.length - visibleProducts);
                    setVisibleProducts((prevVisible) => prevVisible + loadAmount);
                }
            }
        },
        [visibleProducts, products]
    );

    useEffect(() => {
        const intersectionObserver = new IntersectionObserver(observerCallback, {
            root: null,
            threshold: 0.1,
        });

        const currentProductsRef = productsRef.current;

        if (currentProductsRef) {
            intersectionObserver.observe(currentProductsRef);
        }

        return () => {
            if (currentProductsRef) {
                intersectionObserver.unobserve(currentProductsRef);
            }
        };
    }, [observerCallback]);

    return (
        <div>
            <Grid container spacing={2} paddingTop={2}>
                {products.slice(0, visibleProducts).map((product, index) => (
                    <Grid item key={product.id} xs={12}>
                        <Link to={`/product/${product.id}`} style={{ textDecoration: "none" }}>
                            <Box style={{ justifyContent: "center", alignItems: "center", display: "flex", width: "100%" }}>
                                <Card style={{ width: "50%" }}>
                                    <CardMedia
                                        component="img"
                                        height="150"
                                        image={require(`../data/${product.image}`)}
                                        alt={product.title}
                                    />
                                    <CardContent>
                                        <Box style={{ display: "flex", justifyContent: "space-between" }}>
                                            <Typography variant="h6">
                                                {product.title.length > 50
                                                    ? product.title.substring(0, 50) + "..."
                                                    : product.title}
                                            </Typography>
                                            <Typography variant="h6" color="textPrimary">
                                                {product.price}$
                                            </Typography>
                                        </Box>
                                    </CardContent>
                                </Card>
                            </Box>
                        </Link>
                        {index === visibleProducts - 1 && (
                            <div ref={productsRef}></div>
                        )}
                    </Grid>
                ))}
            </Grid>
        </div>
    );
};

export default ProductList;
