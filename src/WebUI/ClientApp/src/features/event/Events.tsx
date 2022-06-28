/* eslint-disable @typescript-eslint/no-non-null-assertion */
import React, { useEffect } from 'react';
import ImageList from '@mui/material/ImageList';
import ImageListItem from '@mui/material/ImageListItem';
import ImageListItemBar from '@mui/material/ImageListItemBar';
import IconButton from '@mui/material/IconButton';
import { CircularProgress, Grid } from '@mui/material';
import { useAppDispatch, useAppSelector } from '../../app/store';
import { getEvents } from './EventSlice';

export default function Event(): React.ReactElement {
    const dispatch = useAppDispatch();
    const { events, loading } = useAppSelector((state) => state.event);

    useEffect(() => {
        dispatch(getEvents());
    }, [dispatch]);

    return (
        <>
            {loading ? (
                <CircularProgress />
            ) : (
                <Grid container spacing={2}>
                    <Grid item xs={8} />
                    <Grid item xs={4}>
                        <ImageList sx={{ width: 250, height: `100%` }} cols={1}>
                            {events!.map((event) => (
                                <ImageListItem key={event.url}>
                                    <img
                                        src={`${event.url}?w=248&fit=crop&auto=format`}
                                        srcSet={`${event.url}?w=248&fit=crop&auto=format&dpr=2 2x`}
                                        alt={event.name}
                                        loading="lazy"
                                    />
                                    <ImageListItemBar
                                        title={event.name}
                                        actionIcon={
                                            <IconButton
                                                sx={{ color: 'rgba(255, 255, 255, 0.54)' }}
                                                aria-label={`info about ${event.name}`}
                                            />
                                        }
                                    />
                                </ImageListItem>
                            ))}
                        </ImageList>
                    </Grid>
                </Grid>
            )}
        </>
    );
}
