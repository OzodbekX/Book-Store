import {Button, FormControl, FormControlLabel, List, ListItem, Radio, RadioGroup, Typography,} from '@mui/material';
import jsCookie from 'js-cookie';
import {useRouter} from 'next/router';
import {useSnackbar} from 'notistack';
import React, {useEffect, useState} from 'react';
import {useAppDispatch, useAppSelector} from "../Utils/hooks";
import CheckoutWizard from "../component/CheckoutWizard";
import Layout from "../component/Layout";
import Form from "../component/Form";
import {changePaymentMethod} from "../Utils/shippingSlice";

export default function PaymentScreen() {
    const {enqueueSnackbar} = useSnackbar();
    const router = useRouter();
    const dispatch = useAppDispatch();
    const [paymentMethod, setPaymentMethod] = useState('');
    const {cartItems, shippingData} = useAppSelector(store => store);

    useEffect(() => {
        if (!shippingData.address) {
            router.push('/shipping');
        } else {
            setPaymentMethod(jsCookie.get('paymentMethod') || '');
        }
    }, [router, shippingData]);

    const submitHandler = (e: any) => {
        e.preventDefault();
        if (!paymentMethod) {
            enqueueSnackbar('Payment method is required', {variant: 'error'});
        } else {
            dispatch(changePaymentMethod(paymentMethod));
            jsCookie.set('paymentMethod', paymentMethod);
            router.push('/placeorder');
        }
    };
    return (
        <Layout title="Payment Method">
            <CheckoutWizard activeStep={2}/>
            <Form onSubmit={submitHandler}>
                <Typography component="h1" variant="h1">
                    Payment Method
                </Typography>
                <List>
                    <ListItem>
                        <FormControl component="fieldset">
                            <RadioGroup
                                aria-label="Payment Method"
                                name="paymentMethod"
                                value={paymentMethod}
                                onChange={(e) => setPaymentMethod(e.target.value)}
                            >
                                <FormControlLabel
                                    label="PayPal"
                                    value="PayPal"
                                    control={<Radio/>}
                                />
                                <FormControlLabel
                                    label="Stripe"
                                    value="Stripe"
                                    control={<Radio/>}
                                />
                                <FormControlLabel
                                    label="Cash"
                                    value="Cash"
                                    control={<Radio/>}
                                />
                            </RadioGroup>
                        </FormControl>
                    </ListItem>
                    <ListItem>
                        <Button disabled={!["Stripe", "PayPal", "Cash"].includes(paymentMethod)} fullWidth type="submit"
                                variant="contained"
                                color="primary">
                            Continue
                        </Button>
                    </ListItem>
                    <ListItem>
                        <Button
                            fullWidth
                            type="button"
                            variant="contained"
                            color="secondary"
                            onClick={() => router.push('/shipping')}
                        >
                            Back
                        </Button>
                    </ListItem>
                </List>
            </Form>
        </Layout>
    );
}