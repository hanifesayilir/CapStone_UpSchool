import React, {useEffect, useState} from 'react';
import {Table} from "semantic-ui-react";
import OrderModel from "../../../models/OrderModel";
import Pagination from "semantic-ui-react/dist/commonjs/addons/Pagination";
import Icon from "semantic-ui-react/dist/commonjs/elements/Icon";
import api from "../../sharedServices/utils/AxiosInstance";
import {toast} from "react-toastify";
import Popup from "semantic-ui-react/dist/commonjs/modules/Popup";
import ICommonPageInterface from "../../../models/interfaces/ICommonPageInterface"
import IPageInterface from "../../../models/interfaces/IPageInterface";
import CheckCircle from "semantic-ui-react/dist/commonjs/elements/Icon/Icon";
import {ModalComponent} from "../../sharedComponents/Modal/ModalComponent";
import OrderEventPage from "../../orderEvents/pages/OrderEventPage";
import ProductByOrderIdPage from "../../products/pages/ProductsByOrderIdPage";
import {excelFileDownload} from "../../sharedServices/utils/excelFileDownload";
import Loader from "semantic-ui-react/dist/commonjs/elements/Loader";
import Grid from "semantic-ui-react/dist/commonjs/collections/Grid";
import Typography from "@mui/material/Typography";
import "../../../App.css"





function OrdersPage() {

    const initialValue : ICommonPageInterface<OrderModel> = {pageNumber:0, hasNextPage: false, hasPreviousPage:false, totalCount:0, totalPages:0, items:[]}
    const [orderPage, setOrderPage] = useState<ICommonPageInterface>(initialValue);
    const [currentPage, setCurrentPage] = useState<number>(1)
    const [page, setPage] = useState<IPageInterface>({pageNumber:1,pageSize:10 });
    const [open, setOpen] = useState(false)
    const [productDialogOpen, setProductDialogOpen] = useState(false)
    const [orderId, setOrderId] =  useState("");
    const [orderNumber, setOrderNumber] =  useState("")
    const [loading, setLoading] =  useState(false);


    useEffect(() => {
        (async () => {
            const response = await api.get(`/Orders/GetAll?pageSize=${page.pageSize}&pageNumber=${currentPage}`);
           if (response.status == 200) {
               setOrderPage(response.data);
           } else {
               toast.error(response.statusText);
           }
        })();

    }, [currentPage, page.pageSize]);


    const handlePageChange = (pageNumber) => {
        setCurrentPage(pageNumber);
        setPage({...page, pageNumber: pageNumber});
    };


    const handlePageSizeChange = (event) => {
        const newSize = parseInt(event.target.value, 10);
        setPage({...page, pageSize: newSize});
        setCurrentPage(1);
    };

    const onChange = (e, pageInfo) => {
        setCurrentPage(pageInfo.activePage);
    };

    const handleIconClick = (orderId, orderNumber) => {
        setOpen(true);
        setOrderId(orderId);
        setOrderNumber(orderNumber);

    };

    const handleIconProductClick = (orderId ,orderNumber) => {
        setProductDialogOpen(true);
        setOrderId(orderId);
        setOrderNumber(orderNumber);
    };

    const handleIconExcelFileClick = async (orderId : string) => {
        setLoading(true);
        await new Promise(resolve => setTimeout(resolve, 1000));
      const resp = await excelFileDownload(orderId);
      if (resp.status !== 200) {
          toast.error("The file can not be created");
      }
        setLoading(false);
    };

    const handleIconDeleteOrder = async (orderId : string) => {
        setLoading(true);
            const response = await api.post(`/Orders/DeleteByOrderId`, {Id: orderId});
            if (response.status == 200) {
                const updatedList = orderPage.items.filter((x) =>x.id !== orderId)
                const totalCount = orderPage.totalCount;
                setOrderPage({
                  ...orderPage,
                    totalCount: totalCount -1, items: updatedList});
                toast.success(response.data.message)
            } else {
                toast.error(response.statusText);
            }
        setLoading(false);
    };


    return (
        <div>
            <Grid item xs={12} align={"center"} style={{ paddingTop: 20, paddingBottom: 20}} textAlign={"center"}>
                <Typography  component="h1" variant="h4" color="primary" gutterBottom>
                    <strong>Orders</strong>
                </Typography>
            </Grid>
            <Grid textAlign={"center"}>
                {loading === true && <Loader content='Loading' active inline style={{ color: "orange"}}/>}
            </Grid>

            <Table size={"large"} >
                <Table.Header >
                    <Table.Row verticalAlign={"center"}>
                        <Table.HeaderCell>Order Number</Table.HeaderCell>
                        <Table.HeaderCell>Requested All</Table.HeaderCell>
                        <Table.HeaderCell>Request Quantity</Table.HeaderCell>
                        <Table.HeaderCell>Actual Quantity</Table.HeaderCell>
                        <Table.HeaderCell>Product Crawl Type</Table.HeaderCell>
                        <Table.HeaderCell>Order Events</Table.HeaderCell>
                        <Table.HeaderCell>Products</Table.HeaderCell>
                        <Table.HeaderCell>DownLoad Excel File</Table.HeaderCell>
                        <Table.HeaderCell>Delete Order</Table.HeaderCell>
                    </Table.Row>
                </Table.Header>

                <Table.Body>
                    {orderPage?.items.map(order => (
                        <Table.Row key={order.id} textAlign={"center"}>
                            <Table.Cell>{order.orderNumber}</Table.Cell>
                            <Table.Cell>{order.requestedAll ? <CheckCircle name="check circle" color="green"/> :
                                <Icon name="thumbs down" color={"red"}/>}</Table.Cell>
                            <Table.Cell>{order.requestedQuantity}</Table.Cell>
                            <Table.Cell>{order.actualQuantity}</Table.Cell>
                            <Table.Cell>{order.productCrawlType}</Table.Cell>
                            <Table.Cell >
                                <Popup
                                    content="click to see order events"
                                    trigger={
                                        <Icon
                                            name="eye"
                                            color="green"
                                            link
                                            onClick={() => handleIconClick(order.id, order.orderNumber)}/>}
                                />
                            </Table.Cell>
                            <Table.Cell verticalAlign={"center"}>
                                <Popup
                                    content="click to see products"
                                    trigger={
                                        <Icon
                                            name="shopping cart"
                                            color="blue"
                                            link
                                            onClick={() => handleIconProductClick(order.id, order.orderNumber)}
                                        />
                                    }
                                />

                            </Table.Cell>
                            <Table.Cell verticalAlign={"center"}>
                                <Popup
                                    content="download excel file"
                                    trigger={
                                        <Icon
                                            name="file excel"
                                            color="orange"
                                            link
                                            onClick={() => handleIconExcelFileClick(order.id)}
                                        />
                                    }
                                />

                            </Table.Cell>
                            <Table.Cell verticalAlign={"center"}>
                                        <Icon
                                            name="delete"
                                            color="red"
                                            link
                                            onClick={() => handleIconDeleteOrder(order.id)}
                                        />


                            </Table.Cell>
                        </Table.Row>
                    ))}
                </Table.Body>
                <Table.Footer>
                    <Table.Row>
                        <Table.HeaderCell colSpan="12">
                            <div style={{display: 'flex', justifyContent: 'space-between'}}>
                                <div style={{display: 'flex', alignItems: 'center'}}>
                                    <span style={{marginRight: '10px'}}>Page Size:</span>
                                    <select value={page.pageSize} onChange={handlePageSizeChange}>
                                        <option value="5">5</option>
                                        <option value="10">10</option>
                                        <option value="20">20</option>
                                    </select>
                                </div>

                                <Pagination
                                    activePage={currentPage}
                                    onPageChange={onChange}
                                    totalPages={orderPage?.totalPages}
                                    ellipsisItem={null}
                                />
                            </div>
                        </Table.HeaderCell>
                    </Table.Row>
                </Table.Footer>
            </Table>

            <ModalComponent
                name={"orderEvent"}
                onClose={() => setOpen(false)}
                onOpen={() => setOpen(true)}
                headerText={`Order Number ${orderNumber} Details`}
                open={open}
                children={<OrderEventPage id={orderId} orderNumber={orderNumber}/>}
            />

            <ModalComponent
                name={"product"}
                onClose={() => setProductDialogOpen(false)}
                onOpen={() => setProductDialogOpen(true)}
                headerText={`Order Number ${orderNumber} Details`}
                open={productDialogOpen}
                children={<ProductByOrderIdPage id={orderId} orderNumber={orderNumber}/>}
            />

        </div>


    );
}

export default OrdersPage;





