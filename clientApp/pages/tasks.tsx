import React, {ChangeEvent, useEffect, useState} from 'react';
import Layout from "../component/Layout";
import FormControl from "@mui/material/FormControl";
import InputLabel from "@mui/material/InputLabel";
import {Card, CardContent, TextField, Typography} from "@material-ui/core";
import {FormHelperText, NativeSelect} from "@mui/material";
import axiosClient from "../Utils/axios";
import Button from "@mui/material/Button";
import {useAppSelector} from "../Utils/hooks";

const Tasks = (props: any) => {
    const [subject, setSubject] = React.useState('physics');
    const [userTask, setUserTask] = React.useState<any[]>([]);
    const [innerAddress, setInnerAddress] = React.useState('');
    const {userData} = useAppSelector(store => store);

    const handleChangeSubject = (event: any) => {
        setSubject(event.target.value);
    };

    const handleChangeInnerAddress = (event: ChangeEvent<HTMLInputElement | HTMLTextAreaElement>) => {
        setInnerAddress(event.target.value);
    };

    useEffect(() => {
        getTasks()
    }, [])

    const downloadFile = (item: any) => {
        axiosClient.get(`UserTask/download?userId=${userData.userId}&fileId=${item?.userTaskId}`)
            .then(res => {
                if (res?.status == 200) {
                    let sampleArr = base64ToArrayBuffer(res.data[0].bytes);
                    saveByteArray(item?.userTaskTittle, sampleArr);
                }
            });
    }

    function saveByteArray(reportName: string, byte: any) {
        let blob = new Blob([byte], {type: "application/pdf"});
        let link = document.createElement('a');
        link.href = window.URL.createObjectURL(blob);
        let fileName = reportName;
        link.download = fileName;
        link.click();
    };

    function base64ToArrayBuffer(base64: string) {
        let binaryString = window.atob(base64);
        let binaryLen = binaryString.length;
        let bytes = new Uint8Array(binaryLen);
        for (let i = 0; i < binaryLen; i++) {
            let ascii = binaryString.charCodeAt(i);
            bytes[i] = ascii;
        }
        return bytes;
    }

    const writeCards = () => {
        if (userTask.length > 0) return userTask?.map((i: any, n) => {
            return <Card style={{width: "150px"}} className="m-1 px-2" key={n}>
                <CardContent>
                    <h4>{i?.userTaskTittle}</h4>
                    <Typography>  {i?.description}</Typography>
                    <Button onClick={() => downloadFile(i)}>Download</Button>
                </CardContent>

            </Card>
        })
        else return <Typography>waiting...</Typography>
    }


    const getTasks = () => {
        axiosClient.get('UserTask')
            .then(res => {
                if (res?.data?.succeeded) {
                    setUserTask(res.data.data)
                }
            });
    }
    return <Layout>
        <FormControl className={"m-1"} required sx={{m: 1, minWidth: 120}}>
            <InputLabel variant="standard" htmlFor="uncontrolled-native">Subject</InputLabel>
            <NativeSelect
                inputProps={{
                    name: 'subject',
                    id: 'demo-simple-select-required',
                }}
                value={subject}
                onChange={handleChangeSubject}
            >
                <option className={"m-1 p-1"} value="">
                    <em>None</em>
                </option>
                <option className={"m-1 p-1"} value={"physics"}>Physics</option>
                <option className={"m-1 p-1"} value={"math"}>Math</option>
                <option className={"m-1 p-1"} value={"english"}>English</option>
            </NativeSelect>
            <FormHelperText>Required</FormHelperText>
        </FormControl>
        <FormControl className={"m-1"} sx={{m: 1, minWidth: 120}}>
            <TextField
                id="demo-simple-select-error"
                value={innerAddress}
                label="Age"
                onChange={handleChangeInnerAddress}
            />
        </FormControl>
        <div className={"d-flex flex-wrap"}>
            {writeCards()}
        </div>
    </Layout>

}

export default Tasks;