import React, { Component } from 'react';
import Menu from '../shared/menu/menu';
import { AllPatterns, StartPattern } from 'services/patterns/patterns';
import { Button, Form, Dropdown } from 'react-bootstrap';
import './patterns.css';
import { toast } from 'react-toastify';

class Patterns extends Component {

    constructor(props) {
        super(props);

        this.state = {
            patterns: null,
            selectedPattern: null
        };
    }

    async componentDidMount() {
        const patterns = await AllPatterns();
        this.generateDropDownItems(patterns);
    }

    changePattern = (pattern) => {
        this.setState({ selectedPattern: pattern })
    }

    validateForm = () => {
        if (this.state.selectedPattern === null) {
            return false;
        }

        return true;
    }

    submitForm = async (e) => {
        e.preventDefault();

        if (!this.validateForm()) {
            toast.error('No pattern selected');
            return;
        }

        const patternOptions = {
            patternName: this.state.selectedPattern,
            animationSpeed: Number(document.getElementById('txtbox-animation-speed').value),
            durationMilliseconds: Number(document.getElementById('txtbox-duration').value),
            total: Number(document.getElementById('txtbox-total').value)
        }

        await StartPattern(patternOptions);
    }

    generateDropDownItems = (patterns) => {
        let patternItems = [];

        for (let index = 0; index < patterns.length; index++) {
            const element = patterns[index];
            patternItems.push(<Dropdown.Item eventKey={element}>{element}</Dropdown.Item>);
        }

        this.setState({ patterns: patternItems });
    }

    render() {
        return (
            <div>
                <Menu />
                <div id="main">
                    <Form onSubmit={this.submitForm}>
                        <div>
                            <h4>Patterns</h4>

                            <Form.Group>
                                <Dropdown onSelect={(e) => this.changePattern(e)}>
                                    <Dropdown.Toggle variant="primary">
                                        {this.state.selectedPattern ?? "No patterns selected"}
                                    </Dropdown.Toggle>

                                    <Dropdown.Menu>
                                        {this.state.patterns}
                                    </Dropdown.Menu>
                                </Dropdown>
                            </Form.Group>

                            <Form.Group>
                                <Form.Label>AnimationSpeed</Form.Label>
                                <Form.Control id="txtbox-animation-speed" min={1} max={30} type="number" defaultValue="7" />
                            </Form.Group>

                            <Form.Group>
                                <Form.Label>Duration milliseconds</Form.Label>
                                <Form.Control id="txtbox-duration" type="number" defaultValue="2000" />
                            </Form.Group>

                            <Form.Group>
                                <Form.Label>Total</Form.Label>
                                <Form.Control id="txtbox-total" type="number" defaultValue="7" />
                            </Form.Group>
                        </div>

                        <Button variant="primary" type="submit">Start</Button>
                    </Form>
                </div>
            </div>
        );
    }
}

export default Patterns;