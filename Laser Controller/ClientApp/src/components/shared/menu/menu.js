import React, { Component } from 'react';
import './menu.css';
import { StopAudio } from 'services/audio/audio';

class Menu extends Component {

    emergencyStop = () => {
        StopAudio();        
    }

    render() {
        return (
            <div id="menu">
                <div id="menu-links">
                    <a href="/"><i className="fas fa-columns"></i> Dashboard </a>
                    <a href="/shows"><i className="fas fa-layer-group"></i> Shows</a>
                    <a href="/patterns"><i className="fas fa-palette"></i> Patterns</a>
                    <a href="/settings"><i className="fas fa-cog"></i> Settings</a>
                    <button id="menu-btn-emergency" onClick={(e) => this.emergencyStop()}><i className="far fa-hand-paper"></i> Emergency stop</button>
                </div>
            </div>
        );
    }
}

export default Menu;