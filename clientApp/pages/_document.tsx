import Document, {Head, Html, Main, NextScript} from "next/document";
import React from "react";
import createCache from "@emotion/cache"
import createEmotionServer from '@emotion/server/create-instance'

export default class MyDocument extends Document {
    render(): JSX.Element {
        return (
            <Html lang={"en"}>
                <Head>
                    <link rel="stylesheet"
                          href="https://fonts.googleapis.com/css?family=Roboto:300,400,500,700&display=swap"
                    />
                </Head>
                <body>
                <Main/>
                <NextScript/>
                <div id="modal-root"/>
                </body>
            </Html>
        )
    }
}
MyDocument.getInitialProps = async (ctx) => {
    // const sheets = new ServerStyleSheets();
    const originalRenderPage = ctx.renderPage;
    const cache = createCache({key: "css"})
    const {extractCriticalToChunks} = createEmotionServer(cache)
    ctx.renderPage = () =>
        originalRenderPage({
            // eslint-disable-next-line react/display-name
            enhanceApp: (App) => (props) =>
                <App emotionCache={cache} {...props} />,
        });
    const initialProps = await Document.getInitialProps(ctx);
    const emotionStyles = extractCriticalToChunks(initialProps.html);
    const emotionStyleTags = emotionStyles.styles.map((style) => (
        <style
            data-emotion={`${style.key} ${style.ids.join(' ')}`}
            key={style.key}
            // eslint-disable-next-line react/no-danger
            dangerouslySetInnerHTML={{__html: style.css}}
        />
    ));
    return {
        ...initialProps,
        styles: [
            ...React.Children.toArray(initialProps.styles),
            ...emotionStyleTags,
        ],
    };
}