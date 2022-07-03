import { observer } from 'mobx-react-lite';
import { useEffect } from 'react';
import { Grid } from 'semantic-ui-react';
import LoadingComponent from '../../../app/layout/LoadingComponents';
import { useStore } from '../../../app/stores/store';
import ReactivityDetails from '../details/ReactivityDetails';
import ReactivityForm from '../form/ReactivityForm';
import ReactivityList from './ReactivityList';

export default observer(function ReactivityDashboard() {
    const {activityStore} = useStore();
    const {selectedActivity, editMode} = activityStore;

    useEffect(() => {
      activityStore.loadActivities();
    }, [activityStore]);
  
    if (activityStore.loadingInitial) return <LoadingComponent content='Loading app'/>

    return (
        <Grid>
            <Grid.Column width='10'>
                <ReactivityList />
            </Grid.Column>
            <Grid.Column width='6'>
                {selectedActivity && !editMode &&
                <ReactivityDetails/>}
                {editMode && 
                <ReactivityForm />}
            </Grid.Column>
        </Grid>
    )
})