import React, { Component } from "react";
import RequestView from "./requestView";
import Container from 'react-bootstrap/Container';
import Row from 'react-bootstrap/Row';
import Col from 'react-bootstrap/Col';
import Operation from "../types/Operation";

class MainView extends Component {
    apiGetCall = async () => {
        console.log("It was called")

        let response = await fetch('http://localhost:5000/api/editrecord/hostname=www.joy.com&type=A')
            .then(response => response.json())

        return response["hostName"]
    
    }

    apiPostCall = async () => {
        console.log("It was called")

        let response = await fetch('http://localhost:5000/api/editrecord/hostname=www.joy.com&type=A')
            .then(response => response.json())

        return response["hostName"]
    }

    apiDeleteCall = async () => {
        console.log("It was called")

        let response = await fetch('http://localhost:5000/api/editrecord/hostname=www.joy.com&type=A')
            .then(response => response.json())

        return response["hostName"]
    }

    list: Array<Operation> = [
        new Operation("Retrieve DNS Entry", "Retrieve", "Output", this.apiGetCall),
        new Operation("Add DNS Entry", "Add", "Output", this.apiPostCall),
        new Operation("Remove DNS Entry", "Remove", "Output", this.apiDeleteCall)
    ]
    
    render() {

        console.log(this.list);

        return(
            <Container>  
                <Row>
                    {this.list.map((op: Operation) => (
                        <Col>
                            <RequestView op = {op} />
                        </Col>
                    ))}
                </Row>
                
            </Container>
        )
    }
    
    
}



export default MainView;