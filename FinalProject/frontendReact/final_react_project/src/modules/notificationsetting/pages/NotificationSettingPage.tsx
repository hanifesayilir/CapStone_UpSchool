import React, {useEffect, useState} from 'react';
import Container from '@mui/material/Container';
import Grid from '@mui/material/Grid';
import Checkbox from '@mui/material/Checkbox';
import Button from '@mui/material/Button';
import CssBaseline from "@mui/material/CssBaseline";
import Box from "@mui/material/Box";
import {createTheme, ThemeProvider} from "@mui/material/styles";
import {FormControlLabel} from "@mui/material";
import api from "../../sharedServices/utils/AxiosInstance";
import {toast} from "react-toastify";
import NotificationSettingModel from "../../../models/NotificationSettingModel";


const defaultTheme = createTheme();

const NotificationSettings = () => {
    const initialNotificationSetting = { isApplicationEnabled: false, isEmailEnabled: false};
    const[notificationSetting, setNotificationSetting] = useState<NotificationSettingModel>(initialNotificationSetting);

    useEffect( () =>{
        (async () =>{
            const resp = await api.get("/NotificationSettings/Get")
            if (resp.status == 200) {
                setNotificationSetting(resp.data);
            }
            else toast.error(resp.data.message);
        })()

    },[])

    const onHandleSubmit = (event) =>{
        event.preventDefault();
        (async () =>{
            const resp = await api.post("/NotificationSettings/Add", notificationSetting);
            if (resp.status == 200) {
                toast.success(resp.data.message);
            }
            else toast.error(resp.data.message);
        })()
    }

    return (

        <ThemeProvider theme={defaultTheme}>
            <Container component="main" maxWidth="md">
                <CssBaseline />
                <h1>Notification Settings</h1>
                <Box
                    sx={{
                        borderRadius: 2,
                        px: 4,
                        py: 6,
                        marginTop: 4,
                        borderColor: defaultTheme.palette.primary.main,
                        borderWidth: 2,
                        borderStyle: 'solid',
                        boxShadow: `0px 3px 5px rgba(0, 0, 0, 0.2)`,
                        backgroundColor: 'white',

                    }}
                >

                    <form onSubmit={onHandleSubmit}>
                        <Grid
                            container
                            direction="row"
                            justifyContent="space-around"
                            marginTop={'10px'}
                            alignItems={"center"}
                            rowGap={2}
                        >
                            <Grid item xs={6} >
                                <FormControlLabel
                                    control={
                                        <Checkbox
                                            id="isEmailEnabledRecent"
                                            checked={notificationSetting.isEmailEnabled ?? false}
                                            onChange={(event) => {
                                                setNotificationSetting({ ...notificationSetting, isEmailEnabled: event.target.checked});
                                            }}

                                        />
                                    }
                                    label={
                                        <span style={{ color: defaultTheme.palette.primary.main, display: 'flex', alignItems: 'center' }}>
                                            Email Notification</span>
                                    }
                                />
                            </Grid>
                            <Grid item xs={6}>
                                <FormControlLabel
                                    control={
                                        <Checkbox
                                            id="isApplicationEnabledRecent"
                                            checked={notificationSetting.isApplicationEnabled ?? false}
                                            onChange={(event, data) => {
                                                setNotificationSetting({ ...notificationSetting, isApplicationEnabled: event.target.checked});
                                            }}
                                        />
                                    }
                              label={
                                  <span style={{ color: defaultTheme.palette.primary.main, display: 'flex', alignItems: 'center' }}>
                              Application Notification</span>}

                                />

                            </Grid>
                            <Grid
                                container
                                direction="row"
                                alignItems="center"
                                justifyContent="flex-end"
                            >

                                <Grid item xs={2} >
                                    <Button type="submit" variant="contained" >
                                        Submit
                                    </Button>
                                </Grid>
                            </Grid>

                        </Grid>
                    </form>
                </Box>
            </Container>
        </ThemeProvider>

    );
};

export default NotificationSettings;
