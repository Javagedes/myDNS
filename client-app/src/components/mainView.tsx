import React, { Component, ReactElement } from "react";

import Container from 'react-bootstrap/Container';
import Row from 'react-bootstrap/Row';
import Col from 'react-bootstrap/Col';
import ApiPostView from "./apiPostView";
import ApiGetView from "./apiGetView";
import ApiDelView from "./apiDelView";

class MainView extends Component {

    list: Array<ReactElement> = [
        <ApiGetView/>,
        <ApiPostView/>,
        <ApiDelView/>,
    ]
    
    render() {

        console.log(this.list);

        return(
            <Container>  
                <Row>
                    {this.list.map((element: ReactElement) => (
                        <Col>
                            {element}
                        </Col>
                    ))}
                </Row>
                
            </Container>
        )
    }
    
    
}



export default MainView;