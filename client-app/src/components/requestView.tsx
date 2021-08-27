import React, { Component } from "react";
import Form from 'react-bootstrap/Form';
import Col from 'react-bootstrap/Col';
import Row from 'react-bootstrap/Row';
import Button from 'react-bootstrap/Button';
import InputGroup from 'react-bootstrap/InputGroup';
import Card from 'react-bootstrap/Card';
import Operation from "../types/Operation";
import { match } from "minimatch";



type Props = {
    op: Operation;
}
class RequestView extends Component<Props> {
    state = {
        result: "Results",
        url: "Not Set",
        type: 0
    }

    apiGetCall = async () => {
        console.log("It was called")

        let type = "0";

        if (Number(this.state.type) === 1) {
            type = "A"
        }
        else if (Number(this.state.type) === 2) {
            type = "AAAA"
        }

        console.log(this.state.type)

        //TODO Catch bad JSON Parse
        let response = await fetch(`http://localhost:5000/api/editrecord/hostname=${this.state.url}&type=${type}`)
            .then(response => response.json())

        //TODO edit the form to show all pieces of important data
        return response["hostName"]
    
    }

    render() {

        const action = () => {
            this.apiGetCall().then(result => {
                this.setState({result: result})
            })
            
        }
        
        return(
            <>
            <Card style = {{width: '25rem', height: '20rem'}}>
                <Card.Header><p className="text-dark">Retrieve DNS Entry</p></Card.Header>
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
                            
                                <div className="d-grid gap-2">
                                <Button variant="outline-primary" size="lg" onClick={action}>Retrieve</Button>
                                </div>
                                        
                                <Row> 
                                    " "
                                </Row>

                                <div className="d-grid gap-2">
                                <Form.Control type="text" placeholder = "Results" value={this.state.result}/>
                                </div>
                            </Card.Body>
                        </Card>
            </>
        );
    }
}

export default RequestView;