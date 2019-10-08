import React, { Component } from 'react';
import { Button } from 'reactstrap';

export class ProductsList extends Component {
    static displayName = ProductsList.name;

  constructor(props) {
      super(props);
      this.state = { products: [], category: this.props.category, loading: true };
  }

  componentDidMount() {
    this.populateProductsData(this.props.category);
  }

  shouldComponentUpdate(nextProps) {
    if(this.props.category !== nextProps.category){
      this.populateProductsData(nextProps.category);
      return false;
    }
    return true;
  }

  static renderProductsTable(products) {
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
              <td><Button className="btn-btn-active">Add</Button></td>
            </tr>
          )}
        </tbody>
      </table>
    );
  }

  render() {
    let contents = this.state.loading
      ? <p><em>Loading...</em></p>
      : this.state.products.length > 0
        ? ProductsList.renderProductsTable(this.state.products)
        : <p><em>Not found any products</em></p>;

    return (
      <div>
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
}
