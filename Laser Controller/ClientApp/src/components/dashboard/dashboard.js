import React, { Component } from 'react';
import Menu from '../shared/menu/menu';
import './dashboard.css';
import DashboardCard from './elements/card/dashboard-card';

class Dashboard extends Component {

    getData = () => {

    }

    render() {
        return (
            <div>
                <Menu />
                <div id="main">
                    <div class="row dashboard-card-row">
                        <DashboardCard title="Recently added shows" description="All added shows of the last 3 months" buttonText="Run" />
                        <DashboardCard title="Recently added patterns" description="All added patterns of the last 3 months" buttonText="Run" />
                    </div>

                    <div class="row dashboard-card-row">
                        <DashboardCard title="Recently executed shows" description="All executed shows of the last 3 months" buttonText="Run" />
                        <DashboardCard title="Recently executed patterns" description="All executed patterns of the last 3 months" buttonText="Run" />
                    </div>
                </div>
            </div>
        );
    }
}

export default Dashboard;