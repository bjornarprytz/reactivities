import React from 'react';
import { Grid, List } from 'semantic-ui-react';
import { Reactivity } from '../../../app/models/reactivity';
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
        </Grid>
    )
}