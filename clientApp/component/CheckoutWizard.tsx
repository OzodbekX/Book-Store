import React from 'react';
import {Step, StepLabel, Stepper} from "@material-ui/core";

function CheckoutWizard({activeStep = 0}: { activeStep: number }) {

    return (
        <Stepper activeStep={activeStep} alternativeLabel={true}>
            {["Login", "Shipping Address", 'Payment Method', 'Place Order'].map((step) => {
                return <Step key={step}>
                    <StepLabel>{step}</StepLabel>
                </Step>
            })}
        </Stepper>
    );
}

export default CheckoutWizard;