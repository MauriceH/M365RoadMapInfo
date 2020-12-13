import '../styles/global.css'
import {AppProps} from 'next/app'
import React from "react";
import {RecoilRoot} from "recoil";
import {ThemeProvider} from "@material-ui/styles";
import {myTheme} from "../components/MyTheme";

export default function App({Component, pageProps}: AppProps) {
    return (
        <RecoilRoot>
            <ThemeProvider theme={myTheme}>
                <Component {...pageProps} />
            </ThemeProvider>
        </RecoilRoot>);
}