import React, { Component } from 'react';



import Form from './components/FormComponent/Form'
import './custom.css';

export default class App extends Component {
  static displayName = App.name;

  render() {
    return (
        <div className="App">
            <Form />
        </div>
    );
  }
}
