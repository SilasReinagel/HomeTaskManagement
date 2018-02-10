import React, { Component } from 'react';
import injectTapEventPlugin from 'react-tap-event-plugin';
import MuiThemeProvider from 'material-ui/styles/MuiThemeProvider';
import AppBar from 'material-ui/AppBar';
import Drawer from 'material-ui/Drawer';
import MenuItem from 'material-ui/MenuItem';
import Divider from 'material-ui/Divider';

import FlatButton from 'material-ui/FlatButton';

import './App.css';
import Login from './Login';
import Dashboard from './Dashboard';
import Session from "./Session";
import CreateTask from "./CreateTask";
injectTapEventPlugin();

class App extends Component {  
  constructor(props) {
    super(props);
    this.state={
      menuOpen:false,
      session:new Session(),
      isLoggedIn:false,
      view:[]
    };
  }
  
  openMenu = (_) => this.setState({menuOpen:!this.state.menuOpen}); 

  navigateTo = (viewName) => {
    console.log("Navigate to: " + viewName);
    var view=[];
    if(viewName === "Login") {
      view.push(<Login appContext={this}/>);
    } else if(viewName === "Dashboard") {
      view.push(<Dashboard appContext={this}/>);
    } else if(viewName === "Create Task") {
      view.push(<CreateTask appContext={this}/>);
    } else {
      console.log("Unknown view: " + viewName);
    }
    this.setState({view:view, menuOpen:false});
  }

  componentWillMount() {
    this.update();
    if (!this.state.isLoggedIn) {
      this.navigateTo("Login");
    } else {
      this.navigateTo("Dashboard");
    }
  }

  render() {
    return (
      <div className="App">      
        <MuiThemeProvider>
          <div>
            <AppBar title="Home Task Management" 
              showMenuIconButton={this.state.isLoggedIn}
              onLeftIconButtonClick={this.openMenu}
              iconElementRight={this.state.isLoggedIn && <FlatButton label="Logout" onClick={(event) => this.logout()}/>}/>
            <Drawer docked={false} open={this.state.menuOpen} onRequestChange={(open) => this.setState({menuOpen:open})}>
              <MenuItem onClick={() => this.navigateTo("Create Task")}>Create Task</MenuItem>
              <Divider />
            </Drawer>
            {this.state.view}
          </div>
        </MuiThemeProvider>
      </div>
    );
  }

  update() {
    this.state.isLoggedIn=this.state.session.isLoggedIn();
  }

  logout() {
    this.state.session.logout();
    this.update();
    var loginView=[];
    loginView.push(<Login appContext={this}/>);
    this.setState({ view:loginView })
  }
}

export default App;
