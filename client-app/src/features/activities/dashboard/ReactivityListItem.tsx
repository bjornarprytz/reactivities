import { format } from 'date-fns';
import { observer } from 'mobx-react-lite';
import { Link } from 'react-router-dom';
import { Button, Icon, Item, Label, Segment } from 'semantic-ui-react';
import { Reactivity } from '../../../app/models/reactivity';
import ReactivityListItemAttendee from './ReactivityListItemAttendee';

interface Props {
    activity: Reactivity;
}

export default observer(function ReactivityListItem({activity}: Props) {
    return (
        <Segment.Group>
            <Segment>
                {activity.isCancelled && 
                    <Label attached='top' color='red' content='Cancelled' style={{textAlign: 'center'}} />
                }
                <Item.Group>
                    <Item>
                        <Item.Image as={Link} to={`/profiles/${activity.hostUsername}`} style={{marginBottom: 5}} size='tiny' circular src={activity.host?.image || '/assets/user.png'} />
                        <Item.Content>
                            <Item.Header as={Link} to={`/activities/${activity.id}`}>
                                {activity.title}
                            </Item.Header>
                            <Item.Description>
                                Hosted by <Link to={`/profiles/${activity.hostUsername}`}>{activity.host?.displayName}</Link>
                            </Item.Description>
                            {activity.isHost && (
                                <Item.Description>
                                    <Label basic color='orange'>
                                        You are hosting this activity
                                    </Label>
                                </Item.Description>
                            )}
                            {activity.isGoing && !activity.isHost && (
                                <Item.Description>
                                    <Label basic color='green'>
                                        You are going to this activity
                                    </Label>
                                </Item.Description>
                            )}
                        </Item.Content>
                    </Item>
                </Item.Group>
            </Segment>
            <Segment>
                <span>
                    <Icon name='clock' /> {format(activity.date!, 'dd MMM yyyy h:mm aa')}
                    <Icon name='marker' /> {activity.venue}
                </span>
            </Segment>
            <Segment secondary>
                <ReactivityListItemAttendee attendees={activity.attendees!} />
            </Segment>
            <Segment clearing>
                <span>
                    {activity.description}
                </span>
                <Button 
                    as={Link} 
                    to={`/activities/${activity.id}`}
                    color='teal'
                    floated='right'
                    content='View'
                />
            </Segment>
        </Segment.Group>
    );
})