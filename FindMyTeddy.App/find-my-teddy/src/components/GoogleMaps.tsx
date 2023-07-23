import { useEffect } from "react";
import { ILastLocationModel } from "../models/petlastlocationModels";

import { GoogleMap, LoadScript, Marker } from "@react-google-maps/api";

interface IProps {
  lastLocations: ILastLocationModel[];
  positionMapLat: number;
  positionMapLng: number;
  selectedMapIndex: number;
}
const GoogleMaps = ({
  lastLocations,
  positionMapLat,
  positionMapLng,
  selectedMapIndex,
}: IProps) => {
  const containerStyle = {
    width: "100%",
    height: "60vh",
  };

  useEffect(() => {
    console.log({ lat: positionMapLat, lng: positionMapLng });
  }, [positionMapLat, positionMapLng]);

  const markers = () =>
    lastLocations.map((location, index) => (
      <Marker
        options={{
          zIndex: lastLocations.length - index,
          // icon: {
          //   url:
          //     index === selectedMapIndex
          //       ? "http://maps.google.com/mapfiles/ms/icons/green-dot.png"
          //       : "http://maps.google.com/mapfiles/ms/icons/red-dot.png",
          // },
        }}
        label={(index + 1).toString() + "."}
        position={{ lat: location.latitude, lng: location.longitude }}
      />
    ));
  return (
    <LoadScript googleMapsApiKey="">
      <GoogleMap
        mapContainerStyle={containerStyle}
        center={{ lat: positionMapLat, lng: positionMapLng }}
        zoom={14}
      >
        {markers()}
      </GoogleMap>
    </LoadScript>
  );
};

export default GoogleMaps;
