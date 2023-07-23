import React, {useEffect, useState} from 'react';
import api from "../../sharedServices/utils/AxiosInstance";
import {toast} from "react-toastify";
import {Label, Table} from "semantic-ui-react";
import ProductModel from "../../../models/ProductModel";
import CheckCircle from "semantic-ui-react/dist/commonjs/elements/Icon/Icon";
import Icon from "semantic-ui-react/dist/commonjs/elements/Icon/Icon";
import {formatDate} from "../../sharedServices/utils/formatDate"
import Grid from "semantic-ui-react/dist/commonjs/collections/Grid";
import GridRow from "semantic-ui-react/dist/commonjs/collections/Grid/GridRow";
import GridColumn from "semantic-ui-react/dist/commonjs/collections/Grid/GridColumn";
import {excelFileDownload} from "../../sharedServices/utils/excelFileDownload";
import Popup from "semantic-ui-react/dist/commonjs/modules/Popup";
import Loader from "semantic-ui-react/dist/commonjs/elements/Loader";

export interface IProductByOrderId {
    orderId: string;
}

export default function ProductByOrderIdPage ( {id, orderNumber}) {


        const [orderProducts, setProducts] = useState<ProductModel[]>([])
        const [loading, setLoading] = useState<boolean>(false)


        useEffect(() => {
            (async () => {
                const response = await api.post(`/Products/GetByOrderId`, {orderId: id});

                if (response.status == 200) {
                    setProducts(response.data);
                } else {
                    toast.error(response.statusText);
                }

            })();
        }, []);


    const handleIconExcelFileClick = async (orderId : string) => {
        setLoading(true);
        await new Promise(resolve => setTimeout(resolve, 1000));
        const resp = await excelFileDownload(orderId);
        if (resp.status !== 200) {
            toast.error("The file can not be created");
        }
        setLoading(false);
    };


        return(<>

            <Grid padded={"30px"} style={{ borderColor: "red"}}>

                <GridRow style={{ justifyContent:"space-between"}} >

                    <GridColumn width={16} textAlign={"center"}>
                        {loading === true && <Loader content='Loading' active inline style={{ color: "orange"}}/>}
                    </GridColumn>

                    <GridColumn width={6}>

                        <h1>Product List</h1>

                    </GridColumn>
                    <GridColumn width={1}>
                        <Popup
                            style={{ color: "orange"}}
                            content="Download Excel File"
                            trigger={
                                <Icon
                                    size={"big"}
                                    name="cloud download"
                                    color="orange"
                                    link
                                    onClick={() =>handleIconExcelFileClick(id)}
                                />
                            }
                        />

                    {/*    <Button animated='vertical'  onClick={() =>excelFileDownload(id)} color={"green"}>
                            <Button.Content hidden>Excel File</Button.Content>
                            <Button.Content visible>
                                <Icon name='cloud download' size={"big"}   color="orange"/>
                            </Button.Content>
                        </Button>*/}
                    </GridColumn>
                </GridRow>
            </Grid>

            <Table celled>
                <Table.Header>
                    <Table.Row>
                        <Table.HeaderCell>Product ID</Table.HeaderCell>
                        <Table.HeaderCell>Price</Table.HeaderCell>
                        <Table.HeaderCell>Name</Table.HeaderCell>
                        <Table.HeaderCell>Price</Table.HeaderCell>
                        <Table.HeaderCell>Sale Price</Table.HeaderCell>
                        <Table.HeaderCell>IsOnsALE</Table.HeaderCell>
                        <Table.HeaderCell>createdOn</Table.HeaderCell>
                    </Table.Row>
                </Table.Header>


                <Table.Body >
                    {orderProducts?.map(orderproduct => (
                        <Table.Row key={orderproduct.id} verticalAlign={"center"} >
                            <Table.Cell>{orderproduct.id}</Table.Cell>
                            <Table.Cell>{orderproduct.name}</Table.Cell>
                            <img
                                style={{ maxWidth: '20%' }}
                                src={orderproduct.picture}
                                alt={orderproduct.name}
                            />
                            <Table.Cell>{orderproduct.price}</Table.Cell>
                            <Table.Cell>{orderproduct.salePrice }</Table.Cell>
                            <Table.Cell>{orderproduct.isOnSale  ? <CheckCircle name="check circle" color="green" /> :
                                <Icon name="thumbs down" />}</Table.Cell>
                            <Table.Cell>{formatDate(orderproduct.createdOn)}</Table.Cell>
                        </Table.Row>
                    ))}
                </Table.Body>

            </Table>
        </>)
    };
