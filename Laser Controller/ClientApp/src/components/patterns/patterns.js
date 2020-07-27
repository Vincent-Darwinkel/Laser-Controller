import React, { Component } from 'react';
import Menu from '../shared/menu/menu';
import { Button, Form } from 'react-bootstrap';
import './patterns.css';
import DotDrawer from './elements/dotDrawer';

class Patterns extends Component {

    constructor(props) {
        super(props);

        this.state = {
            dots: [],
            dotDrawers: []
        };
    }

    componentDidMount() {
        const canvas = document.getElementById('patterns-canvas');
        canvas.width = window.innerWidth - 200;
        canvas.heigh = window.innerHeight - 200;
    }

    addDotDrawer = () => {
        let dotDrawers = this.state.dotDrawers;

        const newDotDrawer = <DotDrawer id={dotDrawers.length}/>;
        dotDrawers.push(newDotDrawer);

        this.setState({ dotDrawers: dotDrawers });
    }

    drawLine = (index) => {
        const canvas = document.getElementById('patterns-canvas');
        var ctx = canvas.getContext("2d");

        const x = Math.round(document.getElementById('patterns-x').value / 5);
        const y = Math.round(document.getElementById('patterns-y').value / 5);
        const r = document.getElementById('patterns-red').value;
        const g = document.getElementById('patterns-green').value;
        const b = document.getElementById('patterns-blue').value;

        ctx.beginPath();

        if (r === 0 || g === 0 || b === 0 || this.state.dots === [])
            ctx.arc(x, y, 2, 0, 2 * Math.PI);

        else {
            ctx.moveTo(0, 0);
            ctx.lineTo(300, 150);
        }

        ctx.stroke();

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

                    <canvas id="patterns-canvas" height="500">
                        Your browser does not support the HTML5 canvas tag.
                    </canvas>

                    <Form.Group>
                        <Form.Label>Name</Form.Label>
                        <Form.Control type="text" required minLength={4} placeholder="My pattern" />
                    </Form.Group>

                    <Form onSubmit={this.submitForm}>
                        <div id="patterns-dotdrawers">
                            {this.state.dotDrawers}
                        </div>

                        <Button variant="primary" type="button" onClick={this.addDotDrawer}>
                            Add new dot
                        </Button>

                        <Button variant="primary" type="submit">
                            Save
                        </Button>
                    </Form>
                </div>
            </div>
        );
    }
}

export default Patterns;