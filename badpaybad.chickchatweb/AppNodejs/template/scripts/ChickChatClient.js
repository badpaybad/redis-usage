jQuery('#login').show();
jQuery('#logedin').hide();

//var socket = io.connect('chickchat.badpaybad.info:9000');
var socket = io.connect('localhost:9000');

var channelKeyOwner;
var channelKeyChattingSelected;
var chickChatGroupOwner;

function DoLogin() {
    var txtUid = jQuery('#txtAnonyName').val();
    var txtTitle = jQuery('#txtGroupTitle').val();
    var txtPwd = jQuery('#txtGroupPwd').val();
    if (!txtUid || !txtTitle || txtTitle == '' || txtUid == '') {
        alert('Name and title can not be empty');
        return;
    }
    var obj = { 'txtUid': txtUid, 'txtTitle': txtTitle, txtPwd:txtPwd };
    $.ajax({
            method: "POST",
            url: "/login",
            cache: false,
            dataType: "json",
            data: obj
        })
        .done(function(res) {
            if (res.isOk) {
                channelKeyOwner = res.channelKey;
                chickChatGroupOwner = res.chickChatGroup;

                console.log(chickChatGroupOwner);

                jQuery('#login').hide();
                jQuery('#logedin').show();

                JoinChat();

                pingKeepAlive();

            } else {
                alert('error');
            }
        });
}

function PushMessage() {
    
    var txtMsgToPush = jQuery('#txtMsgToPush').val();

    if (txtMsgToPush == '') {
        alert('Name and message can not be empty');
        return;
    }

    $.ajax({
            method: "POST",
            url: "/pushmessage",
            cache: false,
            dataType: "json",
            data: { message: txtMsgToPush, channelKey: channelKeyChattingSelected, senderChannelKey: channelKeyOwner }
        })
        .done(function(res) {
           
        });

    jQuery('#txtMsgToPush').val('');
}


$(document).ready(function() {


    socket.on('connect', function() {
    });

    socket.on('message', function(message) {
        var xxx = '';
        for (var i = 0; i < message.length; i++) {
            if (!message[i].channelKey || message[i] == '') continue;

            xxx = xxx + '<a href="javascript:void(0)" onclick=\'JoinChat("' + message[i].channelKey + '");return;\'>'
                + message[i].anonymousName + ':' + message[i].anonymousTitle + "</a> , ";
        }
        jQuery('#groups').html(xxx);
    });

    socket.on('joinchat', function(message) {

        var xxx = '';

        if (message) {
            message.sort(function(a, b) {
                var adt = Date.parse(a.date);
                var bdt = Date.parse(b.date);
                return adt - bdt;
            });
        }

        for (var i = 0; i < message.length; i++) {
            if (!message[i].message || message[i].message == '') continue;
            xxx = xxx + BuildMessageDisplay(message[i]) + "<br>";
        }
        jQuery('#content').html(xxx);
    });

    socket.on('disconnect', function() {
        console.log('disconnected');
        jQuery('#groups').html("<b>Disconnected!</b>");
    });

    socket.connect();


});

function BuildMessageDisplay(msgobj) {
    var msg = msgobj.message;

    var indx = msg.indexOf(':');
    
    var ckey = msg.substring(0, indx);
    var namemsg = msg.substring(indx + 1, msg.length);
    indx = namemsg.indexOf(':');
    var name = namemsg.substring(0, indx );
    var msgcontent = namemsg.substring(indx + 1, msg.length);


    var m = '<b>' + name + '</b><i> said at ' + msgobj.date + "</i><br>";
    m = m + msgcontent;
    m= m.replace('\r\n', '<br>');
    m = m.replace('\n', '<br>');
    return m;
}

function JoinChat(channelkey) {
    channelKeyChattingSelected = channelkey;

    if (!channelKeyChattingSelected || channelKeyChattingSelected == '') {
        channelKeyChattingSelected = channelKeyOwner;
    }

    $.ajax({
            method: "POST",
            url: "/joinchat",
            cache: false,
            dataType: "json",
            data: { channelKey: channelKeyChattingSelected, senderChannelKey: channelKeyOwner }
        })
        .done(function (res) {
           
              var  tempGroup = res.chickChatGroup;
              jQuery('#currentTitle').html(
                     "<br>Chat as: <i>" + chickChatGroupOwner.anonymousName
                + ":" + chickChatGroupOwner.anonymousTitle
                + ":" + chickChatGroupOwner.channelKey + '</i>'
                +
                "<br>Join to: <i>"+  tempGroup.anonymousName
                + ":" + tempGroup.anonymousTitle
                + ":" + tempGroup.channelKey +'</i>');

            socket.emit("joinchat", { channelKey: channelKeyChattingSelected, senderChannelKey: channelKeyOwner });
            //socket.emit("joinchat", {query: "channelKey="+ channelKeyChattingSelected+"&senderChannelKey="+ channelKeyOwner });
  
           
        });

    jQuery('#logedin').show();
}

var _timeoutPingKeepAlive;
function pingKeepAlive() {
    if (!channelKeyOwner || channelKeyOwner == '') return;

   // console.log(channelKeyOwner);

    socket.emit("keepalive", { channelKey: channelKeyOwner, senderChannelKey: channelKeyOwner });

    _timeoutPingKeepAlive = setTimeout(function() {
        pingKeepAlive();
    }, 1000);
}


var validNavigation = false;

function endSession() {
   
    $.ajax({
        method: "POST",
        url: "/logout",
        cache: false,
        dataType: "json",
        data: { channelKey: channelKeyOwner, senderChannelKey: channelKeyOwner }
    })
        .done(function (res) {


        });
}

function wireUpEvents() {
    /*
    * For a list of events that triggers onbeforeunload on IE
    * check http://msdn.microsoft.com/en-us/library/ms536907(VS.85).aspx
    */
    window.onbeforeunload = function() {
        if (!validNavigation) {
            endSession();
        }
    }

    // Attach the event keypress to exclude the F5 refresh
    $(document).bind('keypress', function(e) {
        if (e.keyCode == 116){
            validNavigation = true;
        }
    });

   

    // Attach the event submit for all forms in the page
    $('form').bind('submit', function() {
        validNavigation = true;
    });

    // Attach the event click for all inputs in the page
    $('input[type=submit]').bind('click', function() {
        validNavigation = true;
    });

}

// Wire up the events as soon as the DOM tree is ready
$(document).ready(function() {
    wireUpEvents();
});