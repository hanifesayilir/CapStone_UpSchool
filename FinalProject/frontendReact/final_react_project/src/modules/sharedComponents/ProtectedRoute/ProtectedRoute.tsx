import React, {useContext} from "react";
import {Navigate} from "react-router-dom";
import {UserContext} from "../../sharedServices/context/auth";

type ProtectedRouteProps = {
    children:React.ReactElement
}
export default function ProtectedRoute( { children } : ProtectedRouteProps ) {

    const { user } = useContext(UserContext);

    if(!user)
        return <Navigate to="/login" />

    return children;
}
