S.downloads = {
    hub: null,
    id: null,
    running: false,

    init: function () {
        $('.downloads .download > .button').on('click', S.downloads.start);
        $('.cancel-downloads button').on('click', S.downloads.stop);

        //set up signalR hub
        S.downloads.hub = new signalR.HubConnectionBuilder().withUrl('/downloadhub').build();
        S.downloads.hub.on('update', S.downloads.update);
        S.downloads.hub.on('checked', S.downloads.checked);
        S.downloads.hub.start().catch(S.downloads.error);
    },

    start: function () {
        S.downloads.running = true;
        $('.downloads .download').hide();
        $('.cancel-downloads').show();
        setTimeout(() => {
            S.downloads.check();
            S.downloads.checkFeeds();
        }, 10);
    },

    stop: function () {
        S.downloads.running = false;
        $('.downloads .download').show();
        $('.cancel-downloads').hide();
        setTimeout(() => {
            S.downloads.hub.invoke('StopQueue', S.downloads.id);
        }, 10);
    },

    check: function () {
        if (S.downloads.running === false) { return; }
        var id = S.math.rnd.guid(6);
        S.downloads.id = id;
        setTimeout(() => {
            S.downloads.hub.invoke('CheckQueue', id);
        });
    },

    update: function (msg) {
        //receive command from SignalR
        var div = document.createElement('div');
        div.className = 'cli-line';
        div.innerHTML = '> ' + msg;
        $('.downloads .console').append(div);
        //var box = $('.downloads .accordion > .box')[0];
        //box.scrollTop = box.scrollHeight;
    },

    checked: function (downloaded, article, links, words, important) {
        //update stats
        S.downloads.updateStat('downloads', downloaded || 0);
        S.downloads.updateStat('articles', article || 0);
        S.downloads.updateStat('links', links || 0);
        S.downloads.updateStat('words', words || 0);
        S.downloads.updateStat('important', important || 0);

        //check for new downloads
        if (S.downloads.running == true) {
            if (downloaded == 0) {
                setTimeout(S.downloads.check, 10 * 1000); //wait 10 seconds
            } else {
                S.downloads.check(); //check immediately
            }
        }
    },

    updateStat: function (stat, increment) {
        var elem = $('.accordion .stats .stat-' + stat + ' .number');
        var val = parseInt(elem.html()) + increment;
        elem.html(val);
    },

    checkFeeds: function () {
        S.downloads.hub.invoke('CheckFeeds');
        setTimeout(S.downloads.checkFeeds, 15 * 60 * 1001); // every 15 minutes
    },

    error: function (err) {
        S.downloads.update('An error occurred when communicating with the SignalR hub');
        console.log(err);
    },

    whitelist: {
        add: function (domain) {
            S.downloads.hub.invoke('Whitelist', domain);
        }
    },

    blacklist: {
        add: function (domain) {
            S.downloads.hub.invoke('Blacklist', domain);
        }
    }

};

//utility functions ///////////////////////////////////////

S.math.rnd = {
    guid: function (length, separator, groups) {
        let result = '', seeds;
        for (let i = 0; i < length; i++) {
            seeds = [
                Math.floor(Math.random() * 25) + 65,
                Math.floor(Math.random() * 25) + 97
            ];
            result += String.fromCharCode(seeds[Math.floor(Math.random() * 2)]);
        }
        console.log(result);
        if (separator && groups) {
            let e = 0;
            for (var x = 0; x < groups.length; x++) {
                if (groups[x] + e >= result.length) { break; }
                result = result.substr(0, groups[x] + e) + separator + result.substr(groups[x] + e);
                e += groups[x] + separator.length;
            }
        }
        return result;
    }
};

// init html component
S.downloads.init();