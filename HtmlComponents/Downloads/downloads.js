S.downloads = {
    hub: null,

    init: function () {
        $('.downloads .download > .button').on('click', S.downloads.start);

        //set up signalR hub
        S.downloads.hub = new signalR.HubConnectionBuilder().withUrl('/downloadhub').build();
        S.downloads.hub.on('update', S.downloads.update);
        S.downloads.hub.on('checked', S.downloads.checked);
        S.downloads.hub.start().catch(S.downloads.error);
    },

    start: function () {
        $('.downloads .download').hide();
        S.downloads.check();
        S.downloads.checkFeeds();
    },

    check: function () {
        S.downloads.hub.invoke('CheckQueue');
    },

    update: function (msg) {
        //receive command from SignalR
        var box = $('.downloads .accordion > .box')[0];
        var div = document.createElement('div');
        div.className = 'cli-line';
        div.innerHTML = '> ' + msg;
        $('.downloads .console').append(div);
        //box.scrollTop = box.scrollHeight;
    },

    checked: function (article, links, words, important) {
        //update stats
        S.downloads.updateStat('downloads', 1);
        S.downloads.updateStat('articles', article);
        S.downloads.updateStat('links', links);
        S.downloads.updateStat('words', words);
        S.downloads.updateStat('important', important);

        //check for new downloads
        S.downloads.check();
    },

    updateStat: function(stat, increment) {
        var stat = $('.accordion .stats .' + stat + ' .number');
        var val = parseInt(stat.html) + increment;
        stat.html(val);
    },

    checkFeeds: function () {
        S.downloads.hub.invoke('CheckFeeds');
        setTimeout(S.downloads.checkFeeds, 1 * 60 * 60 * 1000); // hours * minutes * seconds * ms
    },

    error: function (err) {
        S.downloads.update('An error occurred when communicating with the SignalR hub');
        console.log(err);
    },

    blacklist: {
        add: function (domain) {
            S.downloads.hub.invoke('Blacklist', domain);
        }
    }

};

S.downloads.init();