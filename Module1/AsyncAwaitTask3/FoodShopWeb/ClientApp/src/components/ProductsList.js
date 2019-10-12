import React, { Component } from 'react';
import { Button } from 'reactstrap';
import { Cart } from './Cart';

export class ProductsList extends Component {
    static displayName = ProductsList.name;

  constructor(props) {
      super(props);
      this.state = {
        products: [],
        category: this.props.category,
        cartCount: 0,
        cartPrice: 0,
        loading: true
      };
  }

  componentDidMount() {
    this.populateProductsData(this.props.category);
    this.initializeCart(this.props.cartId);
  }

  shouldComponentUpdate(nextProps) {
    if(this.props.category !== nextProps.category){
      this.populateProductsData(nextProps.category);
      return false;
    }
    return true;
  }

  renderProductsTable(state) {
    var {products} = state;
    return (
      <table className='table table-striped' aria-labelledby="tabelLabel">
        <thead>
          <tr>
            <th>Name</th>
            <th>Category</th>
            <th>Price</th>
            <th>Action</th>
          </tr>
        </thead>
        <tbody>
          {products.map(product =>
            <tr key={product.id}>
              <td>{product.name}</td>
              <td>{product.categoryId}</td>
              <td>{product.price}</td>
              <td><Button color="primary" 
              onClick={() => {this.addToCart(product.id, this.props.cartId)}}>Add to cart</Button></td>
            </tr>
          )}
        </tbody>
      </table>
    );
  }

  render() {
    let {loading, cartCount, cartPrice} = this.state;
    let contents = loading
      ? <p><em>Loading...</em></p>
      : this.state.products.length > 0
        ? this.renderProductsTable(this.state)
        : <p><em>Not found any products</em></p>;

    return (
      <div>
        <Cart cartCount={cartCount} cartPrice={cartPrice} cartId={this.props.cartId}></Cart>
        <h1 id="TabelLabel" >Products</h1>
        {contents}
      </div>
    );
  }

  async populateProductsData(categoryId) {
    const category = categoryId ? '/category=' + categoryId : '';
    const response = await fetch('product'+category);
    const data = await response.json();
    this.setState({ products: data, loading: false });
  }

  async initializeCart(cartId){
    const response = await fetch('cart/'+cartId);
    const data = await response.json();
    this.setState({ cartCount: data.totalCount, cartPrice: data.totalPrice, loading: false });
  }

   addToCart = async(productId, cartId) => {
    const rawResponse = await fetch('cart', {
      method: 'PUT',
      headers: {
        'Accept': 'application/json',
        'Content-Type': 'application/json'
      },
      body: JSON.stringify({
        CartId: cartId,
        ProductId: productId
      })
    });
    const data = await rawResponse.json();
    this.setState({cartCount: data.totalCount, cartPrice: data.totalPrice})
  }
}
