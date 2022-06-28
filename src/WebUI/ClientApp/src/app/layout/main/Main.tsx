import React from 'react';
import { Route, Routes } from 'react-router-dom';
import Events from '../../../features/event/Events';
import Header from './Header';
import NotFound from './NotFound';

export default function Main(): React.ReactElement {
    return (
        <>
            <Header />
            <Routes>
                <Route path="/events" element={<Events />} />
                <Route path="/*" element={<NotFound />} />
            </Routes>
        </>
    );
}
