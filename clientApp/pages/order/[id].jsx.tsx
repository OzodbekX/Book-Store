import React, {useContext, useEffect, useReducer} from 'react'
import dynamic from "next/dynamic";
import Layout from "../../component/Layout";
import {Alert, Typography} from "@mui/material";
import {Box, CircularProgress} from "@material-ui/core";
import {Action} from "redux";
import {useRouter} from "next/router";
import axiosClient from "../../Utils/axios";
import {useAppSelector} from "../../Utils/hooks";
import {getError} from "../../Utils/errors";

function reducer(state: { loading: true, error: true }, action: { type: string, payload: boolean }) {
    switch (action.type) {
        case 'FETCH_REQUEST':
            return {...state, loading: true, error: ''};
        case 'FETCH_SUCCESS':
            return {...state, loading: false, order: action.payload, error: ''};
        case 'FETCH_FAIL':
            return {...state, loading: false, error: action.payload};
        case 'PAY_REQUEST':
            return {...state, loadingPay: true};
        case 'PAY_SUCCESS':
            return {...state, loadingPay: false, successPay: true};
        case 'PAY_FAIL':
            return {...state, loadingPay: false, errorPay: action.payload};
        case 'PAY_RESET':
            return {...state, loadingPay: false, successPay: false, errorPay: ''};
    }
}

function OrderScreen({params}: { params: any }) {

    const {id: orderId} = params
    const [{loading, error, order, successPay}, dispatch]:any = useReducer<any>(
        reducer,
        {
            loading: true,
            order: {},
            error: '',
        }
    );
    const {
        shippingAddress,
        paymentMethod,
        orderItems,
        itemsPrice,
        taxPrice,
        shippingPrice,
        totalPrice,
        isPaid,
        paidAt,
        isDelivered,
        deliveredAt,
    } = order;

    const router = useRouter();
    const { userData } = useAppSelector(store=>store);

    const [{ isPending }, paypalDispatch] = usePayPalScriptReducer();

    useEffect(() => {
        if (!userData) {
            return router.push('/login');
        }
        const fetchOrder = async () => {
            try {
                dispatch({ type: 'FETCH_REQUEST' });
                const { data } = await axiosClient.get(`/api/orders/${orderId}`, {
                    headers: { authorization: `Bearer ${userData?.accessToken}` },
                });

                dispatch({ type: 'FETCH_SUCCESS', payload: data });
            } catch (err) {
                dispatch({ type: 'FETCH_FAIL', payload: getError(err) });
            }
        };
        if (!order._id || successPay || (order._id && order._id !== orderId)) {
            fetchOrder();
            if (successPay) {
                dispatch({ type: 'PAY_RESET' });
            }
        } else {
            const loadPaypalScript = async () => {
                const { data: clientId } = await axiosClient.get('/api/keys/paypal', {
                    headers: { authorization: `Bearer ${userData?.accessToken}` },
                });
                paypalDispatch({
                    type: 'resetOptions',
                    value: {
                        'client-id': clientId,
                        currency: 'USD',
                    },
                });
                paypalDispatch({ type: 'setLoadingStatus', value: 'pending' });
            };
            loadPaypalScript();
        }
    }, [order, orderId, successPay, paypalDispatch, router, userData]);


    return <Layout title={`Order ${orderId}`}>
        <Typography component={'h1'} variant={'h1'}>
            Order {orderId}
            {
                loading ? <CircularProgress/> : error ? <Alert variant={"outlined"}>{error}</Alert> : <Box>
                </Box>
            }
        </Typography>
    </Layout>;
}

export function getServerSideProps({params}: { params: any }) {
    return {props: {params}};
}

export default dynamic(() => Promise.resolve(OrderScreen), {ssr: false});