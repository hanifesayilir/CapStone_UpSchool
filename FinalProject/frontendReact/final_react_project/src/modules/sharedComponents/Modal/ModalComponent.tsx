
import { Button, Modal, Icon, Header} from 'semantic-ui-react';
import React, {ReactElement, useState} from "react";
import "./customDimmer.css";
/*import "../../../App.css"
import 'semantic-ui-css/semantic.min.css'*/

interface IModalComponentProps  {
    open?: boolean;
    onClose: () => void;
    onOpen: () =>void;
    children: ReactElement | ReactElement[];
    headerText: string;
    name: string;
}



export const ModalComponent = ({  headerText,children, onClose, open, onOpen, name } :IModalComponentProps) =>{


    return(
        <Modal className={"custom-dimmer"}
            name={name}
            dimmer="blurring"
            open={open}>
            <Header style={{ backgroundColor: "#9c27b0", color: "white", textAlign: "center"}}>{headerText}</Header>
            <Modal.Content>
                {children}
            </Modal.Content>
            <Modal.Actions>
                <Button color='#9c27b0' onClick={onClose}>
                    <Icon name='remove' /> Close
                </Button>

            </Modal.Actions>
        </Modal>
    )
}


