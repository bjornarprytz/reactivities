import { observer } from 'mobx-react-lite';
import { useEffect } from 'react';
import { Grid } from 'semantic-ui-react';
import LoadingComponent from '../../../app/layout/LoadingComponents';
import { useStore } from '../../../app/stores/store';
import ReactivityFilters from './ReactivityFilters';
import ReactivityList from './ReactivityList';

export default observer(function ReactivityDashboard() {
    const {activityStore} = useStore();
    const {loadActivities, activityRegistry, loadingInitial} = activityStore;

    useEffect(() => {
      if (activityRegistry.size <= 1) loadActivities();
    }, [loadActivities, activityRegistry.size]);
  
    if (loadingInitial) return <LoadingComponent content='Loading app'/>

    return (
        <Grid>
            <Grid.Column width='10'>
                <ReactivityList />
            </Grid.Column>
            <Grid.Column width='6'>
                <ReactivityFilters />
            </Grid.Column>
        </Grid>
    )
})