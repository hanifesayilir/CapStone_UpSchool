import {useNavigate} from "react-router-dom";
import React, {useEffect, useState} from 'react';
import { useParams } from 'react-router-dom';
import api from "../../sharedServices/utils/AxiosInstance";
import {toast} from "react-toastify";
import OrderEventsPage from "./OrderEventsPage";
import OrderEventModel from "../../../models/OrderEventModel";
import {Table} from "semantic-ui-react";
import Pagination from "semantic-ui-react/dist/commonjs/addons/Pagination";
import ItemContent from "semantic-ui-react/dist/commonjs/views/Item/ItemContent";
import CheckCircle  from 'semantic-ui-react/dist/commonjs/elements/Icon/Icon';
import Icon from "semantic-ui-react/dist/commonjs/elements/Icon/Icon";



export default function OrderEventPage({id, orderNumber}) {

const [orderEvents, setOrderEvents] = useState<OrderEventModel[]>([])

    useEffect(() => {

        (async () => {
            const response = await api.post(`/OrderEvents/GetAllByOrderId`, {orderId: id});

            if (response.status == 200) {
                setOrderEvents(response.data);
            } else {
                toast.error(response.statusText);
            }

        })();
    }, [id]);

    const formatDate = (dateString) => {
        const date = new Date(dateString);
        const formattedDate = date.toLocaleString('en-GB', {
            day: '2-digit',
            month: '2-digit',
            year: 'numeric',
            hour: '2-digit',
            minute: '2-digit',
            second: '2-digit',
        });
        return formattedDate;
    };

    return(<>

            <h1>Order Event List</h1>

        <Table size={"large"} >
                <Table.Header>
                    <Table.Row verticalAlign={"center"}>
                        <Table.HeaderCell>Order Event ID</Table.HeaderCell>
                        <Table.HeaderCell>All Products</Table.HeaderCell>
                       <Table.HeaderCell>Order Request Quantity</Table.HeaderCell>
                        <Table.HeaderCell>Order Actual Quantity</Table.HeaderCell>
                        <Table.HeaderCell>Order Event Status</Table.HeaderCell>
                        <Table.HeaderCell>Created On</Table.HeaderCell>
                    </Table.Row>
                </Table.Header>


                <Table.Body >
                    {orderEvents?.map(orderevent => (
                        <Table.Row key={orderevent.id} textAlign={"center"}>
                            <Table.Cell>{orderevent.id}</Table.Cell>
                            <Table.Cell>{orderevent.requestedAll ? <CheckCircle name="check circle" color="green" /> :
                                <Icon name="thumbs down" />}</Table.Cell>
                            <Table.Cell>{orderevent.requestedQuantity}</Table.Cell>
                            <Table.Cell>{orderevent.actualQuantity}</Table.Cell>
                            <Table.Cell>{orderevent.status}</Table.Cell>
                            <Table.Cell>{formatDate(orderevent.createdOn)}</Table.Cell>
                        </Table.Row>
                    ))}
                </Table.Body>

            </Table>
    </>)
}




