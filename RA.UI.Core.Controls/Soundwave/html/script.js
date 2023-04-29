const waveformContainer = document.getElementById('waveform');
const audioFileInput = document.getElementById('audio-file-input');
const playButton = document.getElementById('play-button')
const pauseButton = document.getElementById('pause-button')
// Initialize WaveSurfer
const wavesurfer = WaveSurfer.create({
    container: waveformContainer,
    plugins: [
        WaveSurfer.timeline.create({
            container: "#wave-timeline"
        })
    ]
});

// Handle file input change event
audioFileInput.addEventListener('change', function () {
    const files = this.files;
    if (!files || !files[0]) {
        return;
    }

    const fileUrl = URL.createObjectURL(files[0]);

    // Load the audio file

    wavesurfer.load(fileUrl);
});

// Handle play button click event
playButton.addEventListener('click', function () {
    wavesurfer.play();
});

// Handle pause button click event
pauseButton.addEventListener('click', function () {
    wavesurfer.pause();
});