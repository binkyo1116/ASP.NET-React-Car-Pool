/* eslint-disable @typescript-eslint/no-explicit-any */
import React, { useEffect, useState } from 'react';
import { toast } from 'react-toastify';
import LoadingButton from '@mui/lab/LoadingButton';
import Box from '@mui/material/Box';
import DoneIcon from '@mui/icons-material/Done';
import Typography from '@mui/material/Typography';
import Container from '@mui/material/Container';
import agent from '../../app/api/agent';
import useQuery from '../../app/util/hooks';

export default function RegisterSuccess(): React.ReactElement {
    const [loading, setLoading] = useState(false);
    const [disabled, setDisabled] = useState(true);
    const email = useQuery().get('email') as string;
    let emailTimer: any = null;
    useEffect(() => {
        const timer = setTimeout(() => {
            setDisabled(false);
        }, 3000);
        return () => {
            clearTimeout(timer);
            if (emailTimer) clearTimeout(emailTimer);
        };
    }, [emailTimer]);

    function handleConfirmEmailResend() {
        setLoading(true);
        agent.Account.resendEmailConfirmation(email)
            .then(() => {
                toast.success('Verification email resent - Please check your email');
                setDisabled(true);
                emailTimer = setTimeout(() => {
                    setDisabled(false);
                }, 3000);
            })
            .finally(() => setLoading(false));
    }

    return (
        <Container component="main" maxWidth="lg">
            <Box
                sx={{
                    marginTop: 8,
                    display: 'flex',
                    flexDirection: 'column',
                    alignItems: 'center',
                    justifyContent: 'center',
                }}
            >
                <DoneIcon color="success" sx={{ fontSize: 60 }} />
                <Typography component="h1" variant="h4" color="green" sx={{ fontWeight: 'bold' }}>
                    Register Successful
                </Typography>
                {email && (
                    <>
                        <Typography mt={10} variant="h6" fontWeight="bold">
                            Please check your email (including junk email) for the verification email
                        </Typography>
                        <Typography variant="subtitle2" sx={{ mt: 5 }}>
                            Didn t receive the email ? Click the below button
                        </Typography>
                        <LoadingButton
                            disabled={disabled}
                            loading={loading}
                            onClick={() => {
                                handleConfirmEmailResend();
                            }}
                            variant="contained"
                            sx={{ mb: 2 }}
                        >
                            Resend email
                        </LoadingButton>
                    </>
                )}
            </Box>
        </Container>
    );
}
