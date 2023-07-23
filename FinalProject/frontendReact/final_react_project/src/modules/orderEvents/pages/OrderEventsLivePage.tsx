import React, {useEffect, useRef, useState} from "react";
import {HubConnection, HubConnectionBuilder, HubConnectionState} from "@microsoft/signalr";
import {toast} from "react-toastify";
import {Grid, GridRow, Table} from "semantic-ui-react";
import {formatDate} from "../../sharedServices/utils/formatDate";
import GridColumn from "semantic-ui-react/dist/commonjs/collections/Grid/GridColumn";
import {ICrawlerLog} from "../../crawler/pages/CrawlerLogPage";
import {eventManager} from "react-toastify/dist/core";
import LocalJwt from "../../../models/LocalJwt";


export interface IOrderEventLive {
    id: string;
    status: string;
    createdOn: string;
    orderId: string;
}

export interface IOrderSetOrderId{
    setOrderId: (orderId: string) =>{}
}


export default function OrderEventsLivePage({setOrderId} : IOrderSetOrderId) {

    const [connectionOrderEvent, setConnectionOrderEvent] = useState<HubConnection | null>(null);
    const [orderEvents, setOrderEvents] = useState<IOrderEventLive[]>([]);
    const jwtJson  = localStorage.getItem("userTokenStorage");
    const [accessToken, setAccessToken] = useState("");
    const orderEventUrl = "https://localhost:7027/Hubs/OrderEventHub";
    const connectionStartedRef = useRef(false);

    const startConnection = async () => {
        const newConnection = new HubConnectionBuilder()
            .withUrl(orderEventUrl+"?access_token="+accessToken).build();

        try {
            await newConnection.start();
            setConnectionOrderEvent(newConnection);
        } catch (error) {
            toast.error("Error starting SignalR connection");
        }
    };

    const stopConnection = async () => {
        if (connectionOrderEvent && connectionOrderEvent.state === HubConnectionState.Connected) {
            try {
                await connectionOrderEvent.stop();
            } catch (error) {
                toast.error("Error stopping SignalR connection");
            } finally {
                setConnectionOrderEvent(null);
            }
        }
    };

    const handleOrderEvent = (event: IOrderEventLive) => {

        setOrderId(event.orderId);
        setOrderEvents((prevOrderEvents) => [...prevOrderEvents, event]);
    };

    useEffect(() => {
        if(jwtJson){
            const jwt : LocalJwt=  JSON.parse(jwtJson)
            setAccessToken(jwt.accessToken);
        }

        if (!connectionStartedRef.current) {
            startConnection();
            connectionStartedRef.current = true;
        }

        return () => {
            stopConnection();
        };
    }, []);

    useEffect(() => {
        if (connectionOrderEvent) {
            connectionOrderEvent.on("Added", handleOrderEvent);
        }

        return () => {
            if (connectionOrderEvent) {
                connectionOrderEvent.off("Added", handleOrderEvent);
            }
        };
    }, [connectionOrderEvent]);

    return(
        <>
            <Grid style={{ paddingTop: "50px"}}>
                <GridRow style={{ justifyContent:"space-between", }} >
                    <GridColumn  width={8} textAlign={"justified"}>
                        <Table celled>
                            <Table.Header>
                                <Table.Row>
                                    <Table.HeaderCell>Order Event ID</Table.HeaderCell>
                                    <Table.HeaderCell>Order Event Status</Table.HeaderCell>
                                    <Table.HeaderCell>Created On</Table.HeaderCell>
                                </Table.Row>
                            </Table.Header>


                            <Table.Body >
                                {orderEvents?.map(orderevent => (
                                    <Table.Row key={orderevent.id} >
                                        <Table.Cell>{orderevent.id}</Table.Cell>
                                        <Table.Cell>{orderevent.status}</Table.Cell>
                                        <Table.Cell>{formatDate(orderevent.createdOn)}</Table.Cell>
                                    </Table.Row>
                                ))}
                            </Table.Body>

                        </Table>
                    </GridColumn>
                    <GridColumn  width={8} textAlign={"justified"}>
                        <div className="fakeMenu">
                            <div className="fakeButtons fakeClose"/>
                            <div className="fakeButtons fakeMinimize"/>
                            <div className="fakeButtons fakeZoom"/>
                        </div>
                        <div className="fakeScreen">


                            {orderEvents.map((log, index) => (
                                <>
                                    <p className={`line${index + 1}`} key={index}>
                                        {log.status} | {formatDate(log.createdOn)}
                                    </p>
                                </>
                            ))}
                        </div>
                    </GridColumn>
                </GridRow>
            </Grid>
        </>
    )







}
