import React, {useContext, useState} from "react";
import {
    Drawer, IconButton,
    List,
    ListItem, ListItemButton, ListItemIcon,
    ListItemText,
} from '@mui/material';
import { Link } from "react-router-dom";
import MenuRoundedIcon from '@mui/icons-material/MenuRounded';
import {UserContext} from "../../sharedServices/context/auth";

const  DrawerComponent = ({links}) =>{

    const [ open, setOpen] = useState(false);
    const { user } = useContext(UserContext)
    return (
        <>
            <Drawer PaperProps={{sx:{backgroundColor:'rgba(49,49,116,1)'} }} anchor={"right"} open={open} onClose={() =>setOpen(false)}>
                <List>
                    {
                        links.map((link, index) =>(
                            <ListItemButton key={link.id}>
                                <ListItemIcon>
                                    <ListItemText sx={{ color: "white"}}>
                                        <Link to={link.link}>
                                            {link.label}
                                            {link.icon}
                                        </Link>
                                    </ListItemText>
                                </ListItemIcon>
                            </ListItemButton>
                        ))
                    }

                </List>
            </Drawer>
                <IconButton onClick={() =>user?.id ? setOpen(!open) : setOpen(false)} sx={{marginLeft:"auto", color:"white"}}>
                    <MenuRoundedIcon />
                </IconButton>

            </>
            );
}


export default DrawerComponent;
