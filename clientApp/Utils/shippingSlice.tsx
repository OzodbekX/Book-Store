import {CaseReducer, createSlice, Draft, PayloadAction} from "@reduxjs/toolkit";

type ShippingSlice ={
    receiver: string
    address: string
    city: string
    postalCode: string
    country: string
    secretWord: string
    paymentMethod?: string
}

const initialState:  ShippingSlice = {
    receiver: "",
    address: "",
    city: "",
    postalCode: "",
    country: "",
    secretWord: "",
    paymentMethod: ""
}
export const shippingSlice = createSlice({
    name: 'saveShipping',
    initialState: initialState,
    reducers: {
        saveShippingData:(state, action:PayloadAction<ShippingSlice>) => {
            return action.payload
        },
        changePaymentMethod: (state, action: PayloadAction<string>) => {
           return {
               ...state,
               paymentMethod: action.payload
           }
        },
        changeSingleArea: (state: Draft<ShippingSlice | null>, action: PayloadAction<{ key: keyof ShippingSlice, value: string }>) => {
            if (!!state) {
                state[action.payload.key] = action.payload.value
                return state
            }
        },
        removeShippingData: (state, action: PayloadAction<any>) => initialState
    }
})
export const {removeShippingData, changeSingleArea, changePaymentMethod, saveShippingData} = shippingSlice.actions
export default shippingSlice.reducer

