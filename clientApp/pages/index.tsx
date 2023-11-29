import type {NextPage} from 'next'
import Layout from "../component/Layout";
import {Button, Card, CardActionArea, CardActions, CardContent, CardMedia, Grid, Typography} from "@material-ui/core";
import NextLink from "next/link"
import {useEffect, useState} from "react";
import SelectedProducts from "../component/Produc/SelectedProducts";
import {useAppDispatch, useAppSelector} from "../Utils/hooks";
import {addToCartItems} from "../Utils/cartSlice";
import axiosClient from "../Utils/axios";
import {SentimentSatisfiedAlt, SentimentVeryDissatisfied, ShoppingCart} from "@mui/icons-material";


const Home: NextPage = () => {
    const [selectedItems, setSelectedItems] = useState<any[]>([])
    const [products, setProducts] = useState<any[]>([])
    const dispatch = useAppDispatch()

    const addToCart = (item: any) => {
        dispatch(addToCartItems(item))
    }
    useEffect(() => {
        getProducts()
    }, [])

    const getProducts = () => {
        axiosClient.get('clientBooks')
            .then(res => {
                if (res?.data?.succeeded && URL.createObjectURL) {
                    let list: any = []
                    res.data.data.forEach((i: any) => {
                        i = {
                            ...i,
                            image: `data:image/gif;base64,${i.pictures?.[0]?.bytes}`
                        }
                        list.push(i)
                    })
                    setProducts(list)
                }
            });
    }
    const onUpGareRating = (item: any, like: boolean) => {
        let rating = item.rating || 1
        let newProducts: any[] = []
        if (like) rating++
        else if (rating > 1) rating--
        else return rating
        if (window) {
            let ids = localStorage.getItem('ProductId')?.split(",") || []
            ids = ids.filter(a => a != "")
            if (!ids?.some(i => i == item.bookId)) {
                axiosClient.put(`Books/rating?bookId=${item.bookId}&rating=${rating}`)
                    .then(res => {
                        if (res?.status == 200) {
                            products.forEach((i: any) => {
                                let a = i
                                if (i.bookId == item.booId) {
                                    a.rating = rating
                                }
                                newProducts = [...newProducts, a]
                            })
                            setProducts([...newProducts])
                            localStorage.setItem("ProductId", `${ids.join(",")},${item.bookId}`)
                        }
                    });
            }

        }

    }

    return <Layout>
        <div className="mt-4">
            <Grid container spacing={4}>
                {products.map((product: any) => {
                    return <Grid key={product.bookId} item md={4}>
                        <Card style={{
                            display: "flex",
                            flexDirection: "column",
                            justifyContent: "space-between",
                            height: "400px",
                            width: "350px"
                        }}>
                            <NextLink href={`/product/${product.bookId}`} passHref>
                                <CardActionArea>
                                    <CardMedia
                                        style={{
                                            margin: "auto",
                                            width: "auto",
                                            maxHeight: "250px",
                                            height: "100%"
                                        }}
                                        component={"img"}
                                        image={product.image}
                                        title={product.title}
                                    >
                                    </CardMedia>
                                    <CardContent>
                                        <Typography>
                                            {product.title}
                                        </Typography>
                                    </CardContent>
                                </CardActionArea>
                            </NextLink>

                            <CardActions>
                                <div className={"d-flex w-100 align-items-center justify-content-between"}>
                                    <Typography>
                                        {product.price} $
                                    </Typography>
                                    <Button onClick={() => onUpGareRating(product, false)} size={"small"}
                                            color={"primary"}>
                                        <SentimentVeryDissatisfied/>
                                    </Button>
                                    <Typography color={"primary"}>
                                        Rating:{product.rating ? Math.round(product.rating * 10) / 10 : 1}
                                    </Typography>
                                    <Button onClick={() => onUpGareRating(product, true)} size={"small"}
                                            color={"primary"}>
                                        <SentimentSatisfiedAlt/>
                                    </Button>
                                    <Button onClick={() => addToCart(product)} size={"small"}
                                            color={"primary"}>
                                        <ShoppingCart/>
                                    </Button>
                                </div>
                            </CardActions>
                        </Card>
                    </Grid>
                })}
            </Grid>
            <SelectedProducts setSelectedItems={setSelectedItems} selectedItems={selectedItems}/>
        </div>
    </Layout>

}

export default Home
