import React from 'react';
import ReactDOM from 'react-dom/client';
import { unstable_HistoryRouter as HistoryRouter } from 'react-router-dom';
import App from './app/layout/App';
import 'react-calendar/dist/Calendar.css'
import './app/layout/styles.css'
import { store, StoreContext } from './app/stores/store';
import { browserHistory } from './features/history/history';
import reportWebVitals from './reportWebVitals';

const root = ReactDOM.createRoot(
  document.getElementById('root') as HTMLElement
);
root.render(
    <StoreContext.Provider value={store}>
      <HistoryRouter history={browserHistory}>
        <App />
      </HistoryRouter>
    </StoreContext.Provider>
);

// If you want to start measuring performance in your app, pass a function
// to log results (for example: reportWebVitals(console.log))
// or send to an analytics endpoint. Learn more: https://bit.ly/CRA-vitals
reportWebVitals();
