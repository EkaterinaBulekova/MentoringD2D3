import React from 'react';
import { ProductsList, } from './Products';

export const ProductsPage = props => {
    console.log(props)
     return (
      <div>
        <ProductsList category={props.match.params.category} />
      </div>
    );
  }

