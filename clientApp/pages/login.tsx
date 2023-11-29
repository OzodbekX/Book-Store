import React, {useState} from 'react';
import {Controller, useForm} from "react-hook-form";
import Layout from "../component/Layout";
import Form from "../component/Form";
import {
    Button,
    IconButton,
    InputAdornment,
    Link,
    List,
    ListItem,
    OutlinedInput,
    TextField,
    Typography
} from "@material-ui/core";
import NextLink from 'next/link';
import {useRouter} from "next/router";
import axiosClient from "../Utils/axios";
import {setUserData} from "../Utils/userSlice";
import {useAppDispatch} from "../Utils/hooks";
import {Visibility, VisibilityOff} from "@mui/icons-material";
import {useSnackbar} from "notistack";

export default function LoginScreen(props: Props) {
    const {enqueueSnackbar} = useSnackbar();

    const dispatch = useAppDispatch()
    const {handleSubmit, setError, clearErrors, control, formState: {errors}} = useForm()
    const [visible, setVisible] = useState<boolean>(false)

    const router = useRouter();
    const {redirect} = router.query;

    const submitHandler = async ({emailAddress, password}:any) => {
        axiosClient.post('Authenticate', {
            emailAddress,
            password,
        }).then(res => {
            if (res.status == 200) {
                if (res.data !== "notFound") {
                    dispatch(setUserData(res.data))
                    if (typeof window !== "undefined") localStorage.setItem("accessToken", res?.data?.accessToken)
                    router.push(redirect?.toString() || "/", undefined, {shallow: false})
                } else {
                    enqueueSnackbar('Password or login is wrong!!', {variant: 'error'});
                    setError("emailAddress", {type: 'validation', message: 'Password or login is incorrect'})
                    setError("password", {type: 'validation', message: 'Password or login is incorrect'})
                }

            }

        })
    }

    const resetErrorType = () => {
        if (errors?.emailAddress?.type === 'validation') {
            clearErrors("emailAddress")
        }
        if (errors?.password?.type === 'validation') {
            clearErrors("password")
        }
    }

    return (
        <Layout>
            <Form className={"w-50"} onChange={resetErrorType} onSubmit={handleSubmit(submitHandler)}>
                <Typography component={"h1"} variant={"h1"}>
                    Login
                </Typography>
                <List>
                    <ListItem>
                        <Controller
                            name="emailAddress"
                            control={control}
                            defaultValue=""
                            rules={{
                                required: true,
                                pattern: /^[a-z0-9._%+-]+@[a-z0-9.-]+\.[a-z]{2,4}$/,
                            }}
                            render={({field}) => (
                                <div className={"w-100"}>
                                    <Typography
                                        color={Boolean(errors.emailAddress) ? "error" : "textPrimary"}>Email
                                    </Typography>
                                    <TextField
                                        variant="outlined"
                                        fullWidth
                                        id="emailAddress"
                                        inputProps={{type: 'emailAddress'}}
                                        error={Boolean(errors.emailAddress)}
                                        helperText={
                                            errors.emailAddress
                                                ? errors.emailAddress.type === 'pattern'
                                                    ? 'Email is not valid'
                                                    : errors.emailAddress.type === 'validation' ? "Email may be incorrect" : "Email is required"
                                                : 'email is not valid'
                                        }
                                        {...field}
                                    />
                                </div>

                            )}
                        />
                    </ListItem>
                    <ListItem>
                        <Controller
                            name="password"
                            control={control}
                            defaultValue=""
                            rules={{
                                required: true,
                                minLength: 6,
                            }}
                            render={({field}) => (
                                <div className={"w-100"}>
                                    <Typography
                                        color={Boolean(errors.password) ? "error" : "textPrimary"}>Password</Typography>
                                    <OutlinedInput
                                        // variant="outlined"
                                        fullWidth
                                        // label="Password"
                                        id="outlined-adornment-password"
                                        inputProps={{type: visible ? "text" : 'password'}}
                                        error={Boolean(errors.password)}
                                        aria-errormessage={
                                            errors.password
                                                ? errors.password.type === 'minLength'
                                                    ? 'Password length is more than 5'
                                                    : 'Password is required'
                                                : ''
                                        }
                                        endAdornment={
                                            <InputAdornment position="end">
                                                <IconButton
                                                    aria-label="toggle password visibility"
                                                    onClick={() => setVisible(!visible)}
                                                    // onMouseDown={handleMouseDownPassword}
                                                >
                                                    {visible ? <Visibility/> : <VisibilityOff/>}
                                                </IconButton>
                                            </InputAdornment>
                                        }
                                        // helperText={
                                        //
                                        // }
                                        {...field}
                                    />
                                </div>

                            )}
                        />
                    </ListItem>

                    <ListItem>
                        <NextLink href={`/register?redirect=${redirect || '/'}`} passHref>
                            <Button variant="contained" style={{marginRight: "10px"}} fullWidth>
                                <Link className={"ml-3"}> Register</Link>
                            </Button>

                        </NextLink>
                        <Button variant="contained" type="submit" fullWidth color="primary">
                            Login
                        </Button>
                    </ListItem>
                </List>
            </Form>
        </Layout>
    );
}
type Props = {}

