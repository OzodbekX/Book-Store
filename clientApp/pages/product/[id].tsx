import React, {FC, useEffect, useState} from 'react';
import {useRouter} from 'next/router'
import Layout from "../../component/Layout";
import NextLink from 'next/link';
import {Button, Card, Grid, Link, List, ListItem, Typography,} from '@material-ui/core';
import Rating from '@material-ui/lab/Rating';
import useStyles from "../../Utils/Styles";
import axios from "axios";
import Image from 'next/image';
import {addToCartItems} from "../../Utils/cartSlice";
import {useAppDispatch} from "../../Utils/hooks";


const ProductScreen: FC<any> = (props) => {
    let router = useRouter()
    const [product, setProduct] = useState<any>()
    const classes = useStyles()
    const dispatch = useAppDispatch()

    useEffect(() => {
        let {id} = router.query
        getProduct(`${id}`)
    }, [])
    const addToCart = (item: any) => {
        dispatch(addToCartItems(item))
    }

    const getProduct = (id: number | string) => {
        axios.get(`https://localhost:8000/api/clientBooks/${id}`)
            .then(res => {
                if (res?.data?.succeeded) {
                    setProduct({
                        ...res.data.data,
                        image: `data:image/gif;base64,${res.data.data?.pictures?.[0]?.bytes}`
                    })
                }
            });
    }
    if (!product) {
        return <div>Product not found</div>
    }

    return (
        <Layout title={product?.name} description={product?.description}>
            <div className={classes.section}>
                <NextLink href={'/'} passHref>
                    <Link>
                        <Typography>
                            Back to Product
                        </Typography>
                    </Link>
                </NextLink>
            </div>

            <Grid container spacing={1}>
                <Grid item md={6} xs={12}>
                    {product?.image && <Image
                        src={product?.image}
                        alt={product?.name}
                        width={640}
                        height={640}
                        layout="responsive"
                    />}
                </Grid>
                <Grid item md={3} xs={12}>
                    <List>
                        <ListItem>
                            <Typography component="h1" variant="h1">
                                {product?.name}
                            </Typography>
                        </ListItem>
                        <ListItem>
                            <Typography>Category: {product?.category}</Typography>
                        </ListItem>
                        <ListItem>
                            <Typography>Brand: {product?.brand}</Typography>
                        </ListItem>
                        <ListItem>
                            <Rating value={product?.rating} readOnly/>
                            <Link href="#reviews">
                                <Typography>({product?.numReviews} reviews)</Typography>
                            </Link>
                        </ListItem>
                        <ListItem>
                            <Typography> Description: {product?.description}</Typography>
                        </ListItem>
                    </List>
                </Grid>
                <Grid item md={3} xs={12}>
                    <Card>
                        <List>
                            <ListItem>
                                <Grid container>
                                    <Grid item xs={6}>
                                        <Typography>Price</Typography>
                                    </Grid>
                                    <Grid item xs={6}>
                                        <Typography>${product?.price}</Typography>
                                    </Grid>
                                </Grid>
                            </ListItem>
                            <ListItem>
                                <Grid container>
                                    <Grid item xs={6}>
                                        <Typography>Status</Typography>
                                    </Grid>
                                    <Grid item xs={6}>
                                        <Typography>
                                            {product?.countInStock > 0 ? 'In stock' : 'Unavailable'}
                                        </Typography>
                                    </Grid>
                                </Grid>
                            </ListItem>
                            <ListItem>
                                <Button
                                    fullWidth
                                    variant="contained"
                                    color="primary"
                                    onClick={() => addToCart(product)}
                                >
                                    Add to cart
                                </Button>
                            </ListItem>
                        </List>
                    </Card>
                </Grid>
            </Grid>
            {/*<List>*/}
            {/*    <ListItem>*/}
            {/*        <Typography name="reviews" id="reviews" variant="h2">*/}
            {/*            Customer Reviews*/}
            {/*        </Typography>*/}
            {/*    </ListItem>*/}
            {/*    {reviews.length === 0 && <ListItem>No review</ListItem>}*/}
            {/*    {reviews.map((review) => (*/}
            {/*        <ListItem key={review._id}>*/}
            {/*            <Grid container>*/}
            {/*                <Grid item className={classes.reviewItem}>*/}
            {/*                    <Typography>*/}
            {/*                        <strong>{review.name}</strong>*/}
            {/*                    </Typography>*/}
            {/*                    <Typography>{review.createdAt.substring(0, 10)}</Typography>*/}
            {/*                </Grid>*/}
            {/*                <Grid item>*/}
            {/*                    <Rating value={review.rating} readOnly></Rating>*/}
            {/*                    <Typography>{review.comment}</Typography>*/}
            {/*                </Grid>*/}
            {/*            </Grid>*/}
            {/*        </ListItem>*/}
            {/*    ))}*/}
            {/*    <ListItem>*/}
            {/*        {userInfo ? (*/}
            {/*            <form onSubmit={submitHandler} className={classes.reviewForm}>*/}
            {/*                <List>*/}
            {/*                    <ListItem>*/}
            {/*                        <Typography variant="h2">Leave your review</Typography>*/}
            {/*                    </ListItem>*/}
            {/*                    <ListItem>*/}
            {/*                        <TextField*/}
            {/*                            multiline*/}
            {/*                            variant="outlined"*/}
            {/*                            fullWidth*/}
            {/*                            name="review"*/}
            {/*                            label="Enter comment"*/}
            {/*                            value={comment}*/}
            {/*                            onChange={(e) => setComment(e.target.value)}*/}
            {/*                        />*/}
            {/*                    </ListItem>*/}
            {/*                    <ListItem>*/}
            {/*                        <Rating*/}
            {/*                            name="simple-controlled"*/}
            {/*                            value={rating}*/}
            {/*                            onChange={(e) => setRating(e.target.value)}*/}
            {/*                        />*/}
            {/*                    </ListItem>*/}
            {/*                    <ListItem>*/}
            {/*                        <Button*/}
            {/*                            type="submit"*/}
            {/*                            fullWidth*/}
            {/*                            variant="contained"*/}
            {/*                            color="primary"*/}
            {/*                        >*/}
            {/*                            Submit*/}
            {/*                        </Button>*/}

            {/*                        {loading && <CircularProgress></CircularProgress>}*/}
            {/*                    </ListItem>*/}
            {/*                </List>*/}
            {/*            </form>*/}
            {/*        ) : (*/}
            {/*            <Typography variant="h2">*/}
            {/*                Please{' '}*/}
            {/*                <Link href={`/login?redirect=/product/${product.slug}`}>*/}
            {/*                    login*/}
            {/*                </Link>{' '}*/}
            {/*                to write a review*/}
            {/*            </Typography>*/}
            {/*        )}*/}
            {/*    </ListItem>*/}
            {/*</List>*/}
        </Layout>
    );
};

export async function getServerSideProps(context: any) {
    const {params} = context;
    const {slug} = params;
    return {
        props: {
            product: {slug:`${slug}`}
        },
    };
}

export default ProductScreen