import { Link, Typography } from '@mui/material';

interface ICopyrightProps {
    sx: {
        mt: number;
        mb?: number;
    };
}
export default function Copyright({ sx }: ICopyrightProps): React.ReactElement {
    return (
        <Typography variant="body2" color="text.secondary" align="center" sx={sx}>
            {'Copyright © '}
            <Link color="inherit" href="https://positivethinking.tech/fr/" target="_blank">
                {`Positive Thinking company `}
            </Link>
            {new Date().getFullYear()}.
        </Typography>
    );
}
