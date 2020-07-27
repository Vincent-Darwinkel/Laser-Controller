import React, { Component } from 'react';
import Menu from '../shared/menu/menu';
import './settings.css';
import { Button, Form, Dropdown } from 'react-bootstrap';
import 'react-bootstrap-range-slider/dist/react-bootstrap-range-slider.css';
import RangeSlider from 'react-bootstrap-range-slider';
import { GetComPorts, SaveSettings, GetSettings } from 'services/settings/settings';
import { toast } from 'react-toastify';

class Settings extends Component {

    constructor(props) {
        super(props);

        this.state = {
            settings: {
                maxPower: 5,
                redPower: 100,
                greenPower: 100,
                bluePower: 100,
                maxHeight: 2000,
                minHeight: -2000,
                maxLeft: -2000,
                maxRight: 2000,
                comDevices: null,
                comPort: null
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
    }

    resetColorBalance = () => {
        let settings = this.state.settings;

        settings.redPower = 100;
        settings.greenPower = 100;
        settings.bluePower = 100;

        this.setState({ settings: settings });
    }

    changeCOMPort = (port) => {
        let settings = this.state.settings;
        settings.comPort = port;

        this.setState({ settings: settings })
    }

    getCOMDevices = async () => {
        const ports = await GetComPorts();
        let items = [];

        items.map((item) => <Dropdown.Item key={ports[item]} onSelect={() => this.changeCOMPort(ports[item])}>{ports[item]}</Dropdown.Item>);

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

    saveSliderValues = (value, sliderName) => {
        let settings = this.state.settings;
        settings[sliderName] = value;

        this.setState({ settings: settings });
    }

    checkBoundaryValues() {
        let settings = this.state.settings;

        if (settings.maxHeight > 2000 || settings.maxHeight < -2000){
            toast('max height value is invalid value reset to 2000');
            settings.maxHeight = 2000;
        } 

        else if (settings.minHeight > 2000 || settings.minHeight < -2000) {
            toast('min height value is invalid value reset to -2000');
            settings.minHeight = -2000;
        } 

        if (Math.abs(settings.maxHeight - settings.minHeight) < 200) toast('difference between max height and min height is small are you sure?');

        if (settings.maxLeft > 2000 || settings.maxLeft < -2000) {
            toast('max left value is invalid value reset to -2000');
            settings.minHeight = -2000;
        }

        else if (settings.maxRight > 2000 || settings.maxRight < -2000) {
            toast('max right value is invalid value reset to 2000');
            settings.minHeight = 2000;
        }

        if (Math.abs(settings.maxLeft - settings.maxRight) < 200) toast('difference between max left and max right is small are you sure?');
    }

    changeBoundaryValues() {
        let settings = this.state.settings;
        settings.maxHeight = Number(document.getElementById('txtbox-maxHeight').value);
        settings.minHeight = Number(document.getElementById('txtbox-minHeight').value);

        settings.maxLeft = Number(document.getElementById('txtbox-maxLeft').value);
        settings.maxRight = Number(document.getElementById('txtbox-maxRight').value);

        this.checkBoundaryValues();
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
                                <Form.Label>Max power</Form.Label>
                                <Form.Text className="text-muted">
                                    <h6>{this.state.settings.maxPower}%</h6>
                                </Form.Text>

                                <RangeSlider
                                    value={this.state.settings.maxPower}
                                    onChange={changeEvent => this.saveSliderValues(Number(changeEvent.target.value), 'maxPower')}
                                />

                                <Button variant="secondary" onClick={() => this.saveSliderValues(5, 'maxPower')}>
                                    Reset
                                </Button>
                            </Form.Group>

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

                        <h5>Color balance</h5>

                        <div>
                            <Form.Group>
                                <Form.Label>Red</Form.Label>
                                <Form.Text className="text-muted">
                                    <h6>{this.state.settings.redPower}%</h6>
                                </Form.Text>

                                <RangeSlider
                                    variant="danger"
                                    value={this.state.settings.redPower}
                                    onChange={changeEvent => this.saveSliderValues(Number(changeEvent.target.value), 'redPower')}
                                />
                            </Form.Group>

                            <Form.Group>
                                <Form.Label>Green</Form.Label>
                                <Form.Text className="text-muted">
                                    <h6>{this.state.settings.greenPower}%</h6>
                                </Form.Text>

                                <RangeSlider
                                    variant="success"
                                    value={this.state.settings.greenPower}
                                    onChange={changeEvent => this.saveSliderValues(Number(changeEvent.target.value), 'greenPower')}
                                />
                            </Form.Group>

                            <Form.Group>
                                <Form.Label>Blue</Form.Label>
                                <Form.Text className="text-muted">
                                    <h6>{this.state.settings.bluePower}%</h6>
                                </Form.Text>

                                <RangeSlider
                                    value={this.state.settings.bluePower}
                                    onChange={changeEvent => this.saveSliderValues(Number(changeEvent.target.value), 'bluePower')}
                                />

                                <Button variant="secondary" onClick={this.resetColorBalance}>
                                    Reset
                                </Button>
                            </Form.Group>

                            <Form.Group className="laser-boundaries-textbox">
                                <Form.Label>Max height</Form.Label>
                                <Form.Control type="number" min={-2000} max={2000} defaultValue={this.state.settings.maxHeight} placeholder="-2000 / 2000" id="txtbox-maxHeight" onInput={() => this.changeBoundaryValues()} />
                            </Form.Group>

                            <Form.Group className="laser-boundaries-textbox">
                                <Form.Label>Min height</Form.Label>
                                <Form.Control type="number" min={-2000} max={2000} defaultValue={this.state.settings.minHeight} placeholder="-2000 / 2000" id="txtbox-minHeight" onInput={() => this.changeBoundaryValues()} />
                            </Form.Group>

                            <Form.Group className="laser-boundaries-textbox">
                                <Form.Label>Max left</Form.Label>
                                <Form.Control type="number" min={-2000} max={2000} defaultValue={this.state.settings.maxLeft} placeholder="-2000 / 2000" id="txtbox-maxLeft" onInput={() => this.changeBoundaryValues()} />
                            </Form.Group>

                            <Form.Group className="laser-boundaries-textbox">
                                <Form.Label>Max right</Form.Label>
                                <Form.Control type="number" min={-2000} max={2000} defaultValue={this.state.settings.maxRight} placeholder="-2000 / 2000" id="txtbox-maxRight" onInput={() => this.changeBoundaryValues()} />
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