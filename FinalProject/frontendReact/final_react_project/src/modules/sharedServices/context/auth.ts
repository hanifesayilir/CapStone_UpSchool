import UserModel from "../../../models/UserModel";
import {createContext} from "react";
import NotificationModel from "../../../models/NotificationModel";

interface IUserContext {
    user: UserModel | undefined;
    setUser: React.Dispatch<React.SetStateAction<UserModel | undefined>>

}
export const UserContext = createContext<IUserContext>({
    user: undefined,
// eslint-disable-next-line @typescript-eslint/no-empty-function
    setUser: () =>{},

});

export type NotificationsContextType = {
    notifications:NotificationModel[],
    setNotifications:React.Dispatch<React.SetStateAction<NotificationModel[]>>
}

export const NotificationContext = createContext<NotificationsContextType>({
    notifications:[],
    setNotifications:() =>{},
})
