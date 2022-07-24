import { observer } from "mobx-react-lite";
import { useEffect } from "react";
import { Grid, Header, Tab } from "semantic-ui-react";
import LoadingComponent from "../../app/layout/LoadingComponents";
import { Profile } from "../../app/models/profile";
import { useStore } from "../../app/stores/store";
import ReactivityInfoCardList from "./ReactivityInfoCardList";

interface Props {
    profile: Profile;
}

export default observer(function ProfileActivitiesOverview({profile}: Props) {
    const {profileStore: {futureActivities, pastActivities, hostingActivities, loadUserActivities, loadingUserActivities}} = useStore();

    useEffect(() => {
        loadUserActivities(profile!.username);
    }, [loadUserActivities, profile])

    const panes = [
        {menuItem: 'Future Events', render: () => <ReactivityInfoCardList activities={futureActivities} /> },
        {menuItem: 'Past Events', render: () => <ReactivityInfoCardList activities={pastActivities} /> },
        {menuItem: 'Hosting', render: () => <ReactivityInfoCardList activities={hostingActivities} /> }
    ]

    if (loadingUserActivities) return <LoadingComponent content="Loading activities..." />

    return (
        <Tab.Pane>
            <Grid>
                <Grid.Column width={16}>
                    <Header floated='left' icon='calendar' content={`Activities`}/>
                    <Tab 
                        menu={{ secondary: true, pointing: true}}
                        panes={panes}
                    />
                </Grid.Column>
            </Grid>
        </Tab.Pane>
    )
})