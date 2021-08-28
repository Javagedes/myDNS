import React, { Component } from "react";
import Form from 'react-bootstrap/Form';
import Col from 'react-bootstrap/Col';
import Row from 'react-bootstrap/Row';
import Button from 'react-bootstrap/Button';
import InputGroup from 'react-bootstrap/InputGroup';
import Card from 'react-bootstrap/Card';
import DnsEntry from "../types/DnsEntry";

class ApiPostView extends Component {
    state = {
        result: "Confirmation",
        url: "Not Set",
        type: 0,
        ip: "0.0.0.0",
        ttl: 0
    }

    apiPostCall = async () => {
        
        let type = "";
        let ttl = "";

        if (Number(this.state.type) === 1)type = "A";
        else if (Number(this.state.type) === 2)type = "AAAA";
        
        if (Number(this.state.ttl) === 1)ttl = "3600";
        else if (Number(this.state.ttl) === 2)ttl= "7200";
        else if (Number(this.state.ttl) === 2)ttl= "14400";
        else if (Number(this.state.ttl) === 2)ttl= "28800";
        else if (Number(this.state.ttl) === 2)ttl= "43200";

        var e = new DnsEntry("0",this.state.url, ttl, type, this.state.ip);

        //TODO Catch bad JSON Parse
        let response = await fetch(`http://localhost:5000/api/editrecord/`, {
            method: 'POST',
            headers: {
                'Content-Type':'application/json'
            },
            body: JSON.stringify(e)
        });

        return response.ok
    }

    render() {

        const action = () => {
            this.apiPostCall().then(result => {
                let s = ""
                if (result === true) {
                    s = `${this.state.url} was successfully stored.`
                }
                else{
                    s = `${this.state.url} could not be stored.`      
                }
                this.setState({result: s})
            })           
        }
        
        return(
            <>
            <Card style = {{width: '25rem', height: '23rem'}}>
                <Card.Header><p className="text-dark">Store DNS Entry</p></Card.Header>
                    <Card.Body>
                        <InputGroup className="mb-3">
                            <Col sm={7}>
                                <Form.Control type="email" 
                                    onChange={e=> this.setState({url: e.target.value})} 
                                    placeholder="Enter URL" />
                            </Col>
                            
                            <Col sm={5}>
                                <Form.Select aria-label="Default select example"
                                    onChange={e => this.setState({type: (e.target as HTMLFormElement).value})} >
                                    <option>Record Entry</option>
                                    <option value="1">A</option>
                                    <option value="2">AAAA</option>
                                </Form.Select>
                            </Col>  
                        </InputGroup>
                        <InputGroup className="mb-3">
                            <Col sm={7}>
                                <Form.Control type="email" 
                                    onChange={e=> this.setState({ip: e.target.value})} 
                                    placeholder="Enter IP Address" />
                            </Col>
                            
                            <Col sm={5}>
                                <Form.Select aria-label="Default select example"
                                    onChange={e => this.setState({ttl: (e.target as HTMLFormElement).value})} >
                                    <option>TTL</option>
                                    <option value="1">3600</option>
                                    <option value="2">7200</option>
                                    <option value="2">14400</option>
                                    <option value="2">28800</option>
                                    <option value="2">43200</option>
                                </Form.Select>
                            </Col>  
                        </InputGroup>
                            
                                <div className="d-grid gap-2">
                                <Button variant="outline-primary" size="lg" onClick={action}>Store</Button>
                                </div>
                                        
                                <Row> 
                                " "
                                </Row>

                                <div className="d-grid gap-2">
                                <Form.Control readOnly type="text" value={this.state.result}/>
                                </div>
                            </Card.Body>
                        </Card>
            </>
        );
    }
}

export default ApiPostView;