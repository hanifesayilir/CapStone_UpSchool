import React, {useContext, useEffect, useRef, useState} from 'react';
import {toast} from "react-toastify";
import api from "../sharedServices/utils/AxiosInstance";
import NotificationModel from "../../models/NotificationModel";
import {Table} from "semantic-ui-react";
import Icon from "semantic-ui-react/dist/commonjs/elements/Icon";
import Popup from "semantic-ui-react/dist/commonjs/modules/Popup";
import {formatDate} from "../sharedServices/utils/formatDate";
import {NotificationContext, UserContext} from "../sharedServices/context/auth";
import Typography from "@mui/material/Typography";
import Grid from "semantic-ui-react/dist/commonjs/collections/Grid";

const NotificationPage = () => {
    const [message, setMessage] = useState<NotificationModel>();
    const { notifications, setNotifications} = useContext(NotificationContext);




    const handleIconClick = async (notificationId : string) => {
        const response = await api.post(`/Notifications/Update`, {Id: notificationId});

        if (response.status == 200) {
            toast.success(response.data.message);
          setNotifications(notifications.filter(x=>x.id !== notificationId))
        } else {
            toast.error(response.statusText);
        }
    };


    return (
        <>
            <Grid item xs={12} align={"center"} style={{ paddingTop: 20, paddingBottom: 20}} textAlign={"center"}>
                <Typography  component="h1" variant="h4" color="primary" gutterBottom>
                    <strong>Notifications</strong>
                </Typography>
            </Grid>

            <Table celled>
                <Table.Header>
                    <Table.Row>
                        <Table.HeaderCell>Notification ID</Table.HeaderCell>
                        <Table.HeaderCell>Content</Table.HeaderCell>
                        <Table.HeaderCell>Created On</Table.HeaderCell>
                        <Table.HeaderCell>Actions</Table.HeaderCell>
                    </Table.Row>
                </Table.Header>

                <Table.Body>
                    {notifications.map(notification => (
                        <Table.Row key={notification.id}>
                            <Table.Cell>{notification.id}</Table.Cell>
                            <Table.Cell>{notification.content}</Table.Cell>
                            <Table.Cell>{formatDate(notification.createdOn)}</Table.Cell>
                            <Table.Cell>
                                <Popup
                                    content="click to set it as already checked "
                                    trigger={<Icon
                                        name="eye"
                                        color="green"
                                        link
                                        onClick={() =>handleIconClick(notification.id)}/>}
                                />
                            </Table.Cell>
                        </Table.Row>
                    ))}
                </Table.Body>
              {/*  <Table.Footer>
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
                                    totalPages={orderPage?.totalPages}
                                    ellipsisItem={null}
                                />
                            </div>
                        </Table.HeaderCell>
                    </Table.Row>
                </Table.Footer>*/}
            </Table>
        </>
    );
};

export default NotificationPage;
