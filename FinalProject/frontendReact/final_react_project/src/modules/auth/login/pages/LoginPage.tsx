import * as React from 'react';
import { createTheme, ThemeProvider } from '@mui/material/styles';
import Avatar from '@mui/material/Avatar';
import Button from '@mui/material/Button';
import CssBaseline from '@mui/material/CssBaseline';
import TextField from '@mui/material/TextField';
import Link from '@mui/material/Link';
import Grid from '@mui/material/Grid';
import Box from '@mui/material/Box';
import LockOutlinedIcon from '@mui/icons-material/LockOutlined';
import Typography from '@mui/material/Typography';
import Container from '@mui/material/Container';
import { Formik, Form, Field } from 'formik';
import * as Yup from 'yup';
import {InputAdornment} from "@mui/material";
import LockIcon from '@mui/icons-material/Lock';
import EmailIcon from '@mui/icons-material/Email';
import { useNavigate } from "react-router-dom";
import {useContext, useState} from "react";
import api from "../../../sharedServices/utils/AxiosInstance";
import {toast} from "react-toastify";
import {UserContext} from "../../../sharedServices/context/auth";
import {getClaimsFromJwt} from "../../../sharedServices/utils/JwtHelper";
import GoogleIcon from '@mui/icons-material/Google';
import LocalJwt from "../../../../models/LocalJwt";


const defaultTheme = createTheme();


const validationSchema = Yup.object({
    email: Yup.string().email('Invalid email address').required('Email is required'),
    password: Yup.string().min(6, "Minimum characters should be 6").required('Password is required'),
});

export default function LoginPage() {

    const BASE_URL = import.meta.env.VITE_API_URL;

    const navigate = useNavigate();

    const { setUser} = useContext(UserContext);


    const handleSubmit = async (values, { setSubmitting }) => {
        try {
            const response = await api.post("/Authentication/Login", values);

            if (response.status === 200) {
                const accessToken = response.data.accessToken;
                const {uid, email, given_name, family_name} = getClaimsFromJwt(accessToken);

                const expires: string = response.data.expires;

                setUser({ id:uid, email, firstName:given_name, lastName:family_name, expires, accessToken });

                const localJwt: LocalJwt = {
                    accessToken,
                    expires
                }

                localStorage.setItem("userTokenStorage", JSON.stringify(localJwt));
                navigate("/");


            }

            else {
                toast.error(response.statusText)
            }
        } catch (error) {

            toast.error("Your Email or password is incorrect.");
        }
    };

    const onGoogleLoginClick = (e: React.FormEvent) => {
        e.preventDefault();
        window.location.href = `${BASE_URL}/Authentication/GoogleSignInStart`;
    };

    return (
        <ThemeProvider theme={defaultTheme}>
            <Container component="main" maxWidth="xs">
                <CssBaseline />
                <Box
                    sx={{
                        boxShadow: 3,
                        borderRadius: 2,
                        px: 4,
                        py: 6,
                        marginTop: 8,
                        display: 'flex',
                        flexDirection: 'column',
                        alignItems: 'center',
                    }}
                >
                    <Avatar sx={{ m: 1, bgcolor: 'secondary.main' }}>
                        <LockOutlinedIcon />
                    </Avatar>
                    <Typography component="h1" variant="h5">
                        Sign In
                    </Typography>
                    <Formik
                        initialValues={{
                            firstName: '',
                            lastName: '',
                            email: '',
                            password: '',
                        }}
                        validationSchema={validationSchema}
                        onSubmit={handleSubmit}
                    >
                        {({ touched, errors }) => (
                            <Form noValidate>
                                <Grid container spacing={2}>
                                    <Grid item xs={12}>
                                        <Field
                                            as={TextField}
                                            required
                                            fullWidth
                                            id="email"
                                            type="email"
                                            label="Email Address"
                                            name="email"
                                            autoComplete="email"
                                            error={touched.email && !!errors.email}
                                            helperText={touched.email && errors.email}
                                            InputProps={{
                                                startAdornment: (
                                                    <InputAdornment position="start">
                                                        <EmailIcon color="secondary"/>
                                                    </InputAdornment>
                                                ),
                                            }}
                                        />
                                    </Grid>
                                    <Grid item xs={12}>
                                        <Field
                                            as={TextField}
                                            required
                                            fullWidth
                                            name="password"
                                            label="Password"
                                            type="password"
                                            id="password"
                                            autoComplete="new-password"
                                            error={touched.password && !!errors.password}
                                            helperText={touched.password && errors.password}
                                            InputProps={{
                                                startAdornment: (
                                                    <InputAdornment position="start">
                                                        <LockIcon color="secondary"/>
                                                    </InputAdornment>
                                                ),
                                            }}

                                        />
                                    </Grid>
                                </Grid>
                                <Button type="submit" fullWidth variant="contained" sx={{ mt: 3, mb: 1 }}>
                                    Sign In
                                </Button>
                                <Button
                                    color={"secondary"}
                                    type="button"
                                    fullWidth
                                    variant="contained"
                                    sx={{ mt: 1, mb: 1 }}
                                    onClick={(e) => onGoogleLoginClick(e)}
                                >
                                    <GoogleIcon color={"primary"}/>
                                    With Google
                                </Button>
                            </Form>
                        )}
                    </Formik>
                    <Grid container justifyContent="flex-end">
                        <Grid item>
                            <Link onClick={() =>navigate("/register")} variant="body2" component={"button"}>
                                Sign Up
                            </Link>

                        </Grid>
                    </Grid>
                </Box>
            </Container>
        </ThemeProvider>
    );
}
