
import React, {useEffect, useState} from 'react';
import ICommonPageInterface from "../../../models/interfaces/ICommonPageInterface";
import IPageInterface from "../../../models/interfaces/IPageInterface";
import api from "../../sharedServices/utils/AxiosInstance";
import {toast} from "react-toastify";
import {Table} from "semantic-ui-react";
import Icon from "semantic-ui-react/dist/commonjs/elements/Icon";
import Pagination from "semantic-ui-react/dist/commonjs/addons/Pagination";
import OrderEventModel from "../../../models/OrderEventModel";
import CheckCircle from "semantic-ui-react/dist/commonjs/elements/Icon/Icon";
import {formatDate} from "../../sharedServices/utils/formatDate";
import Grid from "semantic-ui-react/dist/commonjs/collections/Grid";
import Typography from "@mui/material/Typography";

export default function OrderEventsPage() {

    const initialValue : ICommonPageInterface<OrderEventModel> = {pageNumber:0, hasNextPage: false, hasPreviousPage:false, totalCount:0, totalPages:0, items:[]}
    const [orderEventsPage, setOrderEventsPage] = useState<ICommonPageInterface>(initialValue);
    const [currentPage, setCurrentPage] = useState<number>(1)
    const [page, setPage] = useState<IPageInterface>({pageNumber:1,pageSize:10 });


    useEffect(() => {
        (async () => {
            const response = await api.get(`/OrderEvents/GetAllWithPagination?pageSize=${page.pageSize}&pageNumber=${currentPage}`);
            if (response.status == 200) {
                setOrderEventsPage(response.data);
            } else {
                toast.error(response.statusText);
            }
        })();

    }, [currentPage, page.pageSize]);

    const handlePageSizeChange = (event) => {
        const newSize = parseInt(event.target.value, 10);
        setPage({...page, pageSize: newSize});
        setCurrentPage(1); // Reset to the first page when changing page size
    };

    const onChange = (e, pageInfo) => {
        setCurrentPage(pageInfo.activePage);
    };


    return (
        <div>
            <Grid item xs={12} align={"center"} style={{ paddingTop: 20, paddingBottom: 20}} textAlign={"center"}>
                <Typography  component="h1" variant="h4" color="primary" gutterBottom>
                    <strong>Order Events</strong>
                </Typography>
            </Grid>

            <Table celled>
                <Table.Header>
                    <Table.Row>
                        <Table.HeaderCell>Order ID</Table.HeaderCell>
                        <Table.HeaderCell>Order Event ID</Table.HeaderCell>
                        <Table.HeaderCell>Order Status</Table.HeaderCell>
                        <Table.HeaderCell>Order Request All</Table.HeaderCell>
                        <Table.HeaderCell>Order Request Quantity</Table.HeaderCell>
                        <Table.HeaderCell>Order Actual Quantity</Table.HeaderCell>
                        <Table.HeaderCell>Created on</Table.HeaderCell>
                    </Table.Row>
                </Table.Header>

                <Table.Body>
                    {orderEventsPage?.items.map(orderEvent => (
                        <Table.Row key={orderEvent.id}>
                            <Table.Cell>{orderEvent.orderId}</Table.Cell>
                            <Table.Cell>{orderEvent.id}</Table.Cell>
                            <Table.Cell>{orderEvent.status}</Table.Cell>
                            <Table.Cell>{orderEvent.requestedAll ? <CheckCircle name="check circle" color="green" /> :
                                <Icon name="thumbs down" color={"red"} />}</Table.Cell>
                            <Table.Cell>{orderEvent.requestedQuantity}</Table.Cell>
                            <Table.Cell>{orderEvent.actualQuantity}</Table.Cell>
                            <Table.Cell>{formatDate(orderEvent.createdOn)}</Table.Cell>
                        </Table.Row>
                    ))}
                </Table.Body>
                <Table.Footer>
                    <Table.Row>
                        <Table.HeaderCell colSpan="12">
                            <div style={{ display: 'flex', justifyContent: 'space-between' }}>
                                <div style={{ display: 'flex', alignItems: 'center' }}>
                                    <span style={{ marginRight: '10px' }}>Page Size:</span>
                                    <select value={page.pageSize} onChange={handlePageSizeChange}>
                                        <option value="5">5</option>
                                        <option value="10">10</option>
                                        <option value="20">20</option>
                                    </select>
                                </div>

                                <Pagination
                                    activePage={currentPage}
                                    onPageChange={onChange}
                                    totalPages={orderEventsPage?.totalPages}
                                    ellipsisItem={null}
                                />
                            </div>
                        </Table.HeaderCell>
                    </Table.Row>
                </Table.Footer>
            </Table>
        </div>
    );
}
