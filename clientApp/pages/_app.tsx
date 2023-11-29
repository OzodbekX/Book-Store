import React, {useEffect} from "react"
import '../styles/globals.css'
import '../styles/map.css'
import type {AppProps} from 'next/app'
import {Provider} from "react-redux";
import {store} from "../Utils/Store";
import 'bootstrap/dist/css/bootstrap.css'
import CssBaseline from '@material-ui/core/CssBaseline';
import {SnackbarProvider} from "notistack";

function MyApp({Component, pageProps}: AppProps) {
    useEffect(() => {
        const jssStyles = document.querySelector('#jss-server-side')
        if (jssStyles) jssStyles?.parentElement?.removeChild(jssStyles)
    }, [])

    return <Provider store={store}>
        <CssBaseline/>
        <SnackbarProvider maxSnack={3}>
            <Component {...pageProps} />
        </SnackbarProvider>
    </Provider>
}

export default MyApp
