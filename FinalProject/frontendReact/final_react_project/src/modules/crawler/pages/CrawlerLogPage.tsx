
import {HubConnection, HubConnectionBuilder, HubConnectionState} from "@microsoft/signalr";
import React, {useEffect, useRef, useState} from "react";
import DateTimeFormat = Intl.DateTimeFormat;
import "./fakemenu.css"
import {formatDate} from "../../sharedServices/utils/formatDate";
import {GridRow} from "semantic-ui-react";
import {toast} from "react-toastify";
import LocalJwt from "../../../models/LocalJwt";

export interface ICrawlerLog {
    message: string,
    sentOn: DateTimeFormat,
}

export default function CrawlerLogPage () {

    const [crawlerLog, setCrawlerLog] = useState<ICrawlerLog[]>([]);
    const [connection, setConnection] = useState<HubConnection | null>(null);
    const jwtJson  = localStorage.getItem("userTokenStorage");
    const [accessToken, setAccessToken] = useState("");
    const crawlerHubUrl = "https://localhost:7027/Hubs/CrawlerLogHub";
    const connectionStartedRef = useRef(false);

    const startConnection = async () => {
        const newConnection = new HubConnectionBuilder().withUrl(crawlerHubUrl+"?access_token="+accessToken).build();

        try {
            await newConnection.start();
            setConnection(newConnection);
        } catch (error) {
            toast.error("Error starting SignalR connection");
        }
    };

    const stopConnection = async () => {
        if (connection && connection.state === HubConnectionState.Connected) {
            try {
                await connection.stop();
            } catch (error) {
                toast.error("Error stopping SignalR connection:");
            } finally {
                setConnection(null);
            }
        }
    };

    const handleCrawlerLog = (log: ICrawlerLog) => {
        setCrawlerLog((prevCrawlerLog) => [...prevCrawlerLog, log]);
    };

    useEffect(() => {
        if(jwtJson){
            const jwt : LocalJwt=  JSON.parse(jwtJson)
            setAccessToken(jwt.accessToken);
        }
        if (!connectionStartedRef.current) {
            startConnection();
            connectionStartedRef.current = true;
        }

        return () => {
            stopConnection();
        };
    }, []);

    useEffect(() => {
        if (connection) {
            connection.on("NewCrawlerLogAdded", handleCrawlerLog);
        }

        return () => {
            if (connection) {
                connection.off("NewCrawlerLogAdded", handleCrawlerLog);
            }
        };
    }, [connection]);



    return(
        <>
            <GridRow style={{ justifyContent:"space-between", paddingTop: "50px"}}>

                <div className="fakeMenu">
                    <div className="fakeButtons fakeClose"/>
                    <div className="fakeButtons fakeMinimize"/>
                    <div className="fakeButtons fakeZoom"/>
                </div>
                <div className="fakeScreen">



                    {crawlerLog.map((log, index) => (
                        <>
                            <p className={`line${index + 1}`} key={index}>
                                {log.message} | {formatDate(log.sentOn)}
                            </p>
                        </>
                    ))}
                </div>
            </GridRow>

        </>
    )
}
