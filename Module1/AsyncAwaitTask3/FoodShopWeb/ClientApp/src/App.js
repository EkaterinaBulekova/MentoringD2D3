import React, { Component } from 'react';
import { Route } from 'react-router';
import { Layout } from './components/Layout';
import { Home } from './components/Home';

import './custom.css'
import { ProductsPage } from './components/ProductsPage';

export default class App extends Component {
  static displayName = App.name;

  render () {
    return (
      <Layout>
        <Route exact path='/' component={ProductsPage} />
        <Route exact path='/products' component={ProductsPage} />
        <Route path='/products/:category' component={ProductsPage} />
      </Layout>
    );
  }
}
