import React from 'react';
import ReactDOM from 'react-dom';
import { BrowserRouter, Route, Switch } from 'react-router-dom';
import { Auth0Provider } from '@auth0/auth0-react';
import Home from './components/pages/Home';
import Shows from './components/pages/Shows';
import Today from './components/pages/Today';
import Search from './components/pages/Search';
import Login from './components/pages/Login';
import AdminArea from './components/pages/AdminArea';
import reportWebVitals from './reportWebVitals';
import { DOMAIN, CLIENT_ID } from './helpers/constants';
import './index.css';

ReactDOM.render(
  <React.StrictMode>
    <App/>
  </React.StrictMode>,
  document.querySelector('#root')
);

function App() {
  return (
    <Auth0Provider domain={DOMAIN} clientId={CLIENT_ID} redirectUri={`${window.location.origin}/admin`}>
      <BrowserRouter>
        <Switch>
          <Route exact path="/" component={Home}/>
          <Route exact path="/shows" component={Shows}/>
          <Route exact path="/today" component={Today}/>
          <Route exact path="/search" component={Search}/>
          <Route exact path="/login" component={Login}/>
          <Route exact path="/admin/halls" component={AdminArea}/>
          <Route exact path="/admin/movies" component={AdminArea}/>
          <Route exact path="/admin/shows" component={AdminArea}/>
          <Route exact path="/admin/" component={AdminArea}/>
        </Switch>
      </BrowserRouter>
    </Auth0Provider>
  );
}

// If you want to start measuring performance in your app, pass a function
// to log results (for example: reportWebVitals(console.log))
// or send to an analytics endpoint. Learn more: https://bit.ly/CRA-vitals
reportWebVitals();
