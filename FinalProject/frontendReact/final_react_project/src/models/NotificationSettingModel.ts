

export default class NotificationSettingModel {

    isEmailEnabled: boolean;
    isApplicationEnabled : boolean;


    constructor(isEmailEnabled: boolean, isApplicationEnabled: boolean, emailAddress: string, userName: string) {
        this.isEmailEnabled = isEmailEnabled;
        this.isApplicationEnabled = isApplicationEnabled;
    }
}
