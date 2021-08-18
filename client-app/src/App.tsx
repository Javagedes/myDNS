import React, {Component} from 'react';
import logo from './logo.svg';
import './App.css';
import axios from 'axios';

class App extends Component {

  state = {
    entries: []
  }

  componentDidMount() {
    console.log("We called this function")
    axios.get('https://localhost:5001/api/DNSEntry').then((response) => {
      this.setState({
        entries: response.data
      })
      console.log(response.data)
    })
  }

  render() {
    return (
      <div className="App">
        <header className="App-header">
          <h3>List of DNS Entries</h3>
          <ul>
            {this.state.entries.map((entry:any) => (
              <li key="{entry.id}" >{ entry.hostName } - {entry.type} - {entry.value} </li>
            ))}
          </ul>
        </header>
      </div>
    );
  }
  
}

export default App;
