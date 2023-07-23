

class LocalJwt {
    accessToken: string;
    expires: string;


    constructor(accessToken: string, expires: string) {
        this.accessToken = accessToken;
        this.expires = expires;
    }
}

export default LocalJwt;
