import {useSearchParams, useNavigate} from "react-router-dom";
import {useContext, useEffect} from "react";
import {getClaimsFromJwt} from "../../sharedServices/utils/JwtHelper";
import LocalJwt from "../../../models/LocalJwt";
import {UserContext} from "../../sharedServices/context/auth";



function SocialLogin() {

    const [searchParams] = useSearchParams();

    const navigate = useNavigate();

    const { setUser } = useContext(UserContext);

    useEffect(() => {

        const accessToken = searchParams.get("access_token");

        const expiryDate = searchParams.get("expiry_date");

        const { uid, email, given_name, family_name} = getClaimsFromJwt(accessToken);

        const expires:string = expiryDate;

        setUser({ id:uid, email, firstName:given_name, lastName:family_name, expires, accessToken });

        const localJwt:LocalJwt ={
            accessToken,
            expires
        }

        localStorage.setItem("userTokenStorage", JSON.stringify(localJwt));

        navigate("/");

    },[]);

    return (
        <div></div>
    );
}

export default SocialLogin;
