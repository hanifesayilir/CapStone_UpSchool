class UserModel {
    id: string;
    email: string;
    lastName: string;
    firstName: string;
    accessToken: string;
    expires: string;


    constructor(id: string, email: string, lastName: string, firstName: string, accessToken: string, expires: string) {
        this.id = id;
        this.email = email;
        this.lastName = lastName;
        this.firstName = firstName;
        this.accessToken = accessToken;
        this.expires = expires;
    }
}
export default UserModel;
