/* eslint-disable no-debugger */
/* eslint-disable no-console */
/* eslint-disable @typescript-eslint/no-explicit-any */
/* eslint-disable react/jsx-props-no-spreading */
import * as React from 'react';
import { FieldValues, useForm } from 'react-hook-form';
import { Link as RouterLink, useLocation, useNavigate } from 'react-router-dom';
import Avatar from '@mui/material/Avatar';
import TextField from '@mui/material/TextField';
import { LoadingButton } from '@mui/lab';
import Link from '@mui/material/Link';
import Grid from '@mui/material/Grid';
import Box from '@mui/material/Box';
import LockOutlinedIcon from '@mui/icons-material/LockOutlined';
import Typography from '@mui/material/Typography';
import Container from '@mui/material/Container';
import Copyright from './Copyright';
import { useAppDispatch } from '../../app/store';
import { registerUser } from './AccountSlice';
import { RegisterCommand } from '../../app/api/web-api-dtos';

export default function SignUp(): React.ReactElement {
    const navigate = useNavigate();
    const location = useLocation();
    const dispatch = useAppDispatch();

    const {
        register,
        handleSubmit,
        setError,
        formState: { isSubmitting, errors, isValid },
    } = useForm<RegisterCommand>({ mode: 'all', reValidateMode: 'onChange' });

    function handleApiErrors(apiErrors: any) {
        Object.keys(apiErrors).forEach((key) => {
            Object.values(apiErrors[key]).forEach((apiError: any) => {
                switch (key.toLowerCase()) {
                    case 'username':
                        setError('username', { message: apiError });
                        break;
                    case 'email':
                        setError('email', { message: apiError });
                        break;
                    case 'password':
                        setError('password', { message: apiError });
                        break;
                    default:
                        break;
                }
            });
        });
    }

    async function submitForm(registerDto: FieldValues) {
        await dispatch(registerUser(registerDto as RegisterCommand)).then(({ payload }: any) => {
            if ({}.hasOwnProperty.call(payload, 'error') && {}.hasOwnProperty.call(payload.error, 'errors')) {
                handleApiErrors(payload.error.errors);
            } else {
                navigate(`/registerSuccess?email=${registerDto.email}`, { state: { from: location.pathname } });
            }
        });
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
                    Sign up
                </Typography>
                <Box component="form" onSubmit={handleSubmit(submitForm)} noValidate sx={{ mt: 3 }}>
                    <Grid container spacing={2}>
                        <Grid item xs={12}>
                            <TextField
                                margin="normal"
                                fullWidth
                                label="Username"
                                {...register('username', {
                                    required: 'username required',
                                    maxLength: {
                                        value: 15,
                                        message: `Username can't exceed 15 characters`,
                                    },
                                })}
                                error={!!errors.username}
                                helperText={errors.username?.message}
                            />
                        </Grid>
                        <Grid item xs={12}>
                            <TextField
                                margin="normal"
                                fullWidth
                                label="Email Address"
                                {...register('email', {
                                    required: 'email required',
                                    //     pattern: {
                                    //         value: /^[A-Za-z0-9._%+-]+@test.com$/,
                                    //         message: `The email isn't valid, please enter an 'positivethinking.lu' email`,
                                    //     },
                                })}
                                error={!!errors.email}
                                helperText={errors.email?.message}
                            />
                        </Grid>
                        <Grid item xs={12}>
                            <TextField
                                margin="normal"
                                fullWidth
                                label="Password"
                                type="password"
                                {...register('password', {
                                    required: 'password required',
                                    pattern: {
                                        value: /^(?=.*?[A-Z])(?=.*?[a-z])(?=.*?[0-9])(?=.*?[#?!@$%^&*-]).{8,}$/,
                                        message: `The password isn't valid, it should contain at least one upperase letter, one lowercase letter, one number thus one special character`,
                                    },
                                })}
                                error={!!errors.password}
                                helperText={errors.password?.message}
                            />
                        </Grid>
                    </Grid>
                    <LoadingButton
                        disabled={!isValid}
                        loading={isSubmitting}
                        type="submit"
                        fullWidth
                        variant="contained"
                        sx={{ mt: 3, mb: 2 }}
                    >
                        Sign Up
                    </LoadingButton>
                    <Grid container justifyContent="flex-end">
                        <Grid item>
                            <Link component={RouterLink} to="/login" variant="body2">
                                Already have an account? Sign in
                            </Link>
                        </Grid>
                    </Grid>
                </Box>
            </Box>
            <Copyright sx={{ mt: 5 }} />
        </Container>
    );
}
