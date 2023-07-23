import ICommonPageInterface from "../../../models/interfaces/ICommonPageInterface";
import React, {useEffect, useState} from "react";
import IPageInterface from "../../../models/interfaces/IPageInterface";
import api from "../../sharedServices/utils/AxiosInstance";
import {toast} from "react-toastify";
import {Table} from "semantic-ui-react";
import CheckCircle from "semantic-ui-react/dist/commonjs/elements/Icon/Icon";
import Icon from "semantic-ui-react/dist/commonjs/elements/Icon";
import {formatDate} from "../../sharedServices/utils/formatDate";
import Pagination from "semantic-ui-react/dist/commonjs/addons/Pagination";
import ProductModel from "../../../models/ProductModel";
import Grid from "semantic-ui-react/dist/commonjs/collections/Grid";
import Typography from "@mui/material/Typography";

export default function ProductsPage  () {

    const initialValue : ICommonPageInterface<ProductModel> = {pageNumber:0, hasNextPage: false, hasPreviousPage:false, totalCount:0, totalPages:0, items:[]}
    const [productsPage, setProductsPage] = useState<ICommonPageInterface>(initialValue);
    const [currentPage, setCurrentPage] = useState<number>(1)
    const [page, setPage] = useState<IPageInterface>({pageNumber:1,pageSize:10 });


    useEffect(() => {
        (async () => {
            const response = await api.get(`/Products/GetAllWithPagination?pageNumber=${currentPage}&pageSize=${page.pageSize}`);
            if (response.status == 200) {
                setProductsPage(response.data);
            } else {
                toast.error(response.statusText);
            }
        })();

    }, [currentPage, page.pageSize]);

    const handlePageSizeChange = (event) => {
        const newSize = parseInt(event.target.value, 10);
        setPage({...page, pageSize: newSize});
        setCurrentPage(1);
    };

    const onChange = (e, pageInfo) => {
        setCurrentPage(pageInfo.activePage);
    };

    return (
        <div>
            <Grid item xs={12} align={"center"} style={{ paddingTop: 20, paddingBottom: 20}} textAlign={"center"}>
                <Typography  component="h1" variant="h4" color="primary" gutterBottom>
                    <strong>Product List</strong>
                </Typography>
            </Grid>

            <Table size={"large"} >
                <Table.Header>
                    <Table.Row verticalAlign={"center"}>
                        <Table.HeaderCell>Order ID</Table.HeaderCell>
                        <Table.HeaderCell>Product ID</Table.HeaderCell>
                        <Table.HeaderCell>Picture</Table.HeaderCell>
                        <Table.HeaderCell>Name</Table.HeaderCell>
                        <Table.HeaderCell>Price</Table.HeaderCell>
                        <Table.HeaderCell>Sale Price</Table.HeaderCell>
                        <Table.HeaderCell>IsOnsALE</Table.HeaderCell>

                        <Table.HeaderCell>createdOn</Table.HeaderCell>
                    </Table.Row>
                </Table.Header>

                <Table.Body>
                    {productsPage?.items.map(product => (
                        <Table.Row key={product.id} textAlign={"center"} >
                            <Table.Cell>{product.orderId}</Table.Cell>
                            <Table.Cell>{product.id}</Table.Cell>
                            <Table.Cell>
                                <img
                                    style={{ maxWidth: '20%' , maxHeight: "10%"}}
                                    src={product.picture}
                                    alt={product.name}
                                />
                            </Table.Cell>
                            <Table.Cell>{product.name}</Table.Cell>
                            <Table.Cell>{product.price}</Table.Cell>
                            <Table.Cell>{product.salePrice }</Table.Cell>
                            <Table.Cell>{product.isOnSale  ? <CheckCircle name="check circle" color="green" /> :
                                <Icon name="thumbs down" />}</Table.Cell>
                            <Table.Cell>{formatDate(product.createdOn)}</Table.Cell>
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
                                    totalPages={productsPage?.totalPages}
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
