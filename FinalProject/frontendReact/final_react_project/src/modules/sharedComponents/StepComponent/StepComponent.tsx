import React, { useState } from 'react';
import { Step, Segment, Button, Icon } from 'semantic-ui-react';


const StepComponent = ({ steps, activeStep, handleStepClick }) => {

    return (
        <div>
            <Step.Group size='tiny' fluid>
                {steps.map((step, index) =>
                    (
                    <Step
                        key={index}
                        active={activeStep === index}
                        completed={(activeStep > index || activeStep === steps.length-1)}
                    >
                        <Icon name={activeStep > index ? 'check' : ''} />
                        <Step.Content>
                            <Step.Title>{step.title}</Step.Title>
                            <Step.Description>{step.description}</Step.Description>
                        </Step.Content>
                    </Step>
                ))}
            </Step.Group>
        </div>
    );
};

export default StepComponent;
