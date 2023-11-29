import React, {FC, useEffect, useState} from "react"
import {Map, Marker} from "pigeon-maps"
import Geocode from "react-geocode";
import {useAppDispatch, useAppSelector} from "../../Utils/hooks";
import Box from '@mui/material/Box';
import Fade from '@mui/material/Fade';
import Modal from "@mui/material/Modal";
import {customStyle} from "../CartModal";
import {Button} from "@mui/material";
import {ZoomIn, ZoomOut} from "@mui/icons-material";
import {changeSingleArea} from "../../Utils/shippingSlice";

const MapComponent: FC<TPops> = ({
                                     openMap,
                                     handleClose,
                                     setValue,
                                     location,
                                     zoomLevel
                                 }) => {
    const [mapValue, setMapValue] = useState<[number, number]>([41.16197, 69.14311])
    const [zoom, setZoom] = useState<number>(6)

    const dispatch = useAppDispatch()
    const {address} = useAppSelector(state => state.shippingData)

    useEffect(() => {
        Geocode.setRegion("uz");

        Geocode.setApiKey("AIzaSyBydYec6PtEXmv0nE6VDu_KAFrnkjrJo7E");
    }, [address])

    const onSelectPlace = ({
                               event,
                               latLng,
                               pixel
                           }: { event: MouseEvent, latLng: [number, number], pixel: [number, number] }) => {
        setMapValue(latLng)
        Geocode.setApiKey("AIzaSyBydYec6PtEXmv0nE6VDu_KAFrnkjrJo7E");
        Geocode.fromLatLng(latLng[0].toString(), latLng[1].toString()).then(
            (response) => {
                const address = response.results[0].address_components.map((i: any, n: number) => {
                    if (!i.types.includes("plus_code")) return i.long_name
                }).join(" ");
                console.log("response.results", response.results)
                setValue("address", address);
                dispatch(changeSingleArea({key: "address", value: address}));
            },
            (error) => {
                console.error(error);
            }
        );
    }


    return <Modal
        open={openMap}
        onClose={() => handleClose(false)}
        aria-labelledby="modal-modal-title"
        aria-describedby="modal-modal-description"
    >
        <Fade in={openMap}>
            <Box style={{height: 600}} sx={customStyle}>
                <div className="map w-100 ">
                    <h2 className="map-h2">Select area</h2>
                    <div className="google-map">
                        <Map
                            animate={true}
                            onClick={onSelectPlace}
                            mouseEvents={true}
                            zoom={zoom}
                            center={mapValue}
                            height={500} defaultCenter={mapValue} defaultZoom={zoom}
                        ><Marker width={zoom < 12 ? 50 * 6 / zoom : 25} anchor={mapValue}/>
                        </Map>
                    </div>
                </div>
                <div className={"d-flex mt-1 justify-content-between"}>
                    <div className="d-flex">
                        <Button onClick={() => (setZoom(zoom + zoom * 0.1))}><ZoomIn/></Button>
                        <Button onClick={() => (setZoom(zoom - zoom * 0.1))}><ZoomOut/></Button>
                    </div>
                    <Button onClick={() => handleClose(false)}>close</Button>
                </div>
            </Box>
        </Fade>


    </Modal>
}
type TLocation = {
    lat: number
    lng: number
    address: string

}
type TPops = {
    handleClose: (a: boolean) => void,
    openMap: boolean,
    setValue: (a: string, b: string) => void,
    location: TLocation,
    zoomLevel: number
}
export default MapComponent