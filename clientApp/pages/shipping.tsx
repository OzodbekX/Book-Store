import React, {useEffect, useState} from 'react';
import Layout from "../component/Layout";
import CheckoutWizard from "../component/CheckoutWizard";
import Form from "../component/Form";
import Typography from "@mui/material/Typography";

import {Button, List, ListItem, TextField} from "@material-ui/core";
import {Controller, FieldValues, SubmitHandler, useForm} from "react-hook-form";
import {useRouter} from "next/router";
import jsCookie from 'js-cookie';
import {useAppDispatch, useAppSelector} from "../Utils/hooks";
import {changeSingleArea, saveShippingData} from "../Utils/shippingSlice";
import Map from "../component/Map/Map";
import {OutlinedInput} from "@mui/material";
import {LocationOn} from "@mui/icons-material";

const location = {
    address: '1600 Amphitheatre Parkway, Mountain View, california.',
    lat: 37.42216,
    lng: -122.08427,
}

function ShippingScreen(props: any) {
    const {
        handleSubmit,
        control,
        formState: {errors},
        setValue,
        getValues
    } = useForm();
    const router = useRouter();
    const dispatch = useAppDispatch()
    const [openMap, setOpenMap] = useState(false)
    const {userData, shippingData} = useAppSelector(store => store);
    useEffect(() => {
        if (userData.accessToken !== "") {
            setValue('receiver', shippingData.receiver);
            setValue('address', shippingData.address);
            setValue('city', shippingData.city);
            setValue('postalCode', shippingData.postalCode);
            setValue('country', shippingData.country);
            setValue('secretWord', shippingData.secretWord);
        } else {
            router.push('/login?redirect=/shipping');
        }
    }, [router, setValue, shippingData, userData]);

    const onChangeForm = (e: any) => {
        dispatch(changeSingleArea({key: e.target.id, value: e.target.value}));
    }

    const submitHandler: SubmitHandler<FieldValues> = ({receiver, address, city, postalCode, country, secretWord}) => {
        dispatch(saveShippingData({receiver, address, city, postalCode, country, secretWord}))
        jsCookie.set(
            'shippingData',
            JSON.stringify({
                receiver,
                address,
                city,
                postalCode,
                secretWord,
                country,
            })
        );
        router.push('/payment');
    };

    // AIzaSyBydYec6PtEXmv0nE6VDu_KAFrnkjrJo7E
    return (
        <Layout title={"Shipping address"}>
            <CheckoutWizard activeStep={1}/>
            <Form onChange={onChangeForm} onSubmit={handleSubmit(submitHandler)}>
                <Typography component={"h1"} variant={"h1"}>
                    Shipping Address
                </Typography>
                <List>
                    <ListItem>
                        <Controller
                            name="receiver"
                            control={control}
                            defaultValue=""
                            rules={{
                                required: true,
                                minLength: 2,
                            }}
                            render={({field}) => (
                                <TextField
                                    variant="outlined"
                                    fullWidth
                                    id="receiver"
                                    label="Full Name"
                                    inputProps={{type: 'receiver'}}
                                    error={Boolean(errors.receiver)}
                                    helperText={
                                        errors.receiver
                                            ? errors.receiver.type === 'minLength'
                                                ? 'Full Name length is more than 1'
                                                : 'Full Name is required'
                                            : ''
                                    }
                                    {...field}
                                />
                            )}
                        />

                    </ListItem>
                    <ListItem>
                        <Controller
                            name="city"
                            control={control}
                            defaultValue=""
                            rules={{
                                required: true,
                                minLength: 2,
                            }}
                            render={({field}) => (
                                <TextField
                                    variant="outlined"
                                    fullWidth
                                    id="city"
                                    label="City"
                                    inputProps={{type: 'city'}}
                                    error={Boolean(errors.city)}
                                    helperText={
                                        errors.city
                                            ? errors.city.type === 'minLength'
                                                ? 'City length is more than 1'
                                                : 'City is required'
                                            : ''
                                    }
                                    {...field}
                                />
                            )}
                        />
                    </ListItem>
                    <ListItem>
                        <Controller
                            name="postalCode"
                            control={control}
                            defaultValue=""
                            rules={{
                                required: true,
                                minLength: 2,
                            }}
                            render={({field}) => (
                                <TextField
                                    variant="outlined"
                                    fullWidth
                                    id="postalCode"
                                    label="Postal Code"
                                    inputProps={{type: 'postalCode'}}
                                    error={Boolean(errors.postalCode)}
                                    helperText={
                                        errors.postalCode
                                            ? errors.postalCode.type === 'minLength'
                                                ? 'Postal Code length is more than 1'
                                                : 'Postal Code is required'
                                            : ''
                                    }
                                    {...field}
                                />
                            )}
                        />
                    </ListItem>
                    <ListItem>
                        <Controller
                            name="country"
                            control={control}
                            defaultValue=""
                            rules={{
                                required: true,
                                minLength: 2,
                            }}
                            render={({field}) => (
                                <TextField
                                    variant="outlined"
                                    fullWidth
                                    id="country"
                                    label="Country"
                                    inputProps={{type: 'country'}}
                                    error={Boolean(errors.country)}
                                    helperText={
                                        errors.country
                                            ? errors.country.type === 'minLength'
                                                ? 'Country length is more than 1'
                                                : 'Country is required'
                                            : ''
                                    }
                                    {...field}
                                />
                            )}
                        />
                    </ListItem>
                    <ListItem>
                        <Controller
                            name="secretWord"
                            control={control}
                            defaultValue=""
                            rules={{
                                required: true,
                                minLength: 2,
                            }}
                            render={({field}) => (
                                <TextField
                                    variant="outlined"
                                    fullWidth
                                    id="secretWord"
                                    label="Secret Word"
                                    inputProps={{type: 'secretWord'}}
                                    error={Boolean(errors.secretWord)}
                                    helperText={
                                        errors.secretWord
                                            ? errors.secretWord.type === 'minLength'
                                                ? 'Address length is more than 1'
                                                : 'Address is required'
                                            : ''
                                    }
                                    {...field}
                                />
                            )}
                        />
                    </ListItem>
                    <ListItem>
                        <Controller
                            name="address"
                            control={control}
                            defaultValue=""
                            rules={{
                                required: true,
                                minLength: 2,
                            }}
                            render={({field}) => (
                                <OutlinedInput
                                    fullWidth
                                    id="address"
                                    label="Address"
                                    type={'address'}
                                    error={Boolean(errors.address)}
                                    aria-errormessage={
                                        errors.address
                                            ? errors.address.type === 'minLength'
                                                ? 'Address length is more than 1'
                                                : 'Address is required'
                                            : ''
                                    }
                                    endAdornment={
                                        <Button onClick={() => setOpenMap(true)}>
                                            <LocationOn/>
                                        </Button>
                                    }
                                    {...field}
                                />
                            )}
                        />
                    </ListItem>
                    <Map openMap={openMap} handleClose={setOpenMap} setValue={setValue} location={location}
                         zoomLevel={17}/>
                    <ListItem>
                        <Button variant="contained" type="submit" fullWidth color="primary">
                            Continue
                        </Button>
                    </ListItem>
                </List>
            </Form>
        </Layout>
    );
}

type TShipping = {
    receiver: string
    address: string
    city: string
    postalCode: string
    country: string
    secretWord: string
}


export default ShippingScreen;