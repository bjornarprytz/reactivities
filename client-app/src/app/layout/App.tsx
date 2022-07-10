import { observer } from 'mobx-react-lite';
import HomePage from '../../features/home/HomePage';
import { Route, Routes } from 'react-router-dom';
import { ToastContainer } from 'react-toastify';
import AppRoutes from './AppRoutes';
import ModalContainer from '../common/modals/ModalContainer';


export default observer(function App() {
  return (
    <>
      <ToastContainer position='bottom-right' hideProgressBar />
      <ModalContainer />
      <Routes>
        <Route path='/' element={<HomePage />} />
        <Route path='/*' element={<AppRoutes />} />
      </Routes>
    </>
  );
})
