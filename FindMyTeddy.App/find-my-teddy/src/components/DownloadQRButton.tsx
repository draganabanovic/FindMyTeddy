import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import React from "react";
import QRCode from "react-qr-code";
import { saveAs } from "file-saver";
import { Button } from "react-bootstrap";
import { faDownload, faQrcode } from "@fortawesome/free-solid-svg-icons";

interface IProps {
  className: string;
  url: string;
}
const DownloadQRButton = ({ className, url }: IProps) => {
  const downloadQR = () => {
    const qr = document.getElementById("qr");
    const serializer = new XMLSerializer();
    const svgContent = serializer.serializeToString(qr);
    const blob = new Blob([svgContent], { type: "image/svg+xml" });
    saveAs(blob, "qr-code.svg");
  };
  return (
    <>
      <Button
        className={className}
        title="Download QR-code"
        onClick={downloadQR}
      >
        <FontAwesomeIcon icon={faQrcode} />
      </Button>
      <QRCode className="d-none" id="qr" value={url} />
    </>
  );
};

export default DownloadQRButton;
