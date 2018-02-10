import React, { Component } from 'react';
import axios from 'axios';
import RaisedButton from 'material-ui/RaisedButton';
import TextField from 'material-ui/TextField';

import Session from './Session';
import Dashboard from './Dashboard';
import ApiUrls from './ApiUrls';

class Login extends Component {
  constructor(props) {
    super(props);
    this.state={
      session:new Session(),
      isBusy:false,
      username:'',
      password:''    
    }
  }

  render() {
    return (
      <div>
          <div>
            <TextField hintText="Username" 
              type="username" 
              floatingLabelText="Username" 
              onChange={(event, newValue) => this.setState({username:newValue})}/>
            <br/>
            <TextField hintText="Password" 
              type="password" 
              floatingLabelText="Password" 
              onChange={(event, newValue) => this.setState({password:newValue})}
              onKeyPress={(event) => this.handleButtonPress(event)} />
            <br/>
            {!this.isBusy && (<RaisedButton label="Submit" primary={true} style={style} onClick={(event) => this.login()}/>)}
          </div>
      </div>
    );
  }

  handleButtonPress(event) {
    if (event.key === 'Enter') {
      this.login();
      event.preventDefault();
    }
  }

  login() {
    var self = this;    
    self.setState({isBusy:true});
    var payload = {
      "appname": "HomeTaskManagement",
      "username": this.state.username,
      "password": this.state.password
    };

    axios
      .post(ApiUrls.login(), payload)
      .then(function(response) {
        console.log(response);
        if(response.status === 200) {
          console.log("Login successful");
          self.state.session.setLoginInfo(self.state.username, response.data.Token, response.data.ExpiresAtUtc);
          
          var dashboardView=[];
          dashboardView.push(<Dashboard appContext={self.props.appContext}/>);
          self.props.appContext.setState({view:dashboardView, isLoggedIn:true});
        } else {
          console.log("Login unsuccessful");
        }
      })
      .catch(function(error) {
        console.log(error);
      });
      self.setState({isBusy:false});
  }
}

const style = {
  margin: 30,
};

export default Login;
