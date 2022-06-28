import { useLocation, Navigate } from 'react-router-dom';
import { useAppSelector } from '../store';

export default function RequireAuth({ children }: { children: JSX.Element }): React.ReactElement {
    const { user } = useAppSelector((state) => state.account);
    const location = useLocation();

    if (!user) {
        return <Navigate to="/login" state={{ from: location.pathname }} />;
    }
    return children;
}
