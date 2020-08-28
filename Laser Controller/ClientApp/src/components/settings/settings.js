import React, { Component } from 'react';
import Menu from '../shared/menu/menu';
import './settings.css';
import { Button, Form, Dropdown } from 'react-bootstrap';
import 'react-bootstrap-range-slider/dist/react-bootstrap-range-slider.css';
import { GetComPorts, SaveSettings, GetSettings } from 'services/settings/settings';
import { toast } from 'react-toastify';

class Settings extends Component {

    constructor(props) {
        super(props);

        this.state = {
            settings: {
                maxHeight: null,
                minHeight: null,
                maxLeft: null,
                maxRight: null,
                comDevices: null,
                comPort: null,
                maxLaserPower: []
            }
        }
    }

    async componentDidMount() {
        this.getCOMDevices();
        this.getSettings();
    }

    getSettings = async () => {
        const data = await GetSettings();
        this.setState({ settings: data });
        if (data.maxHeight === 0 && data.minHeight === 0 && data.maxLeft === 0 && data.maxRight === 0) toast.error('Failed to get settings from laser, make sure the laser is turned on and the right Com port is selected and saved, after that reload the page');
        this.changeCOMPort(this.state.settings.comPort);
    }

    changeCOMPort = (port) => {
        let settings = this.state.settings;
        settings.comPort = port;

        this.setState({ settings: settings })
    }

    getCOMDevices = async () => {
        const ports = await GetComPorts() ?? [];
        let items = [];

        items = ports.map((item) => <Dropdown.Item key={item} onSelect={() => this.changeCOMPort(item)}>{item}</Dropdown.Item>);

        let settings = this.state.settings;
        settings.comDevices = items;

        this.setState({ settings: settings });
    }

    submitForm = async (e) => {
        e.preventDefault();

        const data = this.state.settings;
        const result = await SaveSettings(data);

        if (!result.success) toast.error(result.Message);
        if (result.success) toast.success("Settings saved");
    }

    checkBoundaryValues = () => {
        let settings = this.state.settings;

        if (settings.maxHeight > 2000 || settings.maxHeight < -2000) {
            toast('max height value is invalid value reset to 2000');
            settings.maxHeight = 2000;
        }

        else if (settings.minHeight > 2000 || settings.minHeight < -2000) {
            toast('min height value is invalid value reset to -2000');
            settings.minHeight = -2000;
        }

        if (Math.abs(settings.maxHeight - settings.minHeight) < 20) toast('difference between max height and min height is small are you sure?');

        if (settings.maxLeft > 2000 || settings.maxLeft < -2000) {
            toast('max left value is invalid value reset to -2000');
            settings.minHeight = -2000;
        }

        else if (settings.maxRight > 2000 || settings.maxRight < -2000) {
            toast('max right value is invalid value reset to 2000');
            settings.minHeight = 2000;
        }

        if (Math.abs(settings.maxLeft - settings.maxRight) < 20) toast('difference between max left and max right is small are you sure?');
    }

    changeBoundaryValues = () => {
        let settings = this.state.settings;
        settings.maxHeight = Number(document.getElementById('txtbox-maxHeight').value);
        settings.minHeight = Number(document.getElementById('txtbox-minHeight').value);

        settings.maxLeft = Number(document.getElementById('txtbox-maxLeft').value);
        settings.maxRight = Number(document.getElementById('txtbox-maxRight').value);

        this.checkBoundaryValues();
        this.setState({ settings: settings });
    }

    changeLaserPower = (arrayIndex, txtbox) => {
        let settings = this.state.settings;
        settings.maxLaserPower[arrayIndex] = Number(document.getElementById(txtbox).value);

        this.setState({ settings: settings });
    }

    render() {
        return (
            <div>
                <Menu />
                <div id="main">

                    <Form onSubmit={this.submitForm}>
                        <div>
                            <h4>Laser settings</h4>

                            <Form.Group>
                                <Form.Label>COM port</Form.Label>
                                <Dropdown>
                                    <Dropdown.Toggle variant="primary" id="dropdown-basic">
                                        {this.state.settings.comPort ?? "Not set"}
                                    </Dropdown.Toggle>

                                    <Dropdown.Menu>
                                        {this.state.settings.comDevices}
                                    </Dropdown.Menu>
                                </Dropdown>
                            </Form.Group>
                        </div>

                        <h5>Max power per color</h5>

                        <div>
                            <Form.Group className="laser-boundaries-textbox">
                                <Form.Label>Red</Form.Label>
                                <Form.Control type="number" min={0} max={255} id="txtbox-red" onInput={(e) => this.changeLaserPower(0, 'txtbox-red')} defaultValue={this.state.settings.maxLaserPower[0]} placeholder="0 / 255" />
                            </Form.Group>

                            <Form.Group className="laser-boundaries-textbox">
                                <Form.Label>Green</Form.Label>
                                <Form.Control type="number" min={0} max={255} id="txtbox-green" onInput={(e) => this.changeLaserPower(1, 'txtbox-green')} defaultValue={this.state.settings.maxLaserPower[1]} placeholder="0 / 255" />
                            </Form.Group>

                            <Form.Group className="laser-boundaries-textbox">
                                <Form.Label>Blue</Form.Label>
                                <Form.Control type="number" min={0} max={255} id="txtbox-blue" onInput={(e) => this.changeLaserPower(2, 'txtbox-blue')} defaultValue={this.state.settings.maxLaserPower[2]} placeholder="0 / 255" />
                            </Form.Group>

                            <Form.Group className="laser-boundaries-textbox">
                                <Form.Label>Max left</Form.Label>
                                <Form.Control type="number" min={-2000} max={2000} defaultValue={this.state.settings.maxLeft} placeholder="-2000 / 2000" id="txtbox-maxLeft" onInput={() => this.changeBoundaryValues()} />
                            </Form.Group>

                            <Form.Group className="laser-boundaries-textbox">
                                <Form.Label>Max right</Form.Label>
                                <Form.Control type="number" min={-2000} max={2000} defaultValue={this.state.settings.maxRight} placeholder="-2000 / 2000" id="txtbox-maxRight" onInput={() => this.changeBoundaryValues()} />
                            </Form.Group>

                            <Form.Group className="laser-boundaries-textbox">
                                <Form.Label>Min height</Form.Label>
                                <Form.Control type="number" min={-2000} max={2000} defaultValue={this.state.settings.minHeight} placeholder="-2000 / 2000" id="txtbox-minHeight" onInput={() => this.changeBoundaryValues()} />
                            </Form.Group>

                            <Form.Group className="laser-boundaries-textbox">
                                <Form.Label>Max height</Form.Label>
                                <Form.Control type="number" min={-2000} max={2000} defaultValue={this.state.settings.maxHeight} placeholder="-2000 / 2000" id="txtbox-maxHeight" onInput={() => this.changeBoundaryValues()} />
                            </Form.Group>
                        </div>

                        <Button variant="primary" type="submit">Save</Button>
                    </Form>
                </div>
            </div>
        );
    }
}

export default Settings;