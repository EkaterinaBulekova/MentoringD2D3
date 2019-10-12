import React, { Component } from 'react';
import { Route } from 'react-router';
import { Layout } from './components/Layout';
import { ProductsPage } from './components/ProductsPage';
import { CartsPage } from './components/CartsPage';
import './custom.css';


export default class App extends Component {
  static displayName = App.name;

  render () {
    return (
      <Layout>
        <Route exact path='/' component={ProductsPage} />
        <Route path='/carts/:cartId' component={CartsPage} />
        <Route exact path='/products' component={ProductsPage} />
        <Route path='/products/:category' component={ProductsPage} />
      </Layout>
    );
  }
}
