import React, { Component } from 'react';

class DashboardCard extends Component {

    render() {
        return (
            <a className="col-sm-6 card-href" href={this.props.href}>
                <div className="card">
                    <div className="card-body">
                        <h5 className="card-title">{this.props.title}</h5>
                        <p className="card-text">{this.props.description}</p>
                        {this.props.content}
                    </div>
                </div>
            </a>
        );
    }
}

export default DashboardCard;