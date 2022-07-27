import { format } from "date-fns";
import { observer } from "mobx-react-lite";
import { useEffect } from "react";
import { Link } from "react-router-dom";
import { Card, Image } from "semantic-ui-react";
import { UserActivity } from "../../app/models/profile";

interface Props {
    activity: UserActivity;
}

export default observer(function ReactivityInfoCard({activity}: Props) {
    return (
        <Card as={Link} to={`/activities/${activity.id}`}>
            <Image src={`/assets/categoryImages/${activity.category}.jpg`} />
            <Card.Content textAlign='center'>
                <Card.Header>{activity.title}</Card.Header>
                <Card.Meta>{format(new Date(activity.date), 'do LLL')}</Card.Meta>
                <Card.Meta>{format(new Date(activity.date), 'h:mm a')}</Card.Meta>
            </Card.Content>
        </Card>
    )
})