/* eslint-disable no-console */
/* eslint-disable no-debugger */
/* eslint-disable @typescript-eslint/no-use-before-define */
/* eslint-disable @typescript-eslint/no-explicit-any */
import { createAsyncThunk, createSlice, isAnyOf } from '@reduxjs/toolkit';
import { toast } from 'react-toastify';
import agent from '../../app/api/agent';
import { LoginQuery, RegisterCommand, ResetPasswordCommand, UserDto } from '../../app/api/web-api-dtos';

interface AccountState {
    user: UserDto | null;
    loading: boolean;
}

const initialState: AccountState = {
    user: null,
    loading: true,
};

export const registerUser = createAsyncThunk<UserDto, RegisterCommand>(
    'account/registerUser',
    async (registerDto, thunkAPI: any) => {
        try {
            return await agent.Account.register(registerDto);
        } catch (error: any) {
            return thunkAPI.rejectWithValue({ error: error.data });
        }
    },
);

export const signInUser = createAsyncThunk<UserDto, LoginQuery>(
    'account/signInUser',
    async (loginDto, thunkAPI: any) => {
        try {
            const user = await agent.Account.login(loginDto);
            localStorage.setItem('user', JSON.stringify(user));
            return user;
        } catch (error: any) {
            return thunkAPI.rejectWithValue({ error: error.data });
        }
    },
);

export const forgotPassword = createAsyncThunk<void, string>('account/forgotPassword', async (email, thunkAPI: any) => {
    try {
        return await agent.Account.forgotPassword(email);
    } catch (error: any) {
        return thunkAPI.rejectWithValue({ error: error.data });
    }
});

export const resetPassword = createAsyncThunk<void, ResetPasswordCommand>(
    'account/resetPassword',
    async (command, thunkAPI: any) => {
        try {
            return await agent.Account.resetPassword(command);
        } catch (error: any) {
            return thunkAPI.rejectWithValue({ error: error.data });
        }
    },
);

export const fetchCurrentUser = createAsyncThunk<UserDto>(
    'account/fetchCurrentUser',
    async (_, thunkAPI: any) => {
        thunkAPI.dispatch(setLoading(true));
        const token = localStorage.getItem('user');
        if (token) thunkAPI.dispatch(setUser(JSON.parse(token)));
        try {
            const user = await agent.Account.currentUser();
            localStorage.setItem('user', JSON.stringify(user));
            return user;
        } catch (error: any) {
            return thunkAPI.rejectWithValue({ error: error.data });
        }
    },
    {
        condition: () => {
            if (!localStorage.getItem('user')) return false;
            return true;
        },
        dispatchConditionRejection: true,
    },
);

// eslint-disable-next-line import/prefer-default-export
export const accountSlice = createSlice({
    name: 'account',
    initialState,
    reducers: {
        signOut: (state) => {
            state.user = null;
            localStorage.removeItem('user');
        },
        setUser: (state, action) => {
            state.user = action.payload;
        },
        setLoading: (state, action) => {
            state.loading = action.payload;
        },
    },
    extraReducers: (builder) => {
        builder.addCase(fetchCurrentUser.fulfilled, (state, action) => {
            state.user = action.payload;
            state.loading = false;
        });
        builder.addCase(fetchCurrentUser.rejected, (state, { error }) => {
            if (!({}.hasOwnProperty.call(error, 'name') && error.name === 'ConditionError')) {
                toast.error('Session expired - please login again');
            }
            state.user = null;
            state.loading = false;
            localStorage.removeItem('user');
        });
        builder.addCase(signInUser.rejected, (_, action: any) => {
            console.log(action);
            if ({}.hasOwnProperty.call(action, 'payload') && {}.hasOwnProperty.call(action.payload, 'error')) {
                toast.error(action.payload.error);
            } else if ({}.hasOwnProperty.call(action, 'error')) {
                toast.error(action.error);
            }
            localStorage.removeItem('user');
        });
        builder.addMatcher(isAnyOf(signInUser.fulfilled), (state, action) => {
            state.user = action.payload;
        });
    },
});

export const { signOut, setUser, setLoading } = accountSlice.actions;
