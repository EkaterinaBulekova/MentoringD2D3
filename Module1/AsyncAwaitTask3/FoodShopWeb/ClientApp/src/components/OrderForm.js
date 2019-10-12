import React from 'react';
import { Col, Row, Button, Form, FormGroup, Label, Input } from 'reactstrap';

const OrderForm = (props) => {
  return (
    <Form onSubmit={props.onSubmit}>
      <Row form>
        <Col md={6}>
          <FormGroup>
            <Label for="name">Name</Label>
            <Input type="text" name="name" id="Name" placeholder="enter your name" />
          </FormGroup>
        </Col>
        <Col md={6}>
          <FormGroup>
            <Label for="email">Email</Label>
            <Input type="email" name="email" id="Email" placeholder="with a placeholder" />
          </FormGroup>
        </Col>
      </Row>
      <FormGroup>
        <Label for="phone">Phone</Label>
        <Input type="phone" name="phone" id="Phone" placeholder="enter your phone"/>
      </FormGroup>
      <FormGroup>
        <Label for="address">Address</Label>
        <Input type="text" name="address" id="Address" placeholder="enter your address"/>
      </FormGroup>
      <Row form>
        <Col md={6}>
          <FormGroup>
            <Label for="exampleCity">City</Label>
            <Input type="text" name="city" id="exampleCity"/>
          </FormGroup>
        </Col>
        <Col md={4}>
          <FormGroup>
            <Label for="exampleState">State</Label>
            <Input type="text" name="state" id="exampleState"/>
          </FormGroup>
        </Col>
        <Col md={2}>
          <FormGroup>
            <Label for="exampleZip">Zip</Label>
            <Input type="text" name="zip" id="exampleZip"/>
          </FormGroup>  
        </Col>
      </Row>
      <Button color="primary">Order</Button>
    </Form>
  );
}

export default OrderForm;