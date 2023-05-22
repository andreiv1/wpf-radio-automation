const waveformContainer = document.getElementById('waveform');
const audioFileInput = document.getElementById('audio-file-input');
const playButton = document.getElementById('play-button')
const pauseButton = document.getElementById('pause-button')
// Initialize WaveSurfer
const wavesurfer = WaveSurfer.create({
    container: waveformContainer,
    height: 100,
    plugins: [
        WaveSurfer.timeline.create({
            container: "#wave-timeline"
        })
    ]
});


playButton.addEventListener('click', function () {
    wavesurfer.play();
});


pauseButton.addEventListener('click', function () {
    wavesurfer.pause();
});


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