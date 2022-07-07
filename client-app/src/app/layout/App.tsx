import { Container } from 'semantic-ui-react';
import NavBar from './NavBar';
import ReactivityDashboard from '../../features/activities/dashboard/ReactivityDashboard';
import { observer } from 'mobx-react-lite';
import HomePage from '../../features/home/HomePage';
import ReactivityForm from '../../features/activities/form/ReactivityForm';
import { Route, Routes, useLocation } from 'react-router-dom';
import NotFound from '../../features/errors/NotFound';
import ReactivityDetails from '../../features/activities/details/ReactivityDetails';
import TestErrors from '../../features/errors/TestError';
import { ToastContainer } from 'react-toastify';
import ServerError from '../../features/errors/ServerError';


function App() {
  return (
    <>
      <ToastContainer position='bottom-right' hideProgressBar />
      <Routes>
        <Route path='/' element={<HomePage />} />
        <Route path='/*' element={<Reactivitites />} />
      </Routes>
    </>
  );
}

function Reactivitites() {
  const location = useLocation();

  return (
    <>
      <NavBar />
      <Container style={{marginTop: '7em'}}>
        <Routes>
          <Route path='/activities' element={<ReactivityDashboard />} />
          <Route path='/activities/:id' element={<ReactivityDetails />} />
          <Route path='/createActivity' element={<ReactivityForm key={location.key} />} />
          <Route path='/manage/:id' element={<ReactivityForm key={location.key}/>} />
          <Route path='/errors' element={<TestErrors />}/>
          <Route path='/server-error' element={<ServerError />}/>
          
          <Route path='*' element={<NotFound />} />
        </Routes>
      </Container>
    </>
  );
}

export default observer(App);
