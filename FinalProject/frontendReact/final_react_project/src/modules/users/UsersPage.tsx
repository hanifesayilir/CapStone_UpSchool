import React, {useEffect, useState} from "react";
import api from "../sharedServices/utils/AxiosInstance";
import {toast} from "react-toastify";
import UserModel from "../../models/UserModel";
import IPageInterface from "../../models/interfaces/IPageInterface";
import ICommonPageInterface from "../../models/interfaces/ICommonPageInterface";
import OrderModel from "../../models/OrderModel";
import {Table} from "semantic-ui-react";
import Pagination from "semantic-ui-react/dist/commonjs/addons/Pagination";
import {formatDate} from "../sharedServices/utils/formatDate";
import Typography from "@mui/material/Typography";
import {createTheme, ThemeProvider, useTheme} from "@mui/material/styles";
import Grid from "@mui/material/Grid";
import Container from "@mui/material/Container";


export const UsersPage = () =>{

    const defaultTheme = createTheme();
    const theme = useTheme();

    const [currentPage, setCurrentPage] = useState<number>(1)
    const [page, setPage] = useState<IPageInterface>({pageNumber:1,pageSize:10 });
    const initialValue : ICommonPageInterface<OrderModel> = {pageNumber:0, hasNextPage: false, hasPreviousPage:false, totalCount:0, totalPages:0, items:[]}
    const [userPage, setUserPage] = useState<ICommonPageInterface>(initialValue);

    useEffect( () =>{
        (async () =>{
                const resp = await api.get(`/Users/GetAll?pageSize=${page.pageSize}&pageNumber=${currentPage}`);
                if (resp.status == 200) {
                    setUserPage(resp.data);
                }
                else toast.error(resp.data.message);

        })()

    },[currentPage, page.pageSize])

    const handlePageSizeChange = (event) => {
        const newSize = parseInt(event.target.value, 10);
        setPage({...page, pageSize: newSize});
        setCurrentPage(1); // Reset to the first page when changing page size
    };

    const onChange = (e, pageInfo) => {
        setCurrentPage(pageInfo.activePage);
    };


    return(
            <ThemeProvider theme={defaultTheme}>
                <Grid item xs={12} align={"center"} sx={{mx: 'auto', pt: 4}}>
                    <Typography  component="h1" variant="h4" color="primary" gutterBottom>
                        <strong>User List</strong>
                    </Typography>
                </Grid>


                <Table celled>
                    <Table.Header>
                        <Table.Row>
                            <Table.HeaderCell>FirstName</Table.HeaderCell>
                            <Table.HeaderCell>LastName</Table.HeaderCell>
                            <Table.HeaderCell>Email</Table.HeaderCell>
                            <Table.HeaderCell>CreatedOn</Table.HeaderCell>

                        </Table.Row>
                    </Table.Header>

                    <Table.Body>
                        {userPage?.items.map(user => (
                            <Table.Row key={user.id}>
                                <Table.Cell>{user.firstName}</Table.Cell>
                                <Table.Cell>{user.lastName}</Table.Cell>
                                <Table.Cell>{user.email}</Table.Cell>
                                <Table.Cell>{formatDate(user.createdOn)}</Table.Cell>
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
                                        totalPages={userPage?.totalPages}
                                        ellipsisItem={null}
                                    />
                                </div>
                            </Table.HeaderCell>
                        </Table.Row>
                    </Table.Footer>
                </Table>
            </ThemeProvider>
    )
}
