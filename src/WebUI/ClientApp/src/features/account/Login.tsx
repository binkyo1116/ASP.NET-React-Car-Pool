/* eslint-disable no-console */
/* eslint-disable react/jsx-props-no-spreading */
import * as React from 'react';
import { FieldValues, useForm } from 'react-hook-form';
import { Link as RouterLink, useLocation, useNavigate } from 'react-router-dom';
import Avatar from '@mui/material/Avatar';
import TextField from '@mui/material/TextField';
import Link from '@mui/material/Link';
import Grid from '@mui/material/Grid';
import LoadingButton from '@mui/lab/LoadingButton';
import Box from '@mui/material/Box';
import LockOutlinedIcon from '@mui/icons-material/LockOutlined';
import Typography from '@mui/material/Typography';
import Container from '@mui/material/Container';
import { LoginQuery } from '../../app/api/web-api-dtos';
import { signInUser } from './AccountSlice';
import { useAppDispatch } from '../../app/store';
import Copyright from './Copyright';

export default function Login(): React.ReactElement {
    const navigate = useNavigate();
    const location = useLocation();
    const dispatch = useAppDispatch();
    const {
        register,
        handleSubmit,
        formState: { isSubmitting, errors, isValid },
    } = useForm<LoginQuery>({ mode: 'onTouched', reValidateMode: 'onChange' });

    async function submitForm(loginDto: FieldValues) {
        await dispatch(signInUser(loginDto as LoginQuery));
        navigate(location.state && location.state.from !== '/register' ? location.state.from : '/');
    }

    return (
        <Container component="main" maxWidth="xs">
            <Box
                sx={{
                    marginTop: 8,
                    display: 'flex',
                    flexDirection: 'column',
                    alignItems: 'center',
                }}
            >
                <Avatar sx={{ m: 1, bgcolor: 'secondary.main' }}>
                    <LockOutlinedIcon />
                </Avatar>
                <Typography component="h1" variant="h5">
                    Sign in
                </Typography>
                <Box component="form" onSubmit={handleSubmit(submitForm)} noValidate sx={{ mt: 1 }}>
                    <TextField
                        margin="normal"
                        fullWidth
                        label="Email Address"
                        {...register('email', {
                            required: 'email required',
                        })}
                        error={!!errors.email}
                        helperText={errors.email?.message}
                    />
                    <TextField
                        margin="normal"
                        fullWidth
                        label="Password"
                        type="password"
                        {...register('password', {
                            required: 'password required',
                        })}
                        error={!!errors.password}
                        helperText={errors.password?.message}
                    />
                    <LoadingButton
                        disabled={!isValid}
                        loading={isSubmitting}
                        type="submit"
                        fullWidth
                        variant="contained"
                        sx={{ mt: 3, mb: 2 }}
                    >
                        Sign In
                    </LoadingButton>
                    <Grid container>
                        <Grid item xs>
                            <Link component={RouterLink} to="/passwordForgotten" variant="body2">
                                Forgot password?
                            </Link>
                        </Grid>
                        <Grid item>
                            <Link component={RouterLink} to="/register" variant="body2">
                                Don&apos;t have an account? Sign Up
                            </Link>
                        </Grid>
                    </Grid>
                </Box>
            </Box>
            <Copyright sx={{ mt: 8, mb: 4 }} />
        </Container>
    );
}
