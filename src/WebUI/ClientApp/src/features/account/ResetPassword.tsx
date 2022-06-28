/* eslint-disable @typescript-eslint/no-explicit-any */
// resetPassword

/* eslint-disable no-console */
/* eslint-disable react/jsx-props-no-spreading */
import * as React from 'react';
import { FieldValues, useForm } from 'react-hook-form';
import { Link as RouterLink, useNavigate } from 'react-router-dom';
import Avatar from '@mui/material/Avatar';
import TextField from '@mui/material/TextField';
import Link from '@mui/material/Link';
import Grid from '@mui/material/Grid';
import LoadingButton from '@mui/lab/LoadingButton';
import Box from '@mui/material/Box';
import LockReset from '@mui/icons-material/LockOutlined';
import Typography from '@mui/material/Typography';
import Container from '@mui/material/Container';
import { ResetPasswordCommand } from '../../app/api/web-api-dtos';
import { resetPassword } from './AccountSlice';
import { useAppDispatch } from '../../app/store';
import Copyright from './Copyright';
import useQuery from '../../app/util/hooks';

export default function ResetPassword(): React.ReactElement {
    const navigate = useNavigate();
    const dispatch = useAppDispatch();

    const email = useQuery().get('email') as string;
    const token = useQuery().get('token') as string;

    const {
        register,
        getValues,
        handleSubmit,
        setError,
        formState: { isSubmitting, errors, isValid },
    } = useForm<{ password: string; confirmPassword: string }>({ mode: 'onTouched', reValidateMode: 'onChange' });

    function handleApiErrors(apiErrors: any) {
        Object.keys(apiErrors).forEach((key) => {
            Object.values(apiErrors[key]).forEach((apiError: any) => {
                switch (key.toLowerCase()) {
                    case 'email':
                        setError('password', { message: apiError });
                        break;

                    default:
                        break;
                }
            });
        });
    }

    async function submitForm({ password }: FieldValues) {
        const command: ResetPasswordCommand = {
            email,
            token,
            password,
        };
        await dispatch(resetPassword(command)).then(({ payload }: any) => {
            console.log(payload);
            if ({}.hasOwnProperty.call(payload, 'error')) {
                if ({}.hasOwnProperty.call(payload.error, 'errors')) {
                    handleApiErrors(payload.error.errors);
                }
            } else {
                navigate('/login');
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
                    <LockReset />
                </Avatar>
                <Typography component="h1" variant="h5">
                    Reset Password
                </Typography>
                <Box component="form" onSubmit={handleSubmit(submitForm)} noValidate sx={{ mt: 1 }}>
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
                    <TextField
                        margin="normal"
                        fullWidth
                        label="Confirm Password"
                        type="password"
                        {...register('confirmPassword', {
                            required: 'confirm password required',
                            validate: (value) => value === getValues('password') || `The passwords aren't identical`,
                        })}
                        error={!!errors.confirmPassword}
                        helperText={errors.confirmPassword?.message}
                    />
                    <LoadingButton
                        disabled={!isValid}
                        loading={isSubmitting}
                        type="submit"
                        fullWidth
                        variant="contained"
                        sx={{ mt: 3, mb: 2 }}
                    >
                        Reset Password
                    </LoadingButton>
                    <Grid container>
                        <Grid item xs>
                            <Link component={RouterLink} to="/login" variant="body2">
                                Back to Sign-in?
                            </Link>
                        </Grid>
                    </Grid>
                </Box>
            </Box>
            <Copyright sx={{ mt: 8, mb: 4 }} />
        </Container>
    );
}
