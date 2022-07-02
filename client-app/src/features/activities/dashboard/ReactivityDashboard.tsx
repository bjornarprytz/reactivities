import React from 'react';
import { Grid } from 'semantic-ui-react';
import { Reactivity } from '../../../app/models/reactivity';
import ReactivityDetails from '../details/ReactivityDetails';
import ReactivityForm from '../form/ReactivityForm';
import ReactivityList from './ReactivityList';

interface Props {
    activities: Reactivity[];
    selectedActivity: Reactivity | undefined;
    selectActivity: (id: string) => void;
    cancelSelectActivity: () => void;
    editMode: boolean;
    openForm: (id: string) => void;
    closeForm: () => void;
}

export default function ReactivityDashboard(
    { 
        activities, 
        selectedActivity, 
        selectActivity, 
        cancelSelectActivity,
        editMode,
        openForm,
        closeForm
        
    } : Props
    ) {
    return (
        <Grid>
            <Grid.Column width='10'>
                <ReactivityList activities={activities} selectActivity={selectActivity} />
            </Grid.Column>
            <Grid.Column width='6'>
                {selectedActivity && !editMode &&
                <ReactivityDetails 
                    activity={selectedActivity}
                    cancelSelectActivity={cancelSelectActivity}
                    openForm={openForm}
                />}
                {editMode && 
                <ReactivityForm 
                    activity={selectedActivity}
                    closeForm={closeForm}
                />}
            </Grid.Column>
        </Grid>
    )
}