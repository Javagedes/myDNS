import React, {Component} from 'react';
import logo from './logo.svg';
import './App.css';
import axios from 'axios';
import MainView from './components/mainView';

class App extends Component {

  state = {
    entries: []
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
