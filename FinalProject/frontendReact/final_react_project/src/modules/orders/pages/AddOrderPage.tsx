import React, {useEffect, useRef, useState} from "react";
import {HubConnection, HubConnectionBuilder, HubConnectionState} from "@microsoft/signalr";
import {toast} from "react-toastify";
import {Button, Form, Grid, Header, Icon, Image, Label, Segment, Step} from "semantic-ui-react";
import Checkbox from "semantic-ui-react/dist/commonjs/modules/Checkbox";
import Input from "semantic-ui-react/dist/commonjs/elements/Input";
import GridRow from "semantic-ui-react/dist/commonjs/collections/Grid/GridRow";
import GridColumn from "semantic-ui-react/dist/commonjs/collections/Grid/GridColumn";
import StepComponent from "../../sharedComponents/StepComponent/StepComponent";
import OrderEventsLivePage from "../../orderEvents/pages/OrderEventsLivePage";
import CrawlerLogPage from "../../crawler/pages/CrawlerLogPage";
import ProductByOrderIdPage from "../../products/pages/ProductsByOrderIdPage";
import LocalJwt from "../../../models/LocalJwt";
import NotificationPage from "../../notifications/NotificationPage";
import {useFormik, Formik} from "formik";
import * as Yup from 'yup';


interface ISendParameters{
    isAllProducts: boolean,
    isAnyNumberOfProducts: boolean
    numberOfProducts: number,
    isDiscounted: boolean,
    isNormalPriced: boolean,
    isAllPrice: boolean,
    jwtToken: string,
}







const AddOrderPage = () =>{


    const initialValue = {
        isAllProducts: false,
        isAnyNumberOfProducts: false,
        numberOfProducts: 1,
        isDiscounted: false,
        isNormalPriced: false,
        isAllPrice: false,
        jwtToken: "",
    }
    const [parameterList, setParameterList] = useState<ISendParameters>(initialValue)
    const [connection, setConnection] = useState<HubConnection | null>(null);
    const [connectionApplicationHub, setConnectionApplicationHub] = useState<HubConnection | null>(null);
    const [activeStep, setActiveStep] = useState(0);
    const jwtJson  = localStorage.getItem("userTokenStorage");
    const [accessToken, setAccessToken] = useState("");
    const [orderId, setOrderId] = useState<string>("");
    const orderParametersUrl = 'https://localhost:7027/Hubs/CrawlerSendingParameters';
    const steps = [
        {id: 1, title: "Crawl Parameters", description: "Please enter the crawling parameters"},
        {id: 2, title: "Log View", description: "Real TimeLog View"},
        {id: 3, title: "Products", description: "Product Summary Page"},

    ]


    const connectionStartedRef = useRef(false);
    const [disableSubmitButton, setDisableSubmitButton] = useState(true);



    useEffect(() => {
        const areProductsValid = parameterList.isAllProducts || parameterList.numberOfProducts > 0;
        const isPriceOptionValid =
            (parameterList.isDiscounted ? 1 : 0) +
            (parameterList.isNormalPriced ? 1 : 0) +
            (parameterList.isAllPrice ? 1 : 0) === 1;

        setDisableSubmitButton(!(areProductsValid && isPriceOptionValid));
    }, [
        parameterList.isAllProducts,
        parameterList.numberOfProducts,
        parameterList.isDiscounted,
        parameterList.isNormalPriced,
        parameterList.isAllPrice,
    ]);



    const setOrderIdFromChild = (orderId: string) =>{
        setOrderId(orderId);
    }


    const stopConnection = async () => {
        if (connection && connection.state === HubConnectionState.Connected) {
            try {
                await connection.stop();
            } catch (error) {
                toast.error("Error stopping SignalR connection:");
            } finally {
                setConnection(null);
            }
        }
    };

    const startConnection = async () => {
        const newConnection = new HubConnectionBuilder()
            .withUrl(orderParametersUrl+"?access_token="+accessToken)
            .build();

        try {
            await newConnection.start();
            setConnection(newConnection);
        } catch (error) {
            toast.error("Error starting SignalR connection");
        }
    };


    useEffect(() => {

        if(jwtJson){
           const jwt : LocalJwt=  JSON.parse(jwtJson)
            setAccessToken(jwt.accessToken);
            setParameterList({...parameterList, jwtToken:  jwt.accessToken});
        }

        if (!connectionStartedRef.current) {
            startConnection();
            connectionStartedRef.current = true;
        }

      return () => {
            stopConnection();
        };
    }, []);

    const sendParameters = async () =>{

      // ;
        if ((parameterList.isAllProducts || parameterList.numberOfProducts>0)&& (parameterList.isAllPrice || parameterList.isDiscounted || parameterList.isNormalPriced)) {
            handleStepClick(activeStep + 1)
            await connection?.invoke("SendParameters", parameterList).catch((e)=>toast.error(e));


        }

        else {
            toast.error("Either AllProducts or number of Products should be entered and one of AllPrice, Discounted or Normal Prices should be chosen");
            return;
        }
    }

    useEffect( () =>{
        if (connection) connection.on("ReceiveMessages", message =>{
            if (activeStep ===1 )toast.success(message);
            handleStepClick(activeStep + 1);
        })

        return () => {
            if (connection) {
                connection.off("ReceiveMessages");
            }
        };
    },[activeStep, connection])


    useEffect ( () =>{
        if (parameterList.isAllProducts) {
        setParameterList((prevParameters) => ({
            ...prevParameters,
            numberOfProducts: 0, isAnyNumberOfProducts: false,
        }))
    } else {
            setParameterList((prevParameters) => ({
                ...prevParameters,
               isAnyNumberOfProducts: true,

            }))
        }

    },[parameterList.isAllProducts])


    const handleStepClick = (stepIndex) => {
        setActiveStep(stepIndex);
    };

    const handleNumberOfItemsChange = (event) => {
        const inputValue = event.target.value;
        const digitsOnly = inputValue.replace(/\D/g, "");
        if (!isNaN(digitsOnly) && parseInt(digitsOnly) >= 0) {
            setParameterList({ ...parameterList, numberOfProducts: parseInt(digitsOnly) });
        } else {
            setParameterList({ ...parameterList, numberOfProducts: 0 });
        }
    };

    const onNewOrderHandler = () =>{
        setActiveStep(0);
        setParameterList({...initialValue, jwtToken:  accessToken});

    }


    return(
        <>
            { activeStep == 2 &&
            <Grid>
                <Grid.Row>
                    <Grid.Column width={13}></Grid.Column>
                    <Grid.Column width={2} >
                        <Button fluid type="button" onClick={onNewOrderHandler} >
                            <Icon name="plus" />New Order
                        </Button>
                    </Grid.Column>
                </Grid.Row>
            </Grid>


            }


            <GridRow style={{ justifyContent:"space-between", paddingBottom: "50px", paddingTop: "50px"}} >
            <StepComponent
                steps={steps}
                activeStep={activeStep}
                handleStepClick={handleStepClick}
            />
            </GridRow>

            { activeStep === 0 &&   <Form onSubmit={sendParameters}>
               <Segment stacked style={{ paddingTop: "2em"}}>
                   <Header as="h2" style={{ color: "#9c27b0"}} textAlign="center" >
                       Crawler Parameters
                   </Header>
                     <Grid padded={"30px"} style={{ borderColor: "red"}}>

                <GridRow style={{ justifyContent:"space-between" }} >

                        <GridColumn width={6}>
                            <Form.Field>
                                <label>All Products</label>
                                <Checkbox
                                    name={"isAllProducts"}
                                    checked={parameterList.isAllProducts}
                                    onChange={() =>
                                        setParameterList({...parameterList, isAllProducts: !parameterList.isAllProducts})}
                                />
                            </Form.Field>


                        </GridColumn>
                        <GridColumn width={6}>
                            <Form.Field>
                                <label>Number Of Items</label>
                                <Input
                                    name={"numberOfItems"}
                                    disabled={parameterList.isAllProducts ? true : false}
                                    id="numberOfItems"
                                    type="text"
                                    pattern={"[0-9]"}
                                    value={parameterList.numberOfProducts}
                                    onChange={handleNumberOfItemsChange}
                                    // onChange={(event) => setParameterList({...parameterList, numberOfProducts: parseInt(event?.target.value as string)})}
                                />
                                {(!parameterList.isAllProducts && parameterList.numberOfProducts < 1) && (
                                    <span style={{ color: 'red' }}>Number of items should be greater than 0</span>
                                )}
                            </Form.Field>
                        </GridColumn>
                        <GridColumn width={4}></GridColumn>
                    </GridRow>


                <GridRow style={{ justifyContent:"space-between" }} >
                                <GridColumn width={4}>
                                    <Form.Field>
                                        <label>All prices</label>
                                        <Checkbox
                                            name={"isAllPrice"}
                                            checked={parameterList.isAllPrice}
                                            onChange={() => setParameterList({...parameterList,
                                                isDiscounted: false,
                                                isNormalPriced: false,
                                                isAllPrice: !parameterList.isAllPrice})}
                                            //onChange={handleCheckboxChange}
                                        />
                                    </Form.Field>
                                </GridColumn>
                                <GridColumn width={4}>
                                    <Form.Field>
                                        <label>Normal prices</label>
                                        <Checkbox
                                            name={"isNormalPriced"}
                                            checked={parameterList.isNormalPriced}
                                           onChange={() => setParameterList({...parameterList,
                                               isAllPrice: false,
                                               isDiscounted: false,
                                               isNormalPriced: !parameterList.isNormalPriced})}
                                        />
                                    </Form.Field>
                                </GridColumn>
                                <GridColumn width={4}>
                                    <Form.Field>
                                    <label>Discounted Prices</label>
                                    <Checkbox
                                        name={"isDiscounted"}
                                        checked={parameterList.isDiscounted}
                                        onChange={() => setParameterList({...parameterList, isDiscounted: !parameterList.isDiscounted,   isAllPrice: false,isNormalPriced: false})}
                                       // onChange={handleCheckboxChange}
                                        />
                                    </Form.Field>
                                </GridColumn>
                </GridRow>
                        <GridRow style={{ justifyContent:"flex-end"}}>
                            <GridColumn  width={2} textAlign={"justified"}>
                                <Button  fluid size="small" type="submit" disabled={disableSubmitButton}>
                                    Start Crawler
                                </Button>
                            </GridColumn>
                        </GridRow>
            </Grid>
               </Segment>
            </Form> }
            { activeStep === 1 && <OrderEventsLivePage setOrderId={setOrderIdFromChild}/>}
            { activeStep === 1 && <CrawlerLogPage />}
            { activeStep === 2 && <ProductByOrderIdPage id={orderId}/>}
            </>
    )
}

export default AddOrderPage;
