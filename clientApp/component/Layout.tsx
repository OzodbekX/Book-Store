import React, {FC, JSXElementConstructor, ReactElement, useEffect, useState} from 'react';
import Head from "next/head";
import {
    AppBar,
    Badge,
    Box,
    Button,
    Container,
    CssBaseline,
    Dialog,
    DialogActions,
    DialogContent,
    DialogTitle,
    Link,
    Switch,
    ThemeProvider,
    Toolbar,
    Typography
} from "@material-ui/core";
import Cookies from 'js-cookie'
import jsCookie from 'js-cookie'
import {createTheme} from '@material-ui/core/styles'
import useStyles from "../Utils/Styles";
import {useAppDispatch, useAppSelector} from "../Utils/hooks";
import NextLink from 'next/link';
import {changeThemeMode} from "../Utils/modeSlice";
import ServerModal from "./CartModal";
import layoutScss from "./layout.module.css";
import {removeUserData} from "../Utils/userSlice";
import {useRouter} from "next/router";
import {ShoppingBag} from "@mui/icons-material";
import {removeShippingData} from "../Utils/shippingSlice";
import {removeCartItems} from "../Utils/cartSlice";

const Layout: FC<TProps> = ({children, description, title}) => {
    const {darkMode} = useAppSelector(state => state.modeSlice)
    const {cartItems} = useAppSelector(state => state)
    const {userData} = useAppSelector(state => state)
    const {route} = useRouter()
    const [open, setOpen] = React.useState(false);
    const handleClickOpen = () => {
        setOpen(true);
    };
    const handleClose = (event: React.SyntheticEvent<unknown>, reason?: string) => {
        if (reason !== 'backdropClick') {
            setOpen(false);
        }
    };

    const [modalState, setModalState] = useState<boolean>(false)
    const dispatch = useAppDispatch()
    const router = useRouter()
    const theme = createTheme({
        typography: {
            h1: {
                fontSize: '1.6rem',
                fontWeight: 400,
                margin: "1rem 0"
            },
            h2: {
                fontSize: '1.4rem',
                fontWeight: 400,
                margin: "1rem 0"
            }
        },
        palette: {
            type: darkMode ? "dark" : "light",
            primary: {
                main: '#0A7029'
            },
            secondary: {
                main: '#208080'
            }
        }
    })
    const classes = useStyles();
    const changeToDarkMode = () => {
        dispatch(changeThemeMode(!darkMode));
        Cookies.set("darkMode", !darkMode ? "true" : "false")
    }

    const logOut = () => {
        if (typeof window !== "undefined") {
            localStorage.removeItem("accessToken")
            jsCookie.remove('cartItems');
            jsCookie.remove('shippingAddress');
            jsCookie.remove('paymentMethod');
        }
        dispatch(removeUserData(null));
        dispatch(removeShippingData(null));
        dispatch(removeCartItems(null));
    }

    useEffect(() => {
        if (userData.accessToken == "" && ["/tasks", "/questions"].includes(router.route)) router.push(`/login?redirect=${router.route}`)
    }, [])

    return (
        <div style={{width: "100%"}}>
            <Head>
                <title>{title ? `${title} - Book Store` : 'Book Store'}</title>
                {description && <meta name="description" content={description}/>}
            </Head>
            <ThemeProvider theme={theme}>
                <CssBaseline/>
                <AppBar className={layoutScss.navbar} position={"static"}>
                    <Toolbar className={"d-flex justify-content-between"}>
                        <div className={"mr-auto d-flex "}>
                            <NextLink href={'/'} passHref>
                                <Link>
                                    <Typography color={route == "/" ? "initial" : "textSecondary"} className={"px-2"}>
                                        Book Store
                                    </Typography>
                                </Link>
                            </NextLink>
                            <NextLink href={'/tasks'} passHref>
                                <Link>
                                    <Typography color={route == "/tasks" ? "initial" : "textSecondary"}
                                                className={"px-2"}>
                                        Tasks
                                    </Typography>
                                </Link>
                            </NextLink>
                            <NextLink href={'/questions'} passHref>
                                <Link>
                                    <Typography color={route == "/questions" ? "initial" : "textSecondary"}
                                                className={"px-2"}>
                                        Questions
                                    </Typography>
                                </Link>
                            </NextLink>
                            <NextLink href={'/admin-chat'} passHref>
                                <Link>
                                    <Typography color={route == "/admin-chat" ? "initial" : "textSecondary"}
                                                className={"px-2"}>
                                        Admin Chat
                                    </Typography>
                                </Link>
                            </NextLink>
                        </div>
                        <div className={classes.grow}/>
                        <div className={"d-flex"}>
                            <Switch onChange={changeToDarkMode} checked={darkMode}/>
                            <Badge
                                overlap={"rectangular"}
                                className={"pe-auto mt-1"}
                                onClick={() => setModalState(true)}
                                style={{height: "25px", width: "28px", marginRight: "10px"}}
                                badgeContent={cartItems.length}
                                color="primary">
                                <ShoppingBag color={"warning"}/>
                            </Badge>
                            <ServerModal
                                handleOpen={() => setModalState(true)}
                                open={modalState}
                                handleClose={() => setModalState(false)}/>
                            {userData?.accessToken !== "" ?
                                <div>
                                    <Button className={"text-white"}
                                            onClick={handleClickOpen}>{userData.emailAddress}</Button>
                                    <Dialog onBackdropClick={handleClose} disableEscapeKeyDown open={open}
                                            onClose={handleClose}>
                                        <DialogTitle>Change the user</DialogTitle>
                                        <DialogContent>
                                            <Box
                                                component="form"
                                                sx={{display: 'flex', flexWrap: 'wrap'}}>
                                                <Button>
                                                    <NextLink href={'/register'} passHref>
                                                        <Typography>
                                                            Edit
                                                        </Typography>
                                                    </NextLink>
                                                </Button>
                                                <Button onClick={logOut}>
                                                    <Typography>
                                                        Log out
                                                    </Typography>
                                                </Button>
                                            </Box>
                                        </DialogContent>
                                        <DialogActions>
                                            <Button onClick={handleClose}>Close</Button>
                                        </DialogActions>
                                    </Dialog>
                                </div>

                                :
                                <NextLink href={'/login'} passHref>
                                    <Link className={"mt-2"}>
                                        Login
                                    </Link>
                                </NextLink>}
                        </div>
                    </Toolbar>
                </AppBar>
                <Container className={classes.main}>
                    {children}
                </Container>
                <footer className={classes.footer}>
                    <Typography>
                        All rights reserved. Bookstore.
                    </Typography>
                </footer>
            </ThemeProvider>
        </div>
    );
}

type TProps = {
    title?: string
    description?: string
    children: ReactElement<any, JSXElementConstructor<any>> | ReactElement<any, JSXElementConstructor<any>>[]
}

export default Layout;