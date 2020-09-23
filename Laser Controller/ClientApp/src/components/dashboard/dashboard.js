import React, { Component } from 'react';
import Menu from '../shared/menu/menu';
import './dashboard.css';
import DashboardCard from './elements/card/dashboard-card';

class Dashboard extends Component {

    render() {
        return (
            <div>
                <Menu />
                <div id="main">
                    <div className="row dashboard-card-row">
                        <DashboardCard title={"Audio"} description="Let an audio algorithm create patterns based on an audio signal" href="/audio" />
                        <DashboardCard title={"Games"} description="Play a game with the laser" href="/games" />
                    </div>
                </div>
            </div>
        );
    }
}

export default Dashboard;