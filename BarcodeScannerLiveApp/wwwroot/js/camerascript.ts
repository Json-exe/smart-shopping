let currentStream = null;
// let continueScanning = false;

export function enumerateCameras() {
    navigator.mediaDevices.enumerateDevices()
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
        });
}

export function startCamera(dotNetObject) {
    const video = document.getElementById('video') as HTMLVideoElement;
    // const canvas = document.getElementById('canvas') as HTMLCanvasElement;
    // const context = canvas.getContext('2d');
    const videoSelect = document.getElementById('videoSource') as HTMLSelectElement;
    const deviceId = videoSelect.value;

    if (currentStream) {
        stopCamera();
    }
    
    navigator.mediaDevices.getUserMedia({video: {deviceId: {exact: deviceId}}})
        .then(stream => {
            currentStream = stream;
            console.log("Starting capture...");
            video.srcObject = stream;
            video.setAttribute("playsinline", "true");
            setTimeout(startDetection, 1000);
        });

    // function tick() {
    //     if (!continueScanning) return;
    //     if (video.readyState === video.HAVE_ENOUGH_DATA) {
    //         canvas.height = video.videoHeight;
    //         canvas.width = video.videoWidth;
    //         context.drawImage(video, 0, 0, canvas.width, canvas.height);
    //
    //         // Capture the frame and send it to C#
    //         const imageDataUrl = canvas.toDataURL('image/png');
    //         const base64ImageData = imageDataUrl.split(',')[1];
    //         dotNetObject.invokeMethodAsync('ProcessFrame', base64ImageData);
    //     }
    //
    //     setTimeout(tick, 500);
    // }

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
                    console.error(e)
                }
            )
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