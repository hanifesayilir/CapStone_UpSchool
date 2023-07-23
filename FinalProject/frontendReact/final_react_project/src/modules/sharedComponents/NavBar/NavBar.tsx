import React, {useContext, useEffect, useRef, useState} from 'react';
import {AppBar, Toolbar, Typography, Button, IconButton, Badge, Grid, Tab} from '@mui/material';

import {
    Notifications,
    Home,
    AddCircle,
    Assignment,
    Store,
    Group,
    Settings,
    ExitToApp,
    ShoppingCartCheckout,

} from '@mui/icons-material';

import CottageIcon from '@mui/icons-material/Cottage';
import HomeIcon from '@mui/icons-material/Home';
import ShoppingCartIcon from '@mui/icons-material/ShoppingCart';
import {NotificationContext, UserContext} from "../../sharedServices/context/auth";
import {HubConnection, HubConnectionBuilder} from "@microsoft/signalr";
import NotificationModel from "../../../models/NotificationModel";
import api from "../../sharedServices/utils/AxiosInstance";
import {toast} from "react-toastify";
import Box from "@mui/material/Box";
import Tabs from "@mui/material/Tabs";
import {createTheme, ThemeProvider} from '@mui/material/styles';
import { useTheme, useMediaQuery} from "@mui/material";
import DrawerComponent from "../Drawer/DrawerComponent";

import { useNavigate } from "react-router-dom";

import { Link} from 'react-router-dom';
import LocalJwt from "../../../models/LocalJwt";
const Navbar = () => {

    const navigate = useNavigate();
    const { user } = useContext(UserContext)
    const defaultTheme = createTheme();
    const theme = useTheme();
    const isMatch = useMediaQuery(theme.breakpoints.down('lg'));
    const [connection, setConnection] = useState<HubConnection | null>(null);
    const notificationApplicationHub = "https://localhost:7027/Hubs/NotificationApplicationHub";
    const connectionStartedRef = useRef(false);
    const [message, setMessage] = useState<NotificationModel>();
    const { notifications, setNotifications} = useContext(NotificationContext);
    const jwtJson  = localStorage.getItem("userTokenStorage");
    const [accessToken, setAccessToken] = useState("");
    const [value, setValue] = useState(0);

    const links =[
            { id:1, label: "Home", link: "/", icon: <HomeIcon/>,  component: Link},
            { id:2, label: "Add Orders", link: "/add-order", icon: <AddCircle/>,  component: Link},
            { id:3, label: "Orders", link: "/orders", icon: <AddCircle/>,  component: Link},
            { id:4, label: "OrderEvents", link: "/order-events", icon:<Assignment />, component: Link},
            { id:5, label: "Products", link: "/products", icon: <Store />, component: Link},
            { id:6, label: "Notifications", link: "/notifications", icon: <Badge badgeContent={notifications?.length} color="error">
                    <Notifications/>
                </Badge>, component: Link},
            { id:7, label: "User", link: "view-users", icon: <AddCircle/>, component: Link},
            { id:8, label: "Settings", link: "/notification-setting", icon: <AddCircle/>, component: Link},


        ]

    const handleChange = (event: React.SyntheticEvent, newValue: number) => {
        setValue(newValue);
    };




    useEffect( () =>{
        (async () =>{

            if(jwtJson){
                const jwt : LocalJwt=  JSON.parse(jwtJson)
                setAccessToken(jwt.accessToken);
            }

            if(user?.id) {
                const resp = await api.get("/Notifications/GetNotificationsByUserId")
                if (resp.status == 200) {
                    setNotifications(resp.data);
                }
                else toast.error(resp.data.message);
            }

        })()

    },[user])



    const startConnection = async () => {
        const newConnection = new HubConnectionBuilder()
            .withUrl(notificationApplicationHub+"?access_token="+accessToken)
            .build();

        try {
            await newConnection.start();
            setConnection(newConnection);
        } catch (error) {
            toast.error("Error starting SignalR connection");
        }
    };

    useEffect(() => {
        if (user?.id && !connectionStartedRef.current) {
            (async () =>{
                await startConnection();
                connectionStartedRef.current = true;
            })();
        }
    }, [user?.id]);

    useEffect(() => {
        if (connection) {
            connection.on("SendApplicationNotifications", (incomingMessage) => {
                setMessage(incomingMessage);
                setNotifications([...notifications, incomingMessage]);
            });
        }

    }, [connection, notifications]);


    return (
        <ThemeProvider theme={defaultTheme}>
                <AppBar position="fixed" color="primary" className={"navbar"}>
                         <Toolbar>
                             {
                             isMatch ? (
                                     <>
                                         <Grid item xs={2}>
                                             <ShoppingCartIcon/> FINAL PROJECT
                                         </Grid>
                                          <DrawerComponent links={links}/>
                                     </>) :
                                 <Grid container sx={{placeItems: "center", placeContent: "space-between"}}>
                                         <Grid item xs={2}>
                                             <ShoppingCartIcon/> FINAL PROJECT
                                         </Grid>
                                        <Grid item={6}>
                                            {
                                                user?.id &&

                                             <Tabs
                                                 indicatorColor={"secondary"}
                                                 textColor={"inherit"}
                                                 value={value}
                                                 onChange={(e, val) => handleChange(e, val)}
                                                 centered
                                             >
                                                 {
                                                     links.map((link, index) => (
                                                         <Tab
                                                             key={link.id}
                                                             label={link.label}
                                                             icon={link.icon}
                                                             iconPosition={"start"}
                                                             to={link.link}
                                                             component={Link}
                                                             sx={{
                                                                 '&:hover': {
                                                                     backgroundColor: theme.palette.secondary.main, // Change color on hover
                                                                     color: theme.palette.common.white,
                                                                 },
                                                             }}
                                                         />
                                                     ))
                                                 }

                                             </Tabs>}
                                         </Grid>
                                         <Grid item xs={3} n>
                                             <Box display={"flex"}>
                                                 { !(user?.id) &&   (<Button sx={{marginLeft: "auto"}} variant="contained"  color={"secondary"}  onClick={() =>navigate("/login")}>Login</Button>)}
                                                 { user?.id &&  (<Button sx={{marginLeft: 1}} variant="contained"  color={"secondary"} onClick={() =>navigate("/logout")}>Logout</Button>) }
                                             </Box>
                                         </Grid>
                                     </Grid>
                             }

                         </Toolbar>
                </AppBar>
        </ThemeProvider>
    );
};

export default Navbar;

