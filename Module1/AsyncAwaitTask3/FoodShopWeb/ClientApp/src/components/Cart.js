import React from 'react';
import { NavLink } from 'reactstrap';
import { Link } from 'react-router-dom';
import './Cart.css';

export const Cart = props => {
    var{cartCount, cartPrice,cartId} = props;
     return (
      <NavLink tag={Link} className="text-dark" to={"/carts/" + cartId}>
        <div className="cart">
          <div className="cart-icon">
          </div>
          <div className="cart-text">
            {cartCount> 0 ? cartCount +" - "+ cartPrice+"$" : null}
          </div>
        </div>
      </NavLink>
    );
  }

