import React, { Component } from 'react';
import { Button, Form, Dropdown } from 'react-bootstrap';
import { StartAudio, StopAudio, GetAudioDevices } from 'services/audio/audio';

class Audio extends Component {

    constructor(props) {
        super(props);

        this.state = {
            devices: null,
            selectedAudioDevice: undefined
        }
    }

    async componentDidMount() {
        await this.getAudioDevices();
    }

    getAudioDevices = async () => {
        const devices = await GetAudioDevices();
        let items = [];

        for (let index = 0; index < devices.length; index++) {
            const device = devices[index];
            items.push(<Dropdown.Item key={device} onSelect={() => this.setState({ selectedAudioDevice: device })}>{device}</Dropdown.Item>);
        }

        this.setState({ devices: items });
    }

    startAudio = async () => {
        await StartAudio(this.state.selectedAudioDevice);
    }

    render() {
        return (
            <div className="col-sm-6">

                <Form.Group>
                    <Form.Label>Audio devices</Form.Label>
                    <Dropdown>
                        <Dropdown.Toggle variant="primary" id="dropdown-basic">
                            { this.state.selectedAudioDevice ?? "Not set" }
                        </Dropdown.Toggle>

                        <Dropdown.Menu>
                            {this.state.devices}
                        </Dropdown.Menu>
                    </Dropdown>
                </Form.Group>

                <Button onClick={(e) => this.startAudio()}>
                    Start
                </Button>
            </div>
        );
    }
}

export default Audio;