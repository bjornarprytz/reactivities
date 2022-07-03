import { useEffect, useState } from 'react';
import axios from 'axios';
import { Container } from 'semantic-ui-react';
import { Reactivity } from '../models/reactivity';
import NavBar from './NavBar';
import ReactivityDashboard from '../../features/activities/dashboard/ReactivityDashboard';
import { v4 as uuid } from 'uuid';

function App() {
  const [activities, setActivities] = useState<Reactivity[]>([]);
  const [selectedActivity, setSelectedActivity] = useState<Reactivity | undefined>(undefined);
  const [editMode, setEditMode] = useState(false);

  useEffect(() => {
    axios.get<Reactivity[]>('http://localhost:5182/api/activities').then(response => {
      setActivities(response.data);
    })
  }, []);

  function handleSelectActivity(id: string){
    setSelectedActivity(activities.find(a => a.id === id))
  }

  function handleCancelSelectActivity(){
    setSelectedActivity(undefined);
  }

  function handleFormOpen(id? : string){
    id ? handleSelectActivity(id) : handleCancelSelectActivity();
    setEditMode(true);
  }

  function handleFormClose(){
    setEditMode(false);
  }

  function handleCreateOrEditActivity(activity: Reactivity){
    activity.id 
      ? setActivities([...activities.filter(a => a.id !== activity.id), activity])
      : setActivities([...activities, {...activity, id: uuid()}]);

    setEditMode(false);
    setSelectedActivity(activity);
  }

  function handleDeleteActivity(id: string){
    setActivities([...activities.filter(a => a.id !== id)]);
  }

  return (
    <>
      <NavBar openForm={handleFormOpen} />

      <Container style={{marginTop: '7em'}}>

        <ReactivityDashboard 
          activities={activities}
          selectedActivity={selectedActivity}
          selectActivity={handleSelectActivity}
          cancelSelectActivity={handleCancelSelectActivity}
          editMode={editMode}
          openForm={handleFormOpen}
          closeForm={handleFormClose}
          createOrEdit={handleCreateOrEditActivity}
          deleteActivity={handleDeleteActivity}
        />
      </Container>
    </>
  );
}

export default App;
