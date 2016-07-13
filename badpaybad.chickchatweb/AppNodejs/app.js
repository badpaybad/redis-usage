//var __rootDir = "D:\\badpaybad\\web\\chickchat.badpaybad.info\\chickchatweb\\AppNodejs\\";
var __rootDir = "D:/work/badpaybad.redis.sample/badpaybad.chickchatweb/AppNodejs/";
var __express = require('./express')();
var __server = require('http').createServer(__express);

var __chickChatServices = require('./ChickChatServices');

__chickChatServices.boot(__server);

var __bodyParser = require('./body-parser');
__express.use(__bodyParser.json());
__express.use(__bodyParser.urlencoded({    extended: true}));

__express.use(function (req, res, next) {
    res.header("Access-Control-Allow-Origin", "*");
    res.header("Access-Control-Allow-Headers", "Origin, X-Requested-With, Content-Type, Accept");
    next();
});

__server.listen(9000);

__express.get('/scripts/ChickChatClient.js', function (req, res, next) {
    res.sendFile("/template/scripts/ChickChatClient.js",
  { root: __rootDir });
});

__express.get('/css/app.css', function (req, res, next) {
    res.sendFile("/template/css/app.css",
  { root: __rootDir });
});


__express.get('/', function (req, res, next) {
    res.sendFile("/template/chickchat.html",
    { root: __rootDir });
});

__express.post('/pushmessage', function (req, res, next) {
    console.log(JSON.stringify( req.body));
    __chickChatServices.publishMessage
        (req.body.channelKey, req.body.senderChannelKey, req.body.message);

    res.send({ isOk: true });
});

__express.post('/login', function(req, res, next) {
    setTimeout(function() {

        var ccg = __chickChatServices.registerGroup(req.body.txtUid, req.body.txtTitle);
        
        if (ccg && ccg.channelKey && ccg.channelKey != '') {
            res.send({
                isOk: true,
                channelKey: ccg.channelKey,
                chickChatGroup: ccg.chickChatGroup
            });
        } else {
            res.send({ isOk: false });
        }

    }, 1000);
});

__express.post('/joinchat', function (req, res, next) {
    var channelKey = req.body.channelKey;
    var senderChannelKey = req.body.senderChannelKey;

    __chickChatServices.joinChat(channelKey, senderChannelKey);

    __chickChatServices.getChickChat(channelKey, function (ccg) {
        console.log(ccg);

        res.send({ isOk: true, chickChatGroup: ccg });
    });


});

__express.post('/logout', function (req, res, next) {
    var channelKey = req.body.channelKey;
    var senderChannelKey = req.body.senderChannelKey;
    __chickChatServices.removeChat(channelKey);
    res.send({ isOk: true});
});

