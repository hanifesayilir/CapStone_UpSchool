import React, {useEffect, useMemo, useState} from 'react'
import 'semantic-ui-css/semantic.min.css'
import {Route, Routes} from "react-router-dom";
import {useNavigate} from "react-router-dom";
import UserModel from "./models/UserModel";
import {toast, ToastContainer} from "react-toastify";
import NavBar from "./modules/sharedComponents/NavBar/NavBar";
import {Container} from "semantic-ui-react";
import {NotificationContext, UserContext} from "./modules/sharedServices/context/auth";
import RegisterPage from "./modules/auth/register/RegisterPage";
import OrdersPage from "./modules/orders/pages/OrdersPage";

import ProductsPage from "./modules/products/pages/ProductsPage";
import OrderEventsPage from "./modules/orderEvents/pages/OrderEventsPage";
import AddOrdersPage from "./modules/orders/pages/AddOrderPage";
import SocialLogin from "./modules/auth/socialLogin/SocialLogin";
import NotificationPage from "./modules/notifications/NotificationPage"
import NotificationModel from "./models/NotificationModel";
import {UsersPage} from "./modules/users/UsersPage";
import LoginPage from "./modules/auth/login/pages/LoginPage";
import LogoutComponent from "./modules/sharedComponents/Logout/LogoutComponent";
import NotificationSettingPage from "./modules/notificationsetting/pages/NotificationSettingPage"
import {getClaimsFromJwt} from "./modules/sharedServices/utils/JwtHelper";
import LocalJwt from "./models/LocalJwt";
import NotFoundPage from "./modules/sharedComponents/NotFound/NotFoundPage";
import HomeComponent from "./modules/sharedComponents/Home/HomeComponent";




export default function App() {
  const [user, setUser] = useState<UserModel | undefined>(undefined);
  const [notifications, setNotifications] = useState<NotificationModel[]>([]);
    const navigate = useNavigate();

    useEffect(() => {

        const jwtJson = localStorage.getItem("userTokenStorage");
        if (!jwtJson) {
            navigate("/login");
            return;
        }

        const localJwt: LocalJwt = JSON.parse(jwtJson);

        const {uid, email, given_name, family_name} = getClaimsFromJwt(localJwt.accessToken);

        const expires: string = localJwt.expires;

        setUser({
            id: uid,
            email,
            firstName: given_name,
            lastName: family_name,
            expires,
            accessToken: localJwt.accessToken
        });


    }, []);

  return (
    <>
      <UserContext.Provider value={{user, setUser}}>
          <NotificationContext.Provider value={{notifications, setNotifications}}>
              <ToastContainer />
              <NavBar />
           {/*   <NavBar />*/}
              <Container className="App">
                <Routes>

                    <Route path="/logout" element={<LogoutComponent />}/>
                    <Route path="/register" element={<RegisterPage />}/>
                   <Route path="/login" element={<LoginPage />}/>
                    <Route path="/orders" element={ <OrdersPage />}/>
                    <Route path="/products" element={<ProductsPage />}/>
                    <Route path="/notifications" element={<NotificationPage />}/>
                    <Route path="/notification-setting" element={<NotificationSettingPage />}/>
                    <Route path="/order-events" element={ <OrderEventsPage />}/>
                    <Route path="/add-order" element={ <AddOrdersPage />}/>
                    <Route path="/social-login" element={<SocialLogin/>}/>
                    <Route path="/view-users" element={<UsersPage/>}/>
                    <Route path="/" element={<HomeComponent/>}/>

                  <Route path="/*" element={<NotFoundPage />}/>
                </Routes>
              </Container>
          </NotificationContext.Provider>
      </UserContext.Provider>

    </>
  )
};





