import {createSlice, PayloadAction} from "@reduxjs/toolkit";
import {RootState} from "./Store";
import Cookies from 'js-cookie'

interface ModeSlice {
    darkMode:boolean
}

// Define the initial state using that type
const initialState: ModeSlice = {
    darkMode: Cookies.get("darkMode") === "true"
}
export const modeSlice = createSlice({
    name: 'changeMode',
    // `createSlice` will infer the state type from the `initialState` argument
    initialState,
    reducers: {
        changeThemeMode: (state, action: PayloadAction<boolean>) => {
            state.darkMode = action.payload
        }
    }
})
export const { changeThemeMode } = modeSlice.actions
export const selectMode = (state: RootState) => state?.modeSlice
export default modeSlice.reducer

