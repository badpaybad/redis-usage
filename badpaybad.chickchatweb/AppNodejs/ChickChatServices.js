//'user strick';

//var redisConnectionString = '//localhost:6379';
var redisConnectionString = '//badpaybad.info:6379';

var __redis = require("./redis");
var __socketio;
var __clientRedis = __redis.createClient(redisConnectionString);
var __socketCounter = 0;

module.exports = {
    boot: function(server) {

        __clientRedis.auth('badpaybad.info', function() {
            console.log('redis client authenticated');
        });
        __clientRedis.on("connect", function() {

        });

        __socketio = require('./socket.io').listen(server);

        __socketio.sockets.on('connection', function(socketRequest) {
            __socketCounter = __socketCounter + 1;
            var subscribeGroupChange = __redis.createClient(redisConnectionString);
            subscribeGroupChange.auth('badpaybad.info');

            subscribeGroupChange.subscribe('chickchat_changed');

            var subcriptChatting = __redis.createClient(redisConnectionString);
            subcriptChatting.auth('badpaybad.info');

            subscribeGroupChange.on("message", function(channel, message) {
                listGroupChickChatInternal(function (listGroup) {
                   // console.log(listGroup);
                    socketRequest.send(listGroup);
                });
            });

            socketRequest.on('message', function(msg) {
            });

            socketRequest.on('joinchat', function(msg) {

                var ckey = msg.channelKey;

                subcriptChatting.subscribe(ckey);

                subcriptChatting.on("message", function(channel, message) {

                    getMessageseChatInternal(ckey, function(msgs) {

                        socketRequest.emit("joinchat", msgs);
                    });
                });

                joinChatInternal(ckey);
            });

            socketRequest.on('keepalive', function(msg) {
                var channelKey = msg.channelKey;
                if (!channelKey || channelKey == '') return;

                var todayEnd = new Date().setSeconds(30);

                __clientRedis.expire(channelKey, 30, function (err, res) {

                });
                __clientRedis.expire(channelKey + "_msg", 30, function (err, res) {

                });

               // console.log("connected: " + key + " expire at " + todayEnd);
                socketRequest.emit("keepalive", { channelKey: channelKey, expireAt: todayEnd });
            });

            socketRequest.on('disconnect', function() {
                subscribeGroupChange.quit();
                subcriptChatting.quit();
            });
        });


        MockChange();
    },

    registerGroup: function(uid, title,pwd) {
        var channelKey = guid();
        __clientRedis.publish(channelKey, "chickchat_changed:" + new Date().toString());
        var requirePwd = (pwd && pwd != '');
        if (requirePwd) {
            pwd = hashCode(uid + pwd);
            __clientRedis.hset(channelKey, "haveAuth", requirePwd);
            __clientRedis.hset(channelKey, "pwd", pwd);
        }
        __clientRedis.hset(channelKey, "owner", uid);
        __clientRedis.hset(channelKey, "title", title);
        __clientRedis.hset("anonymous_list", channelKey, channelKey);

        publishMessageInternal(channelKey, channelKey, title + "\r\n------------");

        __clientRedis.publish("chickchat_changed", "chickchat_changed:" + new Date().toString() + channelKey);

        var todayEnd = new Date().setSeconds(30);

        __clientRedis.expire(channelKey, 30, function (err, res) {
            
        });
        __clientRedis.expire(channelKey + "_msg", 30, function (err, res) {
           
        });

        console.log(channelKey + " _ expire at _ " + todayEnd);

        return {
            channelKey: channelKey,
            chickChatGroup: {
                channelKey: channelKey,
                anonymousTitle: title,
                anonymousName:uid
            }
        };
    },

    publishMessage: function(channelKey, senderChannelKey, msg) {
        publishMessageInternal(channelKey, senderChannelKey, msg);
    },
    joinChat: function(channelKey, senderChannelKey) {
        joinChatInternal(channelKey);

    },
    getChickChat: function(channelKey, callBack) {
        getChickChatInternal(channelKey, callBack);
    },
    removeChat: function(channelKey) {
        removeChatInternal(channelKey);
    }
}

function joinChatInternal (channelKey) {
    setTimeout(function() {
        __clientRedis.publish(channelKey, channelKey + new Date().toString());

        __clientRedis.publish("chickchat_changed", "chickchat_changed:" + new Date().toString() + channelKey);

    }, 1000);

}

function publishMessageInternal(channelKey, senderChannelKey, msg) {
   
   getChickChatInternal(senderChannelKey, function(sender) {

        var tempMsg = sender.channelKey + ":" + sender.anonymousName + ":" + msg;
        __clientRedis.hset(channelKey + "_msg", new Date().toString(), tempMsg);
        __clientRedis.publish(channelKey,channelKey+ new Date().toString());

    });

}

function removeChatInternal(channelKey) {

    if (!channelKey || channelKey == '') return;

    __clientRedis.hdel(channelKey, "owner", function (e, r) { });
    __clientRedis.hdel(channelKey, "title", function (e, r) { });
    __clientRedis.hdel("anonymous_list", channelKey, function (e, r) { });
    __clientRedis.hdel(channelKey, function (e, r) { });
    __clientRedis.hkeys(channelKey + "_msg", function(err, all) {
        for (var i = 0; i < all.length; i++) {
            var a = all[i];
            __clientRedis.hdel(channelKey + "_msg", a, function (e, r) { });
        }
    });

    __clientRedis.del(channelKey + "_msg", function (e, r) { });

    __clientRedis.publish("chickchat_changed", new Date().toString());
};

function getChickChatInternal(channelKey, callBack) {
  
    __clientRedis.hgetall(channelKey, function (err, all) {
      
            var chickChat = {
                anonymousName: '',
                anonymousTitle: '',
                channelKey: '',
                isOnline: false
            };

            if (all != null) {
                chickChat.anonymousName = all.owner;
                chickChat.anonymousTitle = all.title;
                chickChat.channelKey = channelKey;
                chickChat.isOnline = true;

            }

            if (callBack) callBack(chickChat);
        
    });

}

function listGroupChickChatRescu(funcRes) {
    var temp = [];

    __clientRedis.hkeys("anonymous_list", function (err, all) {
        
            if (!all|| all.length == 0) {
                return funcRes(temp);
            }
            all.forEach(function(ck) {
                getChickChatInternal(ck, function(cc) {
                    temp.push(cc);
                    if (temp.length == all.length) {
                        funcRes(temp);
                    }
                });
            });
      
    });
}

function listGroupChickChatInternal(callBack) {
    listGroupChickChatRescu(function (res) {
       callBack(res);
   });
};

function getMessageseChatRescu(funcRes, channelKey) {
    var temp = [];
    __clientRedis.hgetall(channelKey + "_msg", function (err, all) {
       
            if (!all|| all.length == 0) {
                return funcRes(temp);
            }
            for (var i in all) {
                temp.push({
                    channelKey: channelKey,
                    date: i,
                    message: all[i]
                });
            }
            funcRes(temp);
       
    });
}

function getMessageseChatInternal(channelKey, callBack) {
    getMessageseChatRescu(function (res) {
        callBack(res);
    }, channelKey);
};

function MockChange() {
   
    __clientRedis.publish("chickchat_changed", "chickchat_changed:" + new Date().toString());

   // __clientRedis.set("xxx", "xxx");

   //__clientRedis.expire("xxx", 1,function (e,r){});

   // __clientRedis.get("xxx", function(e, r) {
   //     console.log(r);
   // });

    setTimeout(function() {
           MockChange();

    }, 2000);
}

function guid() {
    function s4() {
        return Math.floor((1 + Math.random()) * 0x10000)
            .toString(16)
            .substring(1);
    }

    return s4() + s4() + '-' + s4() + '-' + s4() + '-' +
        s4() + '-' + s4() + s4() + s4();
}

 function hashCode(source) {
    var hash = 0, i, chr, len;
    if (source.length === 0) return hash;
    for (i = 0, len = source.length; i < len; i++) {
        chr = source.charCodeAt(i);
        hash = ((hash << 5) - hash) + chr;
        hash |= 0; // Convert to 32bit integer
    }
    return hash;
};

//console.log('redis connected');
//setTimeout(function() {
//    clientRedis.publish("chickchat_changed", "chickchat_changed:" +new Date().toString());


//}, 1000);

//// Set a value
//clientRedis.set("string key", "Hello World", function(err, reply) {
//    console.log(reply+'_set');
//});
//// Get a value
//clientRedis.get("string key", function(err, reply) {
//    console.log(reply + '_get');
//});

//clientRedis.set("string key", "string val", redis.print);
//clientRedis.hset("hash key", "hashtest 1", "some value", redis.print);
//clientRedis.hset(["hash key", "hashtest 2", "some other value"], redis.print);
//clientRedis.hkeys("hash key", function(err, replies) {
//    console.log(replies.length + " replies:");
//    replies.forEach(function(reply, i) {
//        console.log("    " + i + ": " + reply);
//});
// clientRedis.quit();
//});