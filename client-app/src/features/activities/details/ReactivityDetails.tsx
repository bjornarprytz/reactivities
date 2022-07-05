import { observer } from 'mobx-react-lite';
import React, { useEffect } from 'react';
import { Link, useParams } from 'react-router-dom';
import { Button, Card, Grid, Image } from 'semantic-ui-react';
import LoadingComponent from '../../../app/layout/LoadingComponents';
import { useStore } from '../../../app/stores/store';
import ReactivityDetailedChat from './ReactivityDetailedChat';
import ReactivityDetailedHeader from './ReactivityDetailedHeader';
import ReactivityDetailedInfo from './ReactivityDetailedInfo';
import ReactivityDetailedSidebar from './ReactivityDetailedSidebar';


export default observer(function ReactivityDetails() {
    const {activityStore} = useStore();
    const {selectedActivity: activity, loadActivity, loadingInitial} = activityStore;
    const {id} = useParams<{id: string}>();

    useEffect(() => {
        if (id) loadActivity(id);
    }, [id, loadActivity])

    if (loadingInitial || !activity) return <LoadingComponent />;

    return (
        <Grid>
            <Grid.Column width={10}>
                <ReactivityDetailedHeader activity={activity} />
                <ReactivityDetailedInfo activity={activity} />
                <ReactivityDetailedChat />
            </Grid.Column>
            <Grid.Column width={6}>
                <ReactivityDetailedSidebar />

            </Grid.Column>
        </Grid>
    )
})