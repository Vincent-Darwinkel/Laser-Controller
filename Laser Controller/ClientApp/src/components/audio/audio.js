import React, { Component } from 'react';
import Menu from '../shared/menu/menu';
import { Button, Form, Dropdown } from 'react-bootstrap';
import { StartAudio, StopAudio, CalibrateAudio, GetCalibrationValue, CalibrationValue } from 'services/audio/audio';
import './audio.css';
import { toast } from 'react-toastify';

class Audio extends Component {

    constructor(props) {
        super(props);
        this.state = {
            calibrationValue: 0
        }
    }

    async componentDidMount() {
        const value = await GetCalibrationValue();

        this.setState({
            calibrationValue: value
        });
    }

    startAudio = () => {
        StartAudio();
        document.getElementById('audio-btn-stop').disabled = false;
        document.getElementById('audio-btn-start').disabled = true;
    }

    stopAudio = () => {
        StopAudio();
        document.getElementById('audio-btn-stop').disabled = true;
        document.getElementById('audio-btn-start').disabled = false;
    }

    calibrateAudio = async () => {
        const value = Number(document.getElementById('txtbox-calibration').value);
        if (value === NaN) {
            toast.error('Calibration value is not a number');
            return;
        }

        CalibrateAudio(value);
    }

    render() {
        return (
            <div>
                <Menu />
                <div id="main">
                    <h5 className="page-name">Audio</h5>
                    <div id="audio-control" className="col-sm-6 card">
                        <h6>Start or stop a current session</h6>

                        <Button id="audio-btn-start" onClick={(e) => this.startAudio()}>Start <i className="fas fa-play"></i></Button>
                        <Button id="audio-btn-stop" variant="danger" onClick={(e) => this.stopAudio()}>Stop <i class="far fa-stop-circle"></i></Button>

                        <Form.Group id="audio-calibration">
                            <Form.Label>Audio calibration</Form.Label>
                            <Form.Control onChange={this.calibrateAudio} id="txtbox-calibration" type="number" key={`${Math.floor((Math.random() * 1000))}-min`} defaultValue={this.state.calibrationValue} step={0.001} />
                            <Form.Text className="text-muted">
                                Used to make the algorithm more sensitive for bass tones
                            </Form.Text>
                        </Form.Group>
                    </div>
                </div>
            </div>
        );
    }
}

export default Audio;