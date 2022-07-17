import { observer } from "mobx-react-lite"
import { useState } from "react";
import { Button, Grid, Header, Segment, Tab } from "semantic-ui-react"
import { Profile } from "../../app/models/profile";
import { useStore } from "../../app/stores/store";
import ProfileForm from "./ProfileForm";

interface Props {
    profile: Profile;
}

export default observer(function ProfileAbout({profile}: Props) {
    const {profileStore: {isCurrentUser}} = useStore();

    const [editMode, setEditMode] = useState(false);

    return (
        <Tab.Pane>
            <Grid>
                <Grid.Column width={16}>
                <Header floated='left' icon='image' content={`About ${profile.displayName}`}/>
                    {isCurrentUser && (
                        <Button 
                            floated='right' 
                            basic 
                            content={editMode ? 'Cancel' : 'Edit'}
                            onClick={() => setEditMode(!editMode)}
                        />
                    )}
                </Grid.Column>
                <Grid.Column width={16}>
                    {editMode ? (
                        <ProfileForm setEditMode={setEditMode} />
                    ) : (
                        <Segment style={{whiteSpace: 'pre-wrap'}}>
                            {profile.bio}
                        </Segment>
                    )}
                </Grid.Column>
            </Grid>
        </Tab.Pane>
    )
})