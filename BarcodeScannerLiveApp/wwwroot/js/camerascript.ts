let currentStream = null;
// let continueScanning = false;

export function enumerateCameras() {
    navigator.mediaDevices.getUserMedia({ video: true })
        .then(() => {
            return navigator.mediaDevices.enumerateDevices();
        })
        .then(devices => {
            const videoSelect = document.getElementById('videoSource') as HTMLSelectElement;
            videoSelect.innerHTML = '';
            devices.forEach(device => {
                if (device.kind === 'videoinput') {
                    const option = document.createElement('option');
                    option.value = device.deviceId;
                    option.text = device.label || `Camera ${videoSelect.length + 1}`;
                    videoSelect.appendChild(option);
                }
            });
        })
        .catch(error => {
            console.error("Error enumerating devices: ", error);
            alert("An error occurred while enumerating devices: " + error.message);
        });
}

export function startCamera(dotNetObject) {
    const video = document.getElementById('video') as HTMLVideoElement;
    const videoSelect = document.getElementById('videoSource') as HTMLSelectElement;
    const deviceId = videoSelect.value;

    if (currentStream) {
        stopCamera();
    }

    navigator.mediaDevices.getUserMedia({ video: { deviceId: { exact: deviceId } } })
        .then(stream => {
            currentStream = stream;
            console.log("Starting capture...");
            video.srcObject = stream;
            video.setAttribute("playsinline", "true");
            setTimeout(startDetection, 1000);
        })
        .catch(error => {
            console.error("Error accessing camera: ", error);
            if (error.name === 'NotAllowedError') {
                alert("Camera access was denied. Please allow camera access to use this feature.");
            } else if (error.name === 'NotFoundError') {
                alert("No camera found. Please connect a camera and try again.");
            } else {
                alert("An error occurred while accessing the camera: " + error.message);
            }
        });

    function startDetection() {
        if (video.readyState === video.HAVE_ENOUGH_DATA) {
            const reader = new ZXing.BrowserMultiFormatReader();
            reader.decodeFromVideoElement(video).then(
                result => {
                    stopCamera();
                    dotNetObject.invokeMethodAsync('GetResult', result.getText());
                }
            ).catch(
                e => {
                    console.error(e);
                }
            );
        }
    }
}

export function stopCamera() {
    if (currentStream) {
        currentStream.getTracks().forEach(track => {
            track.stop();
        });
        currentStream = null;
    }
}