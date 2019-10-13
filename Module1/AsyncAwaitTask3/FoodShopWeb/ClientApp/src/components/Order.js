import React, { useState } from 'react';
import { Button, Modal, ModalHeader, ModalBody, ModalFooter, NavLink } from 'reactstrap';
import { Link } from 'react-router-dom';
import OrderForm from './OrderForm'
import './order.css';

const Order = (props) => {
  const {
    buttonLabel,
    className,
    cartId
  } = props;

  const [modal, setModal] = useState(false);
  const [nestedModal, setNestedModal] = useState(false);
  const [closeAll, setCloseAll] = useState(false);

  const toggle = () => setModal(!modal);
  const toggleNested = () => {
    setNestedModal(!nestedModal);
    setCloseAll(false);
  }

  const newOrderSet = async(ship) => {
    const rawResponse = await fetch('order', {
    method: 'PUT',
    headers: {
      'Accept': 'application/json',
      'Content-Type': 'application/json'
    },
    body: JSON.stringify({
      CartId: cartId,
      Order: {
        Name: ship.Name,
        Phone: ship.Phone,
        Address: ship.Address,
        City: ship.City,
        State: ship.State,
        Zip: ship.Zip
      }
     })
  });
  return await rawResponse.json();
}

  const onFormSubmit = (e) => {
    e.preventDefault();
    const orderForm = {
      Name: e.target.name.value,
      Phone: e.target.phone.value,
      Address: e.target.address.value,
      City: e.target.city.value,
      State: e.target.state.value,
      Zip: e.target.zip.value,
    }

    newOrderSet(orderForm).then(
      result => {
        if (result !== 'Internal server error'){
          toggleNested();
        } 
        else{
          alert('Sorry, sonthing went wrong. We cannot submit your order.')
          toggle();
        }
      }
    );
  };

  return (
    <div>
      <Button className="order-button" color="primary" onClick={toggle}>{buttonLabel}</Button>
      <Modal isOpen={modal} toggle={toggle} className={className}>
        <ModalHeader toggle={toggle}>Order Detail</ModalHeader>
        <ModalBody>
          <OrderForm onSubmit={onFormSubmit}/>
          <Modal isOpen={nestedModal} toggle={toggleNested} onClosed={closeAll ? toggle : undefined}>
            <ModalBody>
              <h3>Thank you for your purchase</h3>
              <h4>Your order was submited successfully.</h4>
            </ModalBody>
            <ModalFooter>
            <NavLink tag={Link} to={"/"}><Button color="primary">Close</Button>{' '}</NavLink>
            </ModalFooter>
          </Modal>
        </ModalBody>
        <ModalFooter>
        </ModalFooter>
      </Modal>
    </div>
  );
}

export default Order;