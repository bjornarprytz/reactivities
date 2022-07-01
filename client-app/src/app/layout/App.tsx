import React, { Fragment, useEffect, useState } from 'react';
import axios from 'axios';
import { Container, List } from 'semantic-ui-react';
import { Reactivity } from '../models/reactivity';
import NavBar from './NavBar';
import ReactivityDashboard from '../../features/activities/dashboard/ReactivityDashboard';

function App() {
  const [activities, setActivities] = useState<Reactivity[]>([]);

  useEffect(() => {
    axios.get<Reactivity[]>('http://localhost:5182/api/activities').then(response => {
      setActivities(response.data);
    })
  }, []);

  return (
    <>
      <NavBar />

      <Container style={{marginTop: '7em'}}>

        <ReactivityDashboard activities={activities} />
      </Container>
    </>
  );
}

export default App;
