import {createSlice, PayloadAction} from "@reduxjs/toolkit";

interface TUser {
    accessToken: string
    refreshToken: string
    token: string
    userId: number
    emailAddress: string
    password: string
    firstName: string
    middleName: string
    lastName: string
    roleId: number
    refreshTokens: any[]

}
const initialState:TUser={
    accessToken: "",
    refreshToken: "",
    token: "",
    userId: 0,
    emailAddress: "",
    password: "",
    firstName: "",
    middleName: "",
    lastName: "",
    roleId: 0,
    refreshTokens: []
}
// Define the initial state using that type
export const userSlice = createSlice({
    name: 'setUserData',
    initialState,
    reducers: {
        setUserData: (state, action) => action.payload,
        removeUserData: (state, action: PayloadAction<any>) =>  initialState
    }
})
export const {setUserData, removeUserData} = userSlice.actions
export default userSlice.reducer

