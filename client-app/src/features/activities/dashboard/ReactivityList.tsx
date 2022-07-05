import { observer } from 'mobx-react-lite';
import { Fragment } from 'react';
import { Header, Item, Segment } from 'semantic-ui-react';
import { useStore } from '../../../app/stores/store';
import ReactivityListItem from './ReactivityListItem';


export default observer(function ReactivityList() {
    const {activityStore} = useStore();
    const {groupedActivities} = activityStore;


    return(
        <>
            {groupedActivities.map(([group, activities]) => (
                    <Fragment key={group}>
                        <Header sub color='teal'>
                            {group}
                        </Header>
                        {activities.map(activity => 
                            <ReactivityListItem key={activity.id} activity={activity} />
                        )}
                    </Fragment>
                )
            )}
        </>
    )
})