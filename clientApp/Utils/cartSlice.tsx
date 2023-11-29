import {createSlice, PayloadAction} from "@reduxjs/toolkit";

interface CartSlice {
    bookId: number,
    type: string,
    title: string,
    price: number,
    sales:any
    image:any
    publishedDate: Date
    count: number
    slug: number
}

// Define the initial state using that type
const initialState: CartSlice[] =[]
export const cartSlice = createSlice({
    name: 'changeMode',
    initialState,
    reducers: {
        updateCountItems: (state, action: PayloadAction<any>) => {
            if (action.payload.count > 0) {
               return state.map(element => {
                    if (element.bookId == action.payload.bookId) {
                        return {
                            ...element,
                            count: action.payload.count
                        };
                    }
                    return element;
                });
            } else return  state.filter(i => i.bookId == action.payload)
        },
        addToCartItems: (state, action: PayloadAction<any>) => {
            if (state.find(i => i.bookId == action.payload.bookId))
                return state = state.map(element => {
                    if (element.bookId == action.payload.bookId) {
                        return {
                            ...element,
                            count: element.count + 1
                        };
                    }
                    return element;
                });

            else state.push({
                ...action.payload,
                count: 1
            })
        },
        removeItemsFromCart: (state, action: PayloadAction<any>) => {
            return state = state.filter(i => i.bookId == action.payload)
        },
        improveCount: (state, action: PayloadAction<any>) => {
            return state = state.map(element => {
                if (element.bookId == action.payload.bookId) {
                    return {
                        ...element,
                        count: element.count + 1
                    };
                }
                return element;
            });
        },
        decreaseCount: (state, action: PayloadAction<any>) => {
            let list: CartSlice[]= state.map(element => {
                if (element.bookId == action.payload.bookId) {
                    if (element.count > 1)
                        return {
                            ...element,
                            count: element.count - 1
                        };
                }
                return element;
            })||[];
            return state = list.filter(i => i)||[]
        },
        removeCartItems: (state, action: PayloadAction<any>) => {
            return state = []
        }
    }
})
export const {addToCartItems, removeCartItems, updateCountItems, improveCount, decreaseCount} = cartSlice.actions
export default cartSlice.reducer

