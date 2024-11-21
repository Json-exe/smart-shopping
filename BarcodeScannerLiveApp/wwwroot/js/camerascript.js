"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
exports.stopCamera = exports.startCamera = exports.enumerateCameras = void 0;
var currentStream = null;
// let continueScanning = false;
function enumerateCameras() {
    navigator.mediaDevices.enumerateDevices()
        .then(function (devices) {
        var videoSelect = document.getElementById('videoSource');
        videoSelect.innerHTML = '';
        devices.forEach(function (device) {
            if (device.kind === 'videoinput') {
                var option = document.createElement('option');
                option.value = device.deviceId;
                option.text = device.label || "Camera ".concat(videoSelect.length + 1);
                videoSelect.appendChild(option);
            }
        });
    });
}
exports.enumerateCameras = enumerateCameras;
function startCamera(dotNetObject) {
    var video = document.getElementById('video');
    // const canvas = document.getElementById('canvas') as HTMLCanvasElement;
    // const context = canvas.getContext('2d');
    var videoSelect = document.getElementById('videoSource');
    var deviceId = videoSelect.value;
    if (currentStream) {
        stopCamera();
    }
    navigator.mediaDevices.getUserMedia({ video: { deviceId: { exact: deviceId } } })
        .then(function (stream) {
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
            var reader = new ZXing.BrowserMultiFormatReader();
            reader.decodeFromVideoElement(video).then(function (result) {
                stopCamera();
                dotNetObject.invokeMethodAsync('GetResult', result.getText());
            }).catch(function (e) {
                console.error(e);
            });
        }
    }
}
exports.startCamera = startCamera;
function stopCamera() {
    if (currentStream) {
        currentStream.getTracks().forEach(function (track) {
            track.stop();
        });
        currentStream = null;
    }
}
exports.stopCamera = stopCamera;
