import React, { Component } from 'react';
import Menu from '../shared/menu/menu';
import './game-dashboard.css';
import { Button, Form, Dropdown } from 'react-bootstrap';
import { AllGames, StartGame, MovePlayer } from 'services/games/games';

class GameDashboard extends Component {

    constructor(props) {
        super(props);

        this.state = {
            games: ['No game selected'],
            selectedGame: null,
            gameStarted: false
        };
    }

    async componentDidMount() {
        const games = await AllGames();
        this.generateDropDownItems(games);

        document.addEventListener("keydown", this.logKey, false);
    }

    submitForm = async (e) => {
        e.preventDefault();
        await StartGame(this.state.selectedGame);
    }

    changeGame = (game) => {
        this.setState({ selectedGame: game })
    }

    generateDropDownItems = (games) => {
        let gameItems = [];

        for (let index = 0; index < games.length; index++) {
            const element = games[index];
            gameItems.push(<Dropdown.Item eventKey={element}>{element}</Dropdown.Item>);
        }

        this.setState({ games: gameItems });
    }

    logKey = async (e) => {
        await MovePlayer(e.code);
    }

    render() {
        return (
            <div>
                <Menu />
                <div id="main">
                    <Form onSubmit={this.submitForm} onKeyPress={this.handleKeyPress}>
                        <div>
                            <h4>Games</h4>

                            <Form.Group>
                                <Dropdown onSelect={(e) => this.changeGame(e)}>
                                    <Dropdown.Toggle variant="primary" id="games-dropdown-basic">
                                        {this.state.selectedGame ?? "No game selected"}
                                    </Dropdown.Toggle>

                                    <Dropdown.Menu>
                                        {this.state.games}
                                    </Dropdown.Menu>
                                </Dropdown>
                            </Form.Group>
                        </div>

                        <Button variant="primary" type="submit">Start</Button>
                    </Form>
                </div>
            </div>
        );
    }
}

export default GameDashboard;