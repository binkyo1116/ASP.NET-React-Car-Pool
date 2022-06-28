/* eslint-disable @typescript-eslint/no-explicit-any */
/* eslint-disable no-console */
/* eslint-disable react/jsx-props-no-spreading */
import * as React from 'react';
import { FieldValues, useForm } from 'react-hook-form';
import { Link as RouterLink } from 'react-router-dom';
import Avatar from '@mui/material/Avatar';
import TextField from '@mui/material/TextField';
import Link from '@mui/material/Link';
import Grid from '@mui/material/Grid';
import LoadingButton from '@mui/lab/LoadingButton';
import Box from '@mui/material/Box';
import Typography from '@mui/material/Typography';
import Container from '@mui/material/Container';
import { Password } from '@mui/icons-material';
import { useState } from 'react';
import { forgotPassword } from './AccountSlice';
import { useAppDispatch } from '../../app/store';
import Copyright from './Copyright';

export default function ForgotPassword(): React.ReactElement {
    const dispatch = useAppDispatch();
    const [emailSent, setEmailSent] = useState<boolean>(false);

    const {
        register,
        getValues,
        setError,
        handleSubmit,
        formState: { isSubmitting, errors, isValid },
    } = useForm<{ email: string; confirmEmail: string }>({ mode: 'onTouched', reValidateMode: 'onChange' });

    function handleApiErrors(apiErrors: any) {
        Object.keys(apiErrors).forEach((key) => {
            Object.values(apiErrors[key]).forEach((apiError: any) => {
                switch (key.toLowerCase()) {
                    case 'email':
                        setError('email', { message: apiError });
                        break;

                    default:
                        break;
                }
            });
        });
    }

    async function submitForm(values: FieldValues) {
        await dispatch(forgotPassword(values.email as string)).then(({ payload }: any) => {
            if ({}.hasOwnProperty.call(payload, 'error')) {
                if ({}.hasOwnProperty.call(payload.error, 'errors')) {
                    handleApiErrors(payload.error.errors);
                }
            } else {
                setEmailSent(true);
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
                    <Password />
                </Avatar>
                <Typography component="h1" variant="h5">
                    Password Forgotten
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
                        label="Confirm Email Address"
                        {...register('confirmEmail', {
                            required: 'confirm email required',
                            validate: (value) => value === getValues('email') || `The emails aren't identical`,
                        })}
                        error={!!errors.confirmEmail}
                        helperText={errors.confirmEmail?.message}
                    />
                    <LoadingButton
                        disabled={!isValid || emailSent}
                        loading={isSubmitting}
                        type="submit"
                        fullWidth
                        variant="contained"
                        sx={{ mt: 3, mb: 2 }}
                    >
                        Send Email
                    </LoadingButton>
                    {emailSent && (
                        <Typography mt={2} variant="subtitle2" fontWeight="bold">
                            Please check your email (including junk email) for the verification email
                        </Typography>
                    )}
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
