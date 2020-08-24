import React, { Component } from 'react';
import Menu from '../shared/menu/menu';
import { Button, Form, Dropdown } from 'react-bootstrap';
import { StartAudio, StopAudio } from 'services/audio/audio';
import './audio.css';

class Audio extends Component {

    constructor(props) {
        super(props);
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

    render() {
        return (
            <div>
                <Menu />
                <div id="main">
                    <div id="audio-control" className="col-sm-6 card">
                        <h6>Start or stop a current session</h6>

                        <Button id="audio-btn-start" onClick={(e) => this.startAudio()}>Start <i className="fas fa-play"></i></Button>
                        <Button id="audio-btn-stop" variant="danger" onClick={(e) => this.stopAudio()}>Stop <i class="far fa-stop-circle"></i></Button>
                    </div>
                </div>
            </div>
        );
    }
}

export default Audio;