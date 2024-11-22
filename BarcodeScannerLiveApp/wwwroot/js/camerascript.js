"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
exports.enumerateCameras = enumerateCameras;
exports.startCamera = startCamera;
exports.stopCamera = stopCamera;
var currentStream = null;
// let continueScanning = false;
function enumerateCameras() {
    navigator.mediaDevices.getUserMedia({ video: true })
        .then(function () {
        return navigator.mediaDevices.enumerateDevices();
    })
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
    })
        .catch(function (error) {
        console.error("Error enumerating devices: ", error);
        alert("An error occurred while enumerating devices: " + error.message);
    });
}
function startCamera(dotNetObject) {
    var video = document.getElementById('video');
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
    })
        .catch(function (error) {
        console.error("Error accessing camera: ", error);
        if (error.name === 'NotAllowedError') {
            alert("Camera access was denied. Please allow camera access to use this feature.");
        }
        else if (error.name === 'NotFoundError') {
            alert("No camera found. Please connect a camera and try again.");
        }
        else {
            alert("An error occurred while accessing the camera: " + error.message);
        }
    });
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
function stopCamera() {
    if (currentStream) {
        currentStream.getTracks().forEach(function (track) {
            track.stop();
        });
        currentStream = null;
    }
}
//# sourceMappingURL=camerascript.js.map