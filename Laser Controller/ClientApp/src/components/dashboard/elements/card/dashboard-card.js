import React, { Component } from 'react';

class DashboardCard extends Component {

    render() {
        return (
            <div class="col-sm-6">
                <div class="card">
                    <div class="card-body">
                        <h5 class="card-title">{this.props.title}</h5>
                        <p class="card-text">{this.props.description}</p>
                        {this.props.content}
                        
                        <a href="#" class="btn btn-primary">{this.props.buttonText}</a>
                    </div>
                </div>
            </div>
        );
    }
}

export default DashboardCard;