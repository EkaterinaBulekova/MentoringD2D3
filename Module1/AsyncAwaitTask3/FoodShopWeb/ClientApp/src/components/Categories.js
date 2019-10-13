import React, { Component } from 'react';
import { NavItem, NavLink } from 'reactstrap';
import { Link } from 'react-router-dom';


export class CategoryMenu extends Component {
    static displayName = CategoryMenu.name;

  constructor(props) {
      super(props);
      this.state = { categories: [], loading: true };
      this.populateCategoriesData();
  }

  render() {
    let contents = this.state.loading
      ?
      <p><em>Loading...</em></p>
      :
      <ul className="navbar-nav flex-grow">
        < NavItem key={0}>
          <NavLink key={0} tag={Link} className="text-dark" to="/products">All Products</NavLink>
        </NavItem>
        {
          this.state.categories.map(category => 
            < NavItem key={category.id}>
              <NavLink key={category.id} tag={Link} className="text-dark" to={"/products/" + category.id}>{category.name}</NavLink>
            </NavItem>
          )
        }
      </ul>;

    return contents;
  }

    async populateCategoriesData() {
        const response = await fetch('category');
        const data = await response.json();
        this.setState({ categories: data, loading: false });
    }
}
