import React, { Component } from 'react';
import {List, ListItem} from 'material-ui/List';
import {GridList, GridTile} from 'material-ui/GridList';
import Divider from 'material-ui/Divider';
import CircularProgress from 'material-ui/CircularProgress';
import axios from 'axios';

import ApiUrls from './ApiUrls';
import Session from './Session';

class Accounts extends Component {
    constructor(props) {
        super(props);
        this.state = {
            isLoading:true,
            accounts:[]
        }
    }

    componentDidMount() {
        this.loadAccounts();
    }

    render() {
        return (
            <div>
                <h2>Accounts</h2>                   
                <Divider />
                {this.state.isLoading 
                    ? <div><br/><CircularProgress size={80}/></div> 
                    : <div style={styles.root}>     
                        {this.state.accounts.map(function(item) {
                            return (
                                <div style={styles.item}>
                                    <h3 style={{color:'white'}}>{item.balance}</h3>
                                    <p style={{color:'white'}}>{item.name}</p>
                                </div>
                            );
                        })}
                      </div>}
            </div>
        );
    }

    loadAccounts() {
        var self = this;
        axios
            .get(ApiUrls.viewAllAccounts(), { 
                headers: {"Authorization" : "Bearer " + new Session().token()} 
            })
            .then(function(response) {
                self.setState({isLoading:false, accounts:response.data.content})
                console.log(response);
            })
            .catch(function(error) {
                self.setState({isLoading:false})
                console.log(error);
            });
    }
}

const styles = {
    root: {
      display: 'flex',
      flexWrap: 'wrap',
      justifyContent: 'center'
    },
    gridList: {
      width: '80%',
      height: 'auto',
      overflowY: 'auto',
    },
    item: {
      width: 150,
      textAlign:'center',
      backgroundColor: '#4caf50',
      padding: 10,
      margin: 10,
    }
  };

export default Accounts;