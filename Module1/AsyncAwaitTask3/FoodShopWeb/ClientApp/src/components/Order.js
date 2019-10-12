import React, { useState } from 'react';
import { Button, Modal, ModalHeader, ModalBody, ModalFooter } from 'reactstrap';
import OrderForm from './OrderForm'
import './order.css';

const Order = (props) => {
  const {
    buttonLabel,
    className
  } = props;

  const [modal, setModal] = useState(false);

  const toggle = () => setModal(!modal);

  const onFormSubmit = (e) => {
    e.preventDefault();
    console.log(e.target.value);
    console.log(e.target.name.value);
    console.log(e.target.city.value);
    setModal(!modal);
  };

  return (
    <div>
      <Button className="order-button" color="primary" onClick={toggle}>{buttonLabel}</Button>
      <Modal isOpen={modal} toggle={toggle} className={className}>
        <ModalHeader toggle={toggle}>Order Detail</ModalHeader>
        <ModalBody>
          <OrderForm onSubmit={onFormSubmit}/>
        </ModalBody>
        <ModalFooter>
        </ModalFooter>
      </Modal>
    </div>
  );
}

export default Order;