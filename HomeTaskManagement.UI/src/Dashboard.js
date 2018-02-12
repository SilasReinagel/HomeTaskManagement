import React, { Component } from 'react';

import Session from './Session';

class Dashboard extends Component {
  constructor(props) {
    super(props);
    this.state = {
      open:false
    }
  }

  render() {
    return (
      <div>
        <br/>
        <h3>Hello {new Session().username()}</h3>
      </div>
    );
  }
}

export default Dashboard;
