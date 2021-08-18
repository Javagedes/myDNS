import React, { Component } from "react";
import Card from 'react-bootstrap/Card';
import RequestView from "./requestView";
import Container from 'react-bootstrap/Container';
import Row from 'react-bootstrap/Row';
import Col from 'react-bootstrap/Col';
import operation from "../types/operation";

class MainView extends Component {
    
    list: Array<operation> = [
        {title: "Retrieve DNS Entry", action: "Retrieve", result_placeholder: "Output"},
        {title: "Add DNS Entry", action: "Add", result_placeholder: "Output"},
        {title: "Remove DNS Entry", action: "Remove", result_placeholder: "Output"}
    ];
    
    render() {

        console.log(this.list);

        return(
            <Container>  
                <Row>
                    {this.list.map((op:operation) => (
                        <Col>
                            <RequestView title = {op.title} action = {op.action} result_placeholder = {op.result_placeholder}/>
                        </Col>
                    ))}
                </Row>
                
            </Container>
        )
    }
}

export default MainView;