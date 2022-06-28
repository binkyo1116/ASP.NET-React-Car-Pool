/* eslint-disable no-console */
import { CircularProgress, Container, createTheme, CssBaseline, ThemeProvider } from '@mui/material';
import React, { useEffect } from 'react';
import { Routes, Route, Navigate } from 'react-router-dom';
import { ToastContainer } from 'react-toastify';
import 'react-toastify/dist/ReactToastify.css';
import { fetchCurrentUser } from '../../features/account/AccountSlice';
import ConfirmEmail from '../../features/account/ConfirmEmail';
import ForgotPassword from '../../features/account/ForgotPassword';
import Login from '../../features/account/Login';
import Register from '../../features/account/Register';
import RegisterSuccess from '../../features/account/RegisterSuccess';
import ResetPassword from '../../features/account/ResetPassword';
import { useAppDispatch, useAppSelector } from '../store';
import Main from './main/Main';
import RequireAuth from './RequireAuth';

function App(): React.ReactElement {
    const dispatch = useAppDispatch();
    const { loading, user } = useAppSelector((state) => state.account);

    useEffect(() => {
        dispatch(fetchCurrentUser());
    }, [dispatch]);

    const theme = createTheme();

    return (
        <ThemeProvider theme={theme}>
            <Container component="main" maxWidth={false} disableGutters>
                <ToastContainer position="bottom-right" theme="colored" hideProgressBar />
                <CssBaseline />
                {loading ? (
                    <CircularProgress />
                ) : (
                    <Routes>
                        <Route path="/" element={user ? <Navigate replace to="/events" /> : <Login />} />
                        <Route
                            path="/*"
                            element={
                                user ? (
                                    <RequireAuth>
                                        <Main />
                                    </RequireAuth>
                                ) : (
                                    <Navigate replace to="/login" />
                                )
                            }
                        />
                        <Route path="/login" element={user ? <Navigate replace to="/events" /> : <Login />} />
                        <Route path="/register" element={user ? <Navigate replace to="/events" /> : <Register />} />
                        <Route
                            path="/registerSuccess"
                            element={user ? <Navigate replace to="/events" /> : <RegisterSuccess />}
                        />
                        <Route
                            path="/verifyEmail"
                            element={user ? <Navigate replace to="/events" /> : <ConfirmEmail />}
                        />
                        <Route
                            path="/passwordForgotten"
                            element={user ? <Navigate replace to="/events" /> : <ForgotPassword />}
                        />
                        <Route
                            path="/resetPassword"
                            element={user ? <Navigate replace to="/events" /> : <ResetPassword />}
                        />
                    </Routes>
                )}
            </Container>
        </ThemeProvider>
    );
}

export default App;
