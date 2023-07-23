import DateTimeFormat = Intl.DateTimeFormat;


export default class NotificationModel {

    id: string;
    isChecked: boolean | undefined;
    content: string;
    notificationType: string;
    createdOn : Date;


    constructor(id: string, isChecked: boolean | undefined, content: string, notificationType: string, createdOn: Date) {
        this.id = id;
        this.isChecked = isChecked;
        this.content = content;
        this.notificationType = notificationType;
        this.createdOn = createdOn;
    }
}
