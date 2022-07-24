import { observer } from "mobx-react-lite";
import { CardGroup } from "semantic-ui-react";
import { UserActivity } from "../../app/models/profile";
import ReactivityInfoCard from "./ReactivityInfoCard";

interface Props {
    activities: UserActivity[];
}

export default observer(function ReactivityInfoCardList({activities}: Props) {
    return (
        <CardGroup itemsPerRow={4}>
            {activities.map(activity => (
                <ReactivityInfoCard key={activity.id} activity={activity} />
            ))}
        </CardGroup>
    )
})