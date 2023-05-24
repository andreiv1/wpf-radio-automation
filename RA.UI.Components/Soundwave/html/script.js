const waveformContainer = document.getElementById('waveform');


const wavesurfer = WaveSurfer.create({
    container: waveformContainer,
    height: 100,
    plugins: [
        WaveSurfer.timeline.create({
            container: "#wave-timeline"
        }),
        WaveSurfer.markers.create({})
    ]
});


function addStartCue(time) {
    wavesurfer.addMarker({
        time: time,
        label: "START",
        color: '#4caf50',
        draggable: true,
        position: 'top'
    })
}

function addNextCue(time) {
    wavesurfer.addMarker({
        time: time,
        label: "NEXT",
        color: '#ff990a',
        draggable: true
    })
}

function addEndCue(time) {
    wavesurfer.addMarker({
        time: time,
        label: "END",
        color: '#f44336',
        draggable: true,
        position: 'top'
    })
}

function getMarkersTime() {
    console.log(wavesurfer.markers.markers)
    //TODO
}

function loadFile(path) {
    console.log('Load file from: ' + path);
    wavesurfer.load(path);
}

function play() {
    wavesurfer.play();
}

function pause() {
    wavesurfer.pause();
}