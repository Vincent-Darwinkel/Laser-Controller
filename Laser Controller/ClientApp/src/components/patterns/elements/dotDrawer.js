import React, { Component } from 'react';
import { Button, Form } from 'react-bootstrap';

class DotDrawer extends Component {

    render() {
        return (
            <div className="row patterns-form-row">

                <Form.Group className="patterns-form-group patterns-form-group-size-small">
                    <Form.Label>ID</Form.Label>
                    <Form.Control value={this.props.id} disabled />
                </Form.Group>

                <Form.Group className="patterns-form-group patterns-form-group-size-large">
                    <Form.Label>X Pos</Form.Label>
                    <Form.Control id="patterns-x" type="number" required min={-2000} max={2000} placeholder="-2000 / 2000" />
                </Form.Group>

                <Form.Group className="patterns-form-group patterns-form-group-size-large">
                    <Form.Label>Y Pos</Form.Label>
                    <Form.Control id="patterns-y" type="number" required min={-2000} max={2000} placeholder="-2000 / 2000" />
                </Form.Group>

                <Form.Group className="patterns-form-group">
                    <Form.Label>Red Laser</Form.Label>
                    <Form.Control id="patterns-red" type="number" required min={0} max={100} defaultValue="5" placeholder="5" />
                </Form.Group>

                <Form.Group className="patterns-form-group">
                    <Form.Label>Green Laser</Form.Label>
                    <Form.Control id="patterns-green" type="number" required min={0} max={100} defaultValue="5" placeholder="5" />
                </Form.Group>

                <Form.Group className="patterns-form-group">
                    <Form.Label>Blue Laser</Form.Label>
                    <Form.Control id="patterns-blue" type="number" required min={0} max={100} defaultValue="5" placeholder="5" />
                </Form.Group>
            </div>
        );
    }
}

export default DotDrawer;