import React from 'react';
import { Grid } from 'semantic-ui-react';
import { Reactivity } from '../../../app/models/reactivity';
import ReactivityDetails from '../details/ReactivityDetails';
import ReactivityForm from '../form/ReactivityForm';
import ReactivityList from './ReactivityList';

interface Props {
    activities: Reactivity[];
}

export default function ReactivityDashboard({ activities } : Props ){
    return (
        <Grid>
            <Grid.Column width='10'>
                <ReactivityList activities={activities} />
            </Grid.Column>
            <Grid.Column width='6'>
                {activities[0] && <ReactivityDetails activity={activities[0]} />}
                {activities[0] && <ReactivityForm activity={activities[0]} />}
            </Grid.Column>
        </Grid>
    )
}