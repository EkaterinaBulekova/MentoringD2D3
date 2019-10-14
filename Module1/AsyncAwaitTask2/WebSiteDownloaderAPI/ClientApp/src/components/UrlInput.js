import React from 'react';
import { Col, Row, Button, Form, Input } from 'reactstrap';
import './UrlInput.css';

export const UrlInput = (props) => {
  return (
    <Form md={24} onSubmit={props.onSubmit} className="input-form">
      <Row form>
        <Col md={10}>
          <Input type="url" bsSize="lg" name="site" id="site" placeholder="Please enter new URL here" required/>
        </Col>
        <Col md={2}>
          <Button color="success" size="lg"> Add </Button>
        </Col>
      </Row>
    </Form>)};

