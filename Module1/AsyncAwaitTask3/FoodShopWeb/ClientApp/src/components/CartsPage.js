import React from 'react';
import { CartsList } from './CartsList';

export const CartsPage = props => {
  var cartId = props.match.params.cartId

  return (
   <div>
     <CartsList cartId={cartId}/>
   </div>
 );
}
