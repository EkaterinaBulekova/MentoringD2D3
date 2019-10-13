import React from 'react';
import { Col, Row, Button, Form, FormGroup, Label, Input } from 'reactstrap';

const OrderForm = (props) => {
  return (
    <Form onSubmit={props.onSubmit}>
      <Row form>
        <Col md={6}>
          <FormGroup>
            <Label for="name">Name</Label>
            <Input type="text" name="name" id="name" placeholder="enter your name" required/>
          </FormGroup>
        </Col>
        <Col md={6}>
        <FormGroup>
          <Label for="phone">Phone</Label>
          <Input type="phone" name="phone" id="phone" placeholder="enter your phone" pattern="^((8|\+7)[\- ]?)?(\(?\d{3}\)?[\- ]?)?[\d\- ]{7,10}$" required/>
        </FormGroup>
        </Col>
      </Row>
      <FormGroup>
        <Label for="address">Address</Label>
        <Input type="text" name="address" id="address" placeholder="enter your address" required/>
      </FormGroup>
      <Row form>
        <Col md={6}>
          <FormGroup>
            <Label for="city">City</Label>
            <Input type="text" name="city" id="city" required/>
          </FormGroup>
        </Col>
        <Col md={4}>
          <FormGroup>
            <Label for="State">State</Label>
            <Input type="text" name="state" id="state"  required/>
          </FormGroup>
        </Col>
        <Col md={2}>
          <FormGroup>
            <Label for="zip">Zip</Label>
            <Input type="text" name="zip" id="zip" required/>
          </FormGroup>
        </Col>
      </Row>
      <Button color="primary">Order</Button>
    </Form>
  );
}

export default OrderForm;