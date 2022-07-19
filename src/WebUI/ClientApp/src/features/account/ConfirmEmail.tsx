/* eslint-disable @typescript-eslint/no-explicit-any */
import React, { useEffect, useState } from 'react';
import { toast } from 'react-toastify';
import Box from '@mui/material/Box';
import EmailIcon from '@mui/icons-material/Email';
import Typography from '@mui/material/Typography';
import Container from '@mui/material/Container';
import { LoadingButton } from '@mui/lab';
import { Button } from '@mui/material';
import { useNavigate } from 'react-router-dom';
import useQuery from '../../app/util/hooks';
import agent from '../../app/api/agent';

export default function ConfirmEmail(): React.ReactElement {
    const [loading, setLoading] = useState(false);
    const [disabled, setDisabled] = useState(false);

    const navigate = useNavigate();
    const email = useQuery().get('email') as string;
    const token = useQuery().get('token') as string;
    let emailTimer: any = null;

    const Status = {
        Verifying: 'Verifying',
        Failed: 'Failed',
        Success: 'Success',
    };

    const [status, setStatus] = useState(Status.Verifying);

    

    useEffect(() => {
        agent.Account.verifyEmail(token, email)
            .then(() => {
                setStatus(Status.Success);
            })
            .catch(() => {
                setStatus(Status.Failed);
            });
        return () => {
            if (emailTimer) clearTimeout(emailTimer);
        };
    }, [Status.Failed, Status.Success, email, emailTimer, token]);

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

    function getbody(): React.ReactElement {
        switch (status) {
            case Status.Verifying:
                return <p>Verifying...</p>;
            case Status.Failed:
                return (
                    <>
                        <Typography variant="subtitle2" sx={{ mt: 5 }} align="inherit">
                            Verification failed. You can try resending the verify link to your email
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
                );
            default:
                return (
                    <div>
                        <Typography variant="subtitle2" sx={{ mt: 5 }}>
                            Email has been verified - you can now log in
                        </Typography>
                        <Button
                            onClick={() => {
                                navigate('/login');
                            }}
                            variant="contained"
                            sx={{ mb: 2 }}
                        >
                            Redirect to Login
                        </Button>
                    </div>
                );
                break;
        }
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
                <EmailIcon color="success" sx={{ fontSize: 60 }} />
                <Typography component="h1" variant="h4" color="green" sx={{ fontWeight: 'bold' }}>
                    Email Verification
                </Typography>
                {getbody()}
            </Box>
        </Container>
    );
}
