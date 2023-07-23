import {createTheme, ThemeProvider} from "@mui/material/styles";
import Container from "@mui/material/Container";
import CssBaseline from "@mui/material/CssBaseline";
import Box from "@mui/material/Box";
import {Grid} from "@mui/material";
import Avatar from "@mui/material/Avatar";
import LogoutIcon from "@mui/icons-material/Logout";
import Typography from "@mui/material/Typography";
import React, {useContext} from "react";
import {useNavigate} from "react-router-dom";
import {UserContext} from "../../sharedServices/context/auth";

function NotFoundPage() {

    const navigate = useNavigate();
    const { setUser } = useContext(UserContext);
    const defaultTheme = createTheme();
    return (
        <ThemeProvider theme={defaultTheme}>
            <Container component="main" maxWidth="lg">
                <CssBaseline />
                <Box
                    sx={{
                        boxShadow: 3,
                        borderRadius: 2,
                        px: 10,
                        py: 10,
                        marginTop: 8,
                        display: 'flex',
                        flexDirection: 'column',
                        alignItems: 'center',
                        backgroundColor: 'aliceblue',
                        justifyItems:'center',
                        justifyContent: 'center'
                    }}
                >
                    <Grid container spacing={2} alignItems={"center"}>
                        <Grid item xs={2} >
                            <Avatar sx={{ m: 1, bgcolor: 'secondary.main' }}>
                                <LogoutIcon />
                            </Avatar>
                        </Grid>
                        <Grid item xs={10} >
                            <Typography component="h1" variant="h5" style={{ textDecorationColor: "primary"}}>
                             Not Found
                            </Typography>

                        </Grid>
                    </Grid>

                </Box>
            </Container>
        </ThemeProvider>
    )
}

export default NotFoundPage;
