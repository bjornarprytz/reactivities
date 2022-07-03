import { Container } from 'semantic-ui-react';
import NavBar from './NavBar';
import ReactivityDashboard from '../../features/activities/dashboard/ReactivityDashboard';
import { observer } from 'mobx-react-lite';
import HomePage from '../../features/home/HomePage';
import ReactivityForm from '../../features/activities/form/ReactivityForm';
import { Route, Routes } from 'react-router-dom';
import NotFound from '../../features/notfound/NotFound';

function App() {
  return (
    <>
      <NavBar />

      <Container style={{marginTop: '7em'}}>
        <Routes>
          <Route path='/' element={<HomePage/>} />
          <Route path='/activities' element={<ReactivityDashboard />} />
          <Route path='/createActivity' element={<ReactivityForm />} />
          <Route path='*' element={<NotFound />} />
        </Routes>
      </Container>
    </>
  );
}

export default observer(App);
