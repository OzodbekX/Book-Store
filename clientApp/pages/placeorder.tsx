import React, {useEffect, useState} from 'react';
import {
    Card,
    CircularProgress,
    Grid,
    Link,
    List,
    ListItem,
    Table,
    TableBody,
    TableCell,
    TableContainer,
    TableHead,
    TableRow,
    Typography,
} from '@mui/material';
import Button from '@mui/material/Button';
import Image from 'next/image';
import {useRouter} from 'next/router';
import NextLink from 'next/link';
import CheckoutWizard from '../component/CheckoutWizard';
import Layout from '../component/Layout';
import classes from '../Utils/classes';
import {useSnackbar} from 'notistack';
import {getError} from '../Utils/errors';
import jsCookie from 'js-cookie';
import dynamic from 'next/dynamic';
import {useAppDispatch, useAppSelector} from "../Utils/hooks";
import axiosClient from "../Utils/axios";

function PlaceOrderScreen() {
    const {enqueueSnackbar} = useSnackbar();
    const [loading, setLoading] = useState(false);
    const router = useRouter();

    const dispatch = useAppDispatch()
    const {shippingData, userData} = useAppSelector(store => store);
    const {cartItems} = useAppSelector(store => store);
    const round2 = (num: any) => Math.round(num * 100 + Number.EPSILON) / 100; // 123.456 => 123.46
    const itemsPrice = round2(cartItems?.reduce((a: any, c: any) => a + c.price * c.count, 0));
    const shippingPrice = itemsPrice > 1000 ? 0 : 15;
    const taxPrice = round2(itemsPrice * 0.15);
    const totalPrice = round2(itemsPrice + shippingPrice + taxPrice);
    console.log("cartItems", cartItems)
    useEffect(() => {
        if (!shippingData.paymentMethod) {
            router.push('/payment');
        }
        if (cartItems?.length === 0) {
            router.push('/');
        }
    }, [cartItems, shippingData.paymentMethod, router]);

    const placeOrderHandler = async () => {
        try {
            setLoading(true);
            let saleData: string = ""
            cartItems.forEach((x: any) => {
                for (let a = 1; a <= x.count; a++) {
                    saleData = saleData + `${x.bookId},`
                }
            })
            if (saleData.charAt(saleData.length - 1)) {
                saleData = saleData.slice(0, -1)
            }
            let query = new FormData()
            query?.append("Address", shippingData.address)
            query?.append("SaleData", saleData)
            query?.append("PostalCode", shippingData.postalCode)
            query?.append("UserId", userData.userId.toString())
            query?.append("PayTerms", shippingData.paymentMethod || "Paypal")


            await axiosClient.post(
                '/shipping', query
            ).then(res => {
                if (res.data.succeeded) {
                    dispatch({type: 'CART_CLEAR'});
                    jsCookie.remove('cartItems');
                    setLoading(false);
                    router.push(`/order/${res}`);
                }
            });

        } catch (err) {
            setLoading(false);
            enqueueSnackbar(getError(err), {variant: 'error'});
        }
    };
    return (
        <Layout title="Place Order">
            <CheckoutWizard activeStep={3}/>
            <Typography component="h1" variant="h1">
                Place Order
            </Typography>

            <Grid container spacing={1}>
                <Grid item md={9} xs={12}>
                    <Card sx={classes.section}>
                        <List>
                            <ListItem>
                                <Typography component="h2" variant="h2">
                                    Shipping Address
                                </Typography>
                            </ListItem>
                            <ListItem>
                                {shippingData.receiver}, {shippingData.address},{' '}
                                {shippingData.city}, {shippingData.postalCode},{' '}
                                {shippingData.country}
                            </ListItem>
                            <ListItem>
                                <Button
                                    onClick={() => router.push('/shipping')}

                                    type="submit"
                                    color="secondary"
                                >
                                    Edit
                                </Button>
                            </ListItem>
                        </List>
                    </Card>
                    <Card sx={classes.section}>
                        <List>
                            <ListItem>
                                <Typography component="h2" variant="h2">
                                    Payment Method
                                </Typography>
                            </ListItem>
                            <ListItem>{shippingData.paymentMethod}</ListItem>
                            <ListItem>
                                <Button
                                    onClick={() => router.push('/payment')}
                                    type="submit"
                                    color="secondary"
                                >
                                    Edit
                                </Button>
                            </ListItem>
                        </List>
                    </Card>
                    <Card sx={classes.section}>
                        <List>
                            <ListItem>
                                <Typography component="h2" variant="h2">
                                    Order Items
                                </Typography>
                            </ListItem>
                            <ListItem>
                                <TableContainer>
                                    <Table>
                                        <TableHead>
                                            <TableRow>
                                                <TableCell>Image</TableCell>
                                                <TableCell>Name</TableCell>
                                                <TableCell align="right">Quantity</TableCell>
                                                <TableCell align="right">Price</TableCell>
                                            </TableRow>
                                        </TableHead>
                                        <TableBody>
                                            {cartItems.map((item, number) => (
                                                <TableRow key={number}>
                                                    <TableCell>
                                                        <NextLink href={`/product/${item.bookId}`} passHref>
                                                            <Link>
                                                                <Image
                                                                    src={item.image}
                                                                    alt={item.title}
                                                                    width={50}
                                                                    height={50}
                                                                />
                                                            </Link>
                                                        </NextLink>
                                                    </TableCell>
                                                    <TableCell>
                                                        <NextLink href={`/product/${item.bookId}`} passHref>
                                                            <Link>
                                                                <Typography>{item.title}</Typography>
                                                            </Link>
                                                        </NextLink>
                                                    </TableCell>
                                                    <TableCell align="right">
                                                        <Typography>{item.count}</Typography>
                                                    </TableCell>
                                                    <TableCell align="right">
                                                        <Typography>${item.price}</Typography>
                                                    </TableCell>
                                                </TableRow>
                                            ))}
                                        </TableBody>
                                    </Table>
                                </TableContainer>
                            </ListItem>
                        </List>
                    </Card>
                </Grid>
                <Grid item md={3} xs={12}>
                    <Card sx={classes.section}>
                        <List>
                            <ListItem>
                                <Typography variant="h2">Order Summary</Typography>
                            </ListItem>
                            <ListItem>
                                <Grid container>
                                    <Grid item xs={6}>
                                        <Typography>Items:</Typography>
                                    </Grid>
                                    <Grid item xs={6}>
                                        <Typography align="right">${itemsPrice}</Typography>
                                    </Grid>
                                </Grid>
                            </ListItem>
                            <ListItem>
                                <Grid container>
                                    <Grid item xs={6}>
                                        <Typography>Shipping:</Typography>
                                    </Grid>
                                    <Grid item xs={6}>
                                        <Typography align="right">${shippingPrice}</Typography>
                                    </Grid>
                                </Grid>
                            </ListItem>
                            <ListItem>
                                <Grid container>
                                    <Grid item xs={6}>
                                        <Typography>Taxes:</Typography>
                                    </Grid>
                                    <Grid item xs={6}>
                                        <Typography align="right">${taxPrice}</Typography>
                                    </Grid>
                                </Grid>
                            </ListItem>
                            <ListItem>
                                <Grid container>
                                    <Grid item xs={6}>
                                        <Typography>
                                            <strong>Total:</strong>
                                        </Typography>
                                    </Grid>
                                    <Grid item xs={6}>
                                        <Typography align="right">
                                            <strong>${totalPrice}</strong>
                                        </Typography>
                                    </Grid>
                                </Grid>
                            </ListItem>
                            <ListItem>
                                <Button
                                    onClick={placeOrderHandler}
                                    variant="contained"
                                    color="primary"
                                    fullWidth
                                    disabled={loading}
                                >
                                    Place Order
                                </Button>
                            </ListItem>
                            {loading && (
                                <ListItem>
                                    <CircularProgress/>
                                </ListItem>
                            )}
                        </List>
                    </Card>
                </Grid>
            </Grid>
        </Layout>
    );
}

export default dynamic(() => Promise.resolve(PlaceOrderScreen), {ssr: false});
