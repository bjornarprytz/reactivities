import { observer } from "mobx-react-lite";
import { useEffect } from "react";
import { useLocation, Routes, Route } from "react-router-dom";
import { Container } from "semantic-ui-react";
import ReactivityDashboard from "../../features/activities/dashboard/ReactivityDashboard";
import ReactivityDetails from "../../features/activities/details/ReactivityDetails";
import ReactivityForm from "../../features/activities/form/ReactivityForm";
import NotFound from "../../features/errors/NotFound";
import ServerError from "../../features/errors/ServerError";
import TestErrors from "../../features/errors/TestError";
import ProfilePage from "../../features/profiles/ProfilePage";
import { useStore } from "../stores/store";
import LoadingComponent from "./LoadingComponents";
import NavBar from "./NavBar";

export default observer(function AppRoutes() {
    const location = useLocation();
    const {
            commonStore:  {token, appLoaded, setAppLoaded}, 
            userStore: {getUser}
          } = useStore();
  
    useEffect(() => {
      if (token){
        getUser().finally(() => setAppLoaded());
      } else {
        setAppLoaded();
      }
    }, [token, getUser, setAppLoaded])
  
  
    if (!appLoaded) return <LoadingComponent content='Loading app...' />
  
    return (
      <>
        <NavBar />
        <Container style={{marginTop: '7em'}}>
          <Routes>
            <Route path='/activities' element={<ReactivityDashboard />}/>
            <Route path='/activities/:id' element={<ReactivityDetails />}/>
            <Route path='/createActivity' element={<ReactivityForm key={location.key} />}/>
            <Route path='/manage/:id' element={<ReactivityForm key={location.key}/>} />
            <Route path='/profiles/:username' element={<ProfilePage />} />
            <Route path='/errors' element={<TestErrors />}/>
            <Route path='/server-error' element={<ServerError />}/>
            
            <Route path='*' element={<NotFound />} />
          </Routes>
        </Container>
      </>
    );
  })