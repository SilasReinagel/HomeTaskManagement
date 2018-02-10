import React, { Component } from 'react';
import axios from 'axios';
import RaisedButton from 'material-ui/RaisedButton';
import TextField from 'material-ui/TextField';
import SelectField from 'material-ui/SelectField';
import MenuItem from 'material-ui/MenuItem';

import ApiUrls from "./ApiUrls";
import Session from "./Session";

class CreateTask extends Component {
  constructor(props) {
    super(props);
    this.state = {
      id:"",
      name:"",
      unitsOfWork:0,
      importance:"Normal",
      frequency:"Daily"
    }
  }

  updateFrequency = (event, index, value) => this.setState({frequency:value});
  updateImportance = (event, index, value) => this.setState({importance:value});

  render() {
    return (
      <div>
        <div>
          <TextField floatingLabelText="TaskId" type="text" onChange={(event, newValue) => this.setState({id:newValue})}/>
          <br/>
          <TextField floatingLabelText="Name" type="text" onChange={(event, newValue) => this.setState({name:newValue})}/>
          <br/>
          <TextField floatingLabelText="Units Of Work" type="numeric" onChange={(event, newValue) => this.setState({unitsOfWork:parseInt(newValue)})}/>
          <br/>            
          <SelectField floatingLabelText="Frequency" style={{textAlign:'left'}} value={this.state.frequency} onChange={this.updateFrequency} floatingLabelFixed={true}>
            <MenuItem value={"Daily"} primaryText="Daily" />
            <MenuItem value={"Weekly"} primaryText="Weekly" />
          </SelectField>
          <br/>
          <SelectField floatingLabelText="Importance" style={{textAlign:'left'}} value={this.state.importance} onChange={this.updateImportance}>
            <MenuItem value={"Normal"} primaryText="Normal" />
            <MenuItem value={"Critical"} primaryText="Critical" />
          </SelectField>
          <br/>
          <RaisedButton label="Create" primary={true} style={style} onClick={(event) => this.create()}/>
        </div>
      </div>
    );
  }

  create() {
    console.log(this.state);
    var self = this;    
    var payload = self.state;

    axios
      .post(ApiUrls.createTask(), payload, { 
        headers: {
          "Authorization" : "Bearer " + new Session().token(), 
          "Content-Type": "application/json"
        } 
      })
      .then(function(response) {
        console.log(response);
      })
      .catch(function(error) {
        console.log(error);
      });
  }
}

const style = {
  margin: 30
}

export default CreateTask;