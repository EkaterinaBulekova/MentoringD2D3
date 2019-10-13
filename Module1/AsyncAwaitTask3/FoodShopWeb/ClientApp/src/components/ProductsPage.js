import React from 'react';
import { ProductsList, } from './ProductsList';

export const ProductsPage = props => {
     var cartId = localStorage.getItem('cartId');
     if (!cartId) {
       initializeCartId();
       cartId = localStorage.getItem('cartId');
     }

     return (
      <div>
        <ProductsList category={props.match.params.category} cartId={cartId}/>
      </div>
    );
  }

  async function initializeCartId(cartId){
    const response = await fetch('cart/'+cartId);
    const data = await response.json();
    localStorage.setItem('cartId', data.id); 
  }

