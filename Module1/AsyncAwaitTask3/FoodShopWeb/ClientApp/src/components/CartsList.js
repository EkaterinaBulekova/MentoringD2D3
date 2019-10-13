import React, { Component } from 'react';
import { Button } from 'reactstrap';
import Order from './Order';

export class CartsList extends Component {
    static displayName = CartsList.name;

  constructor(props) {
      super(props);
      this.state = {
        carts: [],
        cartCount: 0,
        cartPrice: 0,
        loading: true
      };
  }

  componentDidMount() {
    this.populateCartsData(this.props.cartId);
    this.initializeCart(this.props.cartId);
  }

  renderCartsTable(state) {
    var {carts, cartCount, cartPrice} = state;
    return (
      <table className='table table-striped' aria-labelledby="tabelLabel">
        <thead>
          <tr>
            <th>Name</th>
            <th>Count</th>
            <th>Price</th>
            <th>Action</th>
          </tr>
        </thead>
        <tbody>
          {carts.map(cart =>
            <tr key={cart.id}>
              <td>{cart.product.name}</td>
              <td>{cart.count}</td>
              <td>{cart.product.price*cart.count}</td>
              <td><Button color="danger" onClick={() => {this.removeFromCart(cart.id, this.props.cartId, carts)}}>Remove</Button></td>
            </tr>
          )}
            <tr key={0}>
              <td>Total</td>
              <td>{cartCount}</td>
              <td>{cartPrice}</td>
              <td><Order buttonLabel="Order" className="order" cartId={this.props.cartId}/></td>
            </tr>
        </tbody>
      </table>
    );
  }

  render() {
    let {loading} = this.state;
    let contents = loading
      ? <p><em>Loading...</em></p>
      : this.state.carts.length > 0
        ? this.renderCartsTable(this.state)
        : <p><em>Not found any products</em></p>;

    return (
      <div>

        <h1 id="TabelLabel" >Cart Details</h1>
        {contents}
      </div>
    );
  }

  async populateCartsData(cartId) {
    const response = await fetch('cart/cartid='+cartId);
    const data = await response.json();
    this.setState({carts: data, loading: false});
  }

  async initializeCart(cartId){
    const response = await fetch('cart/'+cartId);
    const data = await response.json();
    this.setState({ cartCount: data.totalCount, cartPrice: data.totalPrice, loading: false });
  }

  removeFromCart = async (id, cartId, carts) => {
    const rawResponse = await fetch('cart', {
      method: 'DELETE',
      headers: {
        'Accept': 'application/json',
        'Content-Type': 'application/json'
      },
      body: JSON.stringify({
        CartId: cartId,
        Id: id
      })
    });
    const data = await rawResponse.json();
    this.setState({
      cartCount: data.totalCount,
      cartPrice: data.totalPrice,
      carts: carts.filter(c => c.id !== id)});
  }
}
