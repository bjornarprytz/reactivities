import React, { Fragment, useEffect, useState } from 'react';
import axios from 'axios';
import { Container, List } from 'semantic-ui-react';
import { Reactivity } from '../models/reactivity';
import NavBar from './NavBar';
import ReactivityDashboard from '../../features/activities/dashboard/ReactivityDashboard';

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
        />
      </Container>
    </>
  );
}

export default App;
