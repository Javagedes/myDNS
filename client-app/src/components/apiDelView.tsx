import React, { Component } from "react";
import Form from 'react-bootstrap/Form';
import Col from 'react-bootstrap/Col';
import Row from 'react-bootstrap/Row';
import Button from 'react-bootstrap/Button';
import InputGroup from 'react-bootstrap/InputGroup';
import Card from 'react-bootstrap/Card';

class ApiDelView extends Component {
    state = {
        result: "Confirmation",
        url: "Not Set",
        type: 0
    }

    apiDelCall = async () => {
        
        let type = "";

        if (Number(this.state.type) === 1) {
            type = "A"
        }
        else if (Number(this.state.type) === 2) {
            type = "AAAA"
        }

        let response = await fetch(`http://localhost:5000/api/editrecord/`, {
            method: 'DELETE',
            headers: {
                'Content-Type':'application/json'
            },
            redirect: 'follow',
            body: JSON.stringify({"hostname":this.state.url, "type":type })
        })

        return response.ok;      
    }

    render() {

        const action = () => {
            this.apiDelCall().then(result => {
                let s = ""
                if (result === true) {
                    s = `${this.state.url} was successfully deleted.`
                }
                else{
                    s = `${this.state.url} could not be deleted, or does not exist.`      
                }
                this.setState({result: s})
            })
            
        }
        
        return(
            <>
            <Card style = {{width: '25rem', height: '20rem'}}>
                <Card.Header><p className="text-dark">Delete DNS Entry</p></Card.Header>
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
                                <Button variant="outline-primary" size="lg" onClick={action}>Delete</Button>
                                </div>
                                        
                                <Row> 
                                " "
                                </Row>

                                <div className="d-grid gap-2">
                                <Form.Control type="text" value={this.state.result}/>
                                </div>
                            </Card.Body>
                        </Card>
            </>
        );
    }
}

export default ApiDelView;