var audioPlayer = document.getElementById('audioPlayer1');
var initialized = 0;
function xxx() {
    $('.audio').css('background', 'transparent');
    var i = '';
    var subtitles = document.getElementById('subtitles');
    audioPlayer.currentTime = 0;
    audioPlayer.paused = false;
    var syncData = [];
    $.each(allData, function (i, item) {
        var word = document.getElementById(item.text).innerText;
        syncData.push({ text: word, start: item.start, end: item.end, ele: item.text });
    });
    audioPlayer.addEventListener('timeupdate', function (e) {
        if (initialized == 0) {
            $('.audio').css('background', 'transparent');
            return;
        }
        $.each(syncData, function (inde, dataItem) {             
            if (audioPlayer.currentTime >= dataItem.start) {
                if (inde > 0) {
                    document.getElementById(i).style.background = 'transparent';
                }
                i = dataItem.ele;               
                document.getElementById(dataItem.ele).style.background = 'yellow';
            }
        });
    });
    initialized=1
    return audioPlayer; 
}
function PlayPauseAudio(obj) {    
    if (audioPlayer.paused == false) {
        audioPlayer.pause();
        $(obj).attr("title","Play")
        $(obj).html('<i class="fa fa-play" aria-hidden="true"></i>')
    }
    else {
        if (initialized == 0) {
            xxx()
        }
        $(obj).attr("title", "Pause")
        $(obj).html('<i class="fa fa-pause" aria-hidden="true"></i>')
        audioPlayer.play();
    }
}
function StopAudio(obj) {
    audioPlayer.pause();
    audioPlayer.currentTime = 0;
    initialized = 0;
    $("#playAudio").attr("title", "Play")
    $("#playAudio").html('<i class="fa fa-play" aria-hidden="true"></i>')
    $('.audio').css('background', 'transparent');
}
