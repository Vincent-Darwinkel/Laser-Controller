import React, { Component } from 'react';
import './menu.css';

class Menu extends Component {

    render() {
        return (
            <div id="menu">
                <label>Status: offline</label><br />
                <div id="menu-links">
                    <a href="/"><i className="fas fa-columns"></i> Dashboard </a>
                    <a href="/shows"><i className="fas fa-layer-group"></i> Shows</a>
                    <a href="/patterns"><i className="fas fa-palette"></i> Patterns</a>
                    <a href="/settings"><i className="fas fa-cog"></i> Settings</a>
                </div>
            </div>
        );
    }
}

export default Menu;