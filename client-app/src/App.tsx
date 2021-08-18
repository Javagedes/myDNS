import React, {Component} from 'react';
import logo from './logo.svg';
import './App.css';
import axios from 'axios';
import MainView from './components/mainView';

class App extends Component {

  state = {
    entries: []
  }

  //TODO: Change pulling the information from db to only happen once. It currently happens every time someone loads the website
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
          
          <MainView/>

        </header>
      </div>
    );
  }
  
}

export default App;
