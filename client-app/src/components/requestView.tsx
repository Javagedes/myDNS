import React, { Component } from "react";
import Form from 'react-bootstrap/Form';
import Col from 'react-bootstrap/Col';
import Row from 'react-bootstrap/Row';
import Button from 'react-bootstrap/Button';
import InputGroup from 'react-bootstrap/InputGroup';
import Card from 'react-bootstrap/Card';


type Props = {
    title: string,
    action: string,
    result_placeholder: string
}
class RequestView extends Component<Props> {

    render() {
        
        const {
            title,
            action,
            result_placeholder
        } = this.props;
        
        return(
            <>
            <Card style = {{width: '25rem', height: '20rem'}}>
                <Card.Header><p className="text-dark">{title}</p></Card.Header>
                    <Card.Body>
                        <InputGroup className="mb-3">
                            <Col sm={7}>
                                <Form.Control type="email" placeholder="Enter URL" />
                            </Col>
                            <Col sm={5}>
                                
                                <Form.Select aria-label="Default select example">
                                    <option>Record Entry</option>
                                    <option value="1">A</option>
                                    <option value="2">AAAA</option>
                                </Form.Select>
                            </Col>  
                        </InputGroup>
                            
                                <div className="d-grid gap-2">
                                <Button variant="outline-primary" size="lg">{action}</Button>
                                </div>
                                        
                                <Row> 
                                    " "
                                </Row>



                                <div className="d-grid gap-2">
                                <Form.Control type="text" placeholder = {result_placeholder} />
                                </div>
                            </Card.Body>
                        </Card>
            </>
        );
    }
}

export default RequestView;