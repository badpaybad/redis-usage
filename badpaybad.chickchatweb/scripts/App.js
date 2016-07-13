jQuery('#login').show();
jQuery('#logedin').hide();


var channelKeyOwner;
var channelKeyChattingSelected;
var chickChatGroupOwner;

var chatHub = $.connection.chatHubSignalR;

//console.log(chatHub);

function DoLogin() {
    var txtUid = jQuery('#txtAnonyName').val();
    var txtTitle = jQuery('#txtGroupTitle').val();
    var txtPwd = jQuery('#txtGroupPwd').val();
    if (!txtUid || !txtTitle || txtTitle == '' || txtUid == '') {
        alert('Name and title can not be empty');
        return;
    }
    var obj = { 'txtUid': txtUid, 'txtTitle': txtTitle, txtPwd: txtPwd };
    $.ajax({
            method: "POST",
            url: "/login",
            cache: false,
            dataType: "json",
            data: obj
        })
        .done(function(res) {
            if (res.IsOk) {
                channelKeyOwner = res.ChannelKey;
                chickChatGroupOwner = res.ChickChatGroup;

                //console.log(res);

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
    chatHub = $.connection.chatHubSignalR;
    
    chatHub.client.ListGroup = function(channelKey, list) {
        var message = list;
      //  console.log('group ' + list);
        var xxx = '';
        for (var i = 0; i < message.length; i++) {
            if (!message[i].ChannelKey || message[i].ChannelKey == '') continue;

            xxx = xxx + '<a href="javascript:void(0)" onclick=\'JoinChat("' + message[i].ChannelKey + '");return;\'>'
                + message[i].AnonymousName + ':' + message[i].AnonymousTitle + "</a> , ";
        }
        jQuery('#groups').html(xxx);
    }

    chatHub.client.JoinChat = function(channelKey, list) {
        var xxx = '';
        var message = list;
        if (message) {
            message.sort(function(a, b) {
                var adt = Date.parse(a.Date);
                var bdt = Date.parse(b.Date);
                return adt - bdt;
            });
        }

        for (var i = 0; i < message.length; i++) {
            if (!message[i].Message || message[i].Message == '') continue;
            xxx = xxx + BuildMessageDisplay(message[i]) + "<br>";
        }
        jQuery('#content').html(xxx);

        notifySound();
    }

    $.connection.hub.start().done(function() {

    });
});

function BuildMessageDisplay(msgobj) {
    var msg = msgobj.Message;

    var indx = msg.indexOf(':');

    var ckey = msg.substring(0, indx);
    var namemsg = msg.substring(indx + 1, msg.length);
    indx = namemsg.indexOf(':');
    var name = namemsg.substring(0, indx);
    var msgcontent = namemsg.substring(indx + 1, msg.length);


    var m = '<b>' + name + '</b><i> said at ' + msgobj.Date + "</i><br>";
    m = m + msgcontent;
    m = m.replace('\r\n', '<br>');
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
            data: { channelKey: channelKeyChattingSelected }
        })
        .done(function(res) {
            var channelKey = channelKeyChattingSelected;
            var tempGroup = res.ChickChatGroup;
            jQuery('#currentTitle').html(
                "<br>Chat as: <i>" + chickChatGroupOwner.AnonymousName
                + ":" + chickChatGroupOwner.AnonymousTitle
                + ":" + chickChatGroupOwner.ChannelKey + '</i>'
                +
                "<br>Join to: <i>" + tempGroup.AnonymousName
                + ":" + tempGroup.AnonymousTitle
                + ":" + tempGroup.ChannelKey + '</i>');

            jQuery('#content').html('');

            chatHub.server.joinChat(channelKey);
        });

    jQuery('#logedin').show();
}

var _timeoutPingKeepAlive;

function pingKeepAlive() {
    if (!channelKeyOwner || channelKeyOwner == '') return;

    chatHub.server.pingKeepAlive(channelKeyOwner);

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
        .done(function(res) {


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
        if (e.keyCode == 116) {
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



function notifySound() {
        var p = document.getElementById('audio');
        p.pause();
        p.currentTime = 0;
        p.play();
}

