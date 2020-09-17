import React, { Component } from 'react';
import Menu from '../shared/menu/menu';
import './game.css';

class Game extends Component {

    constructor(props) {
        super(props);

        this.state = {
            games: null
        };
    }

    componentDidMount() {

    }

    submitForm = (e) => {
        e.preventDefault();

        this.drawLine(e);
        console.log(e);
    }

    render() {
        return (
            <div>
                <Menu />
                <div id="main">
                </div>
            </div>
        );
    }
}

export default Game;