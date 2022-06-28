/* eslint-disable @typescript-eslint/explicit-module-boundary-types */
import { configureStore } from '@reduxjs/toolkit';
import { TypedUseSelectorHook, useDispatch, useSelector } from 'react-redux';
import { accountSlice } from '../features/account/AccountSlice';
import { eventSlice } from '../features/event/EventSlice';

export const store = configureStore({
    reducer: {
        account: accountSlice.reducer,
        event: eventSlice.reducer,
    },
});

export type RootState = ReturnType<typeof store.getState>;
export type AppDispatch = typeof store.dispatch;

export const useAppDispatch = () => useDispatch<AppDispatch>();
export const useAppSelector: TypedUseSelectorHook<RootState> = useSelector;
