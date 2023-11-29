import * as React from 'react';
import Box from '@mui/material/Box';
import Button from '@mui/material/Button';
import Typography from '@mui/material/Typography';
import Modal from '@mui/material/Modal';
import Fade from '@mui/material/Fade';
import {useAppDispatch, useAppSelector} from "../Utils/hooks";
import {OutlinedInput} from "@material-ui/core";
import {decreaseCount, improveCount, updateCountItems} from "../Utils/cartSlice";
import {useRouter} from "next/router";

export const customStyle = {
    position: 'absolute' as 'absolute',
    top: '50%',
    left: '50%',
    transform: 'translate(-50%, -50%)',
    width: 800,
    bgcolor: 'background.paper',
    border: '1px solid #000',
    borderRadius: "5px",
    boxShadow: 24,
    p: 2,
};

export default function ServerModal({open, handleOpen, handleClose}: TProps) {
    const {cartItems} = useAppSelector(state => state)
    const dispatch = useAppDispatch()
    const router = useRouter()
    const formatter = new Intl.NumberFormat('en-US', {
        style: 'currency',
        currency: 'USD',

        // These options are needed to round to whole numbers if that's what you want.
        //minimumFractionDigits: 0, // (this suffices for whole numbers, but will print 2500.10 as $2,500.1)
        //maximumFractionDigits: 0, // (causes 2500.99 to be printed as $2,501)
    });
    return <Modal

        aria-labelledby="modal-modal-title"
        aria-describedby="modal-modal-description"
        open={open}
        onClose={handleClose}
        closeAfterTransition
    >
        <Fade in={open}>
            <Box sx={customStyle}>
                <Typography id="transition-modal-title" variant="h6" component="h2">
                    Selected items
                </Typography>
                {cartItems.map(i => {
                    return <div key={i.bookId}
                                className={"d-flex mt-2 align-items-center justify-content-between w-100"}>
                        <Typography id="transition-modal-title">
                            {i.title}
                        </Typography>

                        <div className={"d-flex"}>
                            <Typography style={{
                                width: "200px",
                                marginRight: "10px"
                            }} className={"text-center mt-2"}
                                        id="transition-modal-title">
                                {i.price} x {i.count} = {formatter.format(i.price * i.count)}
                            </Typography>
                            <OutlinedInput
                                style={{width: 90, height: "30px", marginRight: 10}}
                                id="outlined-adornment-password"
                                type={"number"}
                                defaultValue={i.count}
                                value={i.count}
                                className={" mt-auto px-0"}
                                onChange={(v) => {
                                    let count = parseInt(v.currentTarget.value) || 1
                                    count < 1 && (count = 1)
                                    dispatch(updateCountItems(
                                        {
                                            count: count,
                                            bookId: i.bookId
                                        }))
                                }}
                            />
                            <Button onClick={() => dispatch(improveCount({bookId: i?.bookId}))}
                                    style={{marginRight: 10}} variant="outlined" color="secondary" className={"p-0"}>
                                <Typography
                                    variant="h6">+</Typography></Button>
                            <Button onClick={() => dispatch(decreaseCount({bookId: i?.bookId}))} variant="outlined"
                                    color="error" className={"p-0"}> <Typography
                                className={"text-danger"} variant="h6"
                            >-</Typography></Button>
                        </div>
                    </div>
                })}
                <div className={"d-flex mt-3 justify-content-end"}>
                    <Typography
                        style={{
                            width: "300px",
                            fontWeight: "600",
                            marginRight: "10px"
                        }} className={"text-center mt-2"}
                        id="transition-modal-title">
                        Total
                        expanse: {formatter.format(cartItems.map(i => i.count * i.price).reduce((partialSum, a) => partialSum + a, 0))}
                    </Typography>
                    <Button onClick={() => router.push("/shipping")} style={{marginRight: "10px"}}
                            variant="contained">Buy</Button>
                    <Button onClick={handleClose} variant="outlined">Cancel</Button>

                </div>
            </Box>
        </Fade>
    </Modal>
}
type TProps = {
    handleOpen: () => void
    handleClose: () => void
    open: boolean
}
