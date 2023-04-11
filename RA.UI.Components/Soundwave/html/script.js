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
    let markers = wavesurfer.markers.markers;
    let start = markers[0].time;
    let next = markers[1].time;
    let end = markers[2].time;
    //let result = "start=${start};next=${next};stop=${end}"
    let result = "start=" + start + ";next=" + next + ";stop=" + end;
    console.log(result)
    return result;
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