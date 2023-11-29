import modeSlice from "./modeSlice";
import {configureStore} from '@reduxjs/toolkit'
import cartSlice from "./cartSlice";
import userSlice from "./userSlice";
import shippingSlice from "./shippingSlice";

export const store = configureStore({
    reducer: {
        modeSlice: modeSlice,
        cartItems: cartSlice,
        userData: userSlice,
        shippingData: shippingSlice,
    }
})

// Infer the `RootState` and `AppDispatch` types from the store itself
export type RootState = ReturnType<typeof store.getState>
// Inferred type: {posts: PostsState, comments: CommentsState, users: UsersState}
export type AppDispatch = typeof store.dispatch
