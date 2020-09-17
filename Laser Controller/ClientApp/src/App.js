import React, { Component } from 'react';
import { ToastContainer } from 'react-toastify';
import 'react-toastify/dist/ReactToastify.css';
import { Route } from 'react-router-dom';
import Dashboard from './components/dashboard/dashboard';
import Audio from './components/audio/audio';
import Shows from './components/shows/shows';
import Patterns from './components/patterns/patterns';
import Settings from './components/settings/settings';
import GameDashboard from './components/games/game-dashboard';

import './app.css'

export default class App extends Component {
  static displayName = App.name;

  render() {
    return (
      <div>
        <ToastContainer autoClose={10000} position="top-center"/>
        <Route exact path='/' component={Dashboard} />
        <Route exact path='/audio' component={Audio} />
        <Route exact path='/shows' component={Shows} />
        <Route exact path='/patterns' component={Patterns} />
        <Route exact path='/settings' component={Settings} />
        <Route exact path='/games' component={GameDashboard} />
      </div>
    );
  }
}
