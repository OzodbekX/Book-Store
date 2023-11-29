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
import {useRouter} from 'next/router'
import axiosClient from "../Utils/axios";
import {useAppDispatch} from "../Utils/hooks";
import {setUserData} from "../Utils/userSlice";
import {Visibility, VisibilityOff} from "@mui/icons-material";

export default function RegisterScreen(props: Props) {
    const {handleSubmit, control, getValues, clearErrors, setError, formState: {errors}} = useForm()
    const dispatch = useAppDispatch()
    const [visible, setVisible] = useState<any>({password: false, confirmPassword: false})
    const router = useRouter()
    const submitHandler = ({
                               firstName,
                               emailAddress,
                               password,
                               confirmPassword
                           }: any) => {
        axiosClient.post('Authenticate/createUser', {
            firstName,
            emailAddress,
            password,
            confirmPassword
        }).then(res => {
            if (password !== confirmPassword) setError("confirmPassword", {type: "passwordIsNotIdentical"})
            else if (res.status == 200) {
                if (res.data !== "resigningError") {
                    dispatch(setUserData(res.data))
                    if (typeof window !== "undefined") localStorage.setItem("accessToken", res?.data?.accessToken)
                    router.push("/", undefined, {shallow: true})
                } else setError("emailAddress", {type: 'resigningError', message: 'this email is already exist'})
            }

        })
    }

    const checkPassword = (e: any) => {
        if (e.target.name === "confirmPassword") {
            let password = getValues().password
            if (e.target.value !== password) setError("confirmPassword", {type: "passwordIsNotIdentical"})
            else clearErrors("confirmPassword")
        }
    }

    const {redirect} = router.query;
    return (
        <Layout>
            <Form className={"w-50"} onChange={checkPassword}
                  onSubmit={handleSubmit(submitHandler)}>
                <Typography component={"h1"} variant={"h1"}>
                    Register
                </Typography>
                <List>
                    <ListItem>
                        <Controller
                            name="firstName"
                            control={control}
                            defaultValue=""
                            rules={{
                                required: true,
                                minLength: 2,
                            }}
                            render={({field}) => (
                                <div className={"w-100"}>
                                    <Typography
                                        color={Boolean(errors.firstName) ? "error" : "textPrimary"}>Name</Typography>
                                    <TextField
                                        variant="outlined"
                                        fullWidth
                                        id="firstName"
                                        inputProps={{type: 'text'}}
                                        error={Boolean(errors.firstName)}
                                        helperText={
                                            errors.firstName
                                                ? errors.firstName.type === 'minLength'
                                                    ? 'Name length is more than 1'
                                                    : 'Name is required'
                                                : ''
                                        }
                                        {...field}
                                    />
                                </div>
                            )}
                        />
                    </ListItem>

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
                                        color={Boolean(errors.emailAddress) ? "error" : "textPrimary"}>Email</Typography>
                                    <TextField
                                        variant="outlined"
                                        fullWidth
                                        id="emailAddress"
                                        inputProps={{type: 'email'}}
                                        error={Boolean(errors.emailAddress)}
                                        helperText={
                                            errors.emailAddress
                                                ? errors.emailAddress.type === 'pattern'
                                                    ? 'Email is not valid'
                                                    : errors.emailAddress.type === "resigningError" ?
                                                        'Email is already exist' : "Email is required"
                                                : ''
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
                                        inputProps={{type: visible.password ? "text" : 'password'}}
                                        error={Boolean(errors.password)}
                                        aria-errormessage={
                                            errors.password
                                                ? errors.password.type === 'minLength'
                                                    ? 'Password length is more than 5'
                                                    : 'Password is required'
                                                : 'password is required'
                                        }
                                        endAdornment={
                                            <InputAdornment position="end">
                                                <IconButton
                                                    aria-label="toggle password visibility"
                                                    onClick={() => setVisible({password: !visible.password})}
                                                    // onMouseDown={handleMouseDownPassword}
                                                >
                                                    {visible.password ? <Visibility/> : <VisibilityOff/>}
                                                </IconButton>
                                            </InputAdornment>
                                        }
                                        {...field}
                                    />
                                </div>

                            )}
                        />
                    </ListItem>
                    <ListItem>
                        <Controller
                            name="confirmPassword"
                            control={control}
                            defaultValue=""
                            rules={{
                                required: true,
                                minLength: 6,
                            }}
                            render={({field}) => (
                                <div className={"w-100"}>
                                    <Typography color={Boolean(errors.confirmPassword) ? "error" : "textPrimary"}>Repeat
                                        password</Typography>
                                    <OutlinedInput
                                        fullWidth
                                        id="confirmPassword"
                                        inputProps={{type: visible.confirmPassword ? "text" : 'password'}}
                                        error={Boolean(errors.confirmPassword)}
                                        aria-errormessage={
                                            errors.confirmPassword
                                                ? errors.confirmPassword.type === 'minLength'
                                                    ? 'Confirm Password length is more than 5'
                                                    : errors.confirmPassword.type === 'passwordIsNotIdentical' ?
                                                        "Password is not identical" :
                                                        'Confirm Password is required'
                                                : ''
                                        }
                                        endAdornment={
                                            <InputAdornment position="end">
                                                <IconButton
                                                    aria-label="toggle password visibility"
                                                    onClick={() => setVisible({confirmPassword: !visible.password})}
                                                    // onMouseDown={handleMouseDownPassword}
                                                >
                                                    {visible.confirmPassword ? <Visibility/> : <VisibilityOff/>}
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
                        <Button variant="contained" style={{marginRight: "10px"}} fullWidth>
                            <NextLink href={`/login?redirect=${redirect || '/'}`} passHref>
                                <Link>Login Page</Link>
                            </NextLink>
                        </Button>
                        <Button variant="contained" type="submit" fullWidth color="primary">
                            Register
                        </Button>
                    </ListItem>
                </List>
            </Form>
        </Layout>
    );
}
type Props = {}

