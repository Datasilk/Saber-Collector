S.downloads = {
    hub: null,
    id: null,
    running: false,

    console: {
        visible: false,

        show: function () {
            $('.btn-view-console').hide();
            $('.btn-hide-console').show();
            $('.section-downloads').removeClass('hide');
            S.downloads.console.visible = true;
        },

        hide: function () {
            $('.btn-view-console').show();
            $('.btn-hide-console').hide();
            $('.section-downloads').addClass('hide');
            S.downloads.console.visible = false;
        }
    },

    init: function () {
        $('.downloads .download > .button').on('click', S.downloads.start);
        $('.cancel-downloads button').on('click', S.downloads.stop);

        //set up signalR hub
        S.downloads.hub = new signalR.HubConnectionBuilder().withUrl('/downloadhub').build();
        S.downloads.hub.on('update', S.downloads.update);
        S.downloads.hub.on('checked', S.downloads.checked);
        S.downloads.hub.on('feed', S.downloads.checkedfeed);
        S.downloads.hub.on('article', S.downloads.articles.found);
        S.downloads.hub.start().catch(S.downloads.error);
    },

    start: function () {
        S.downloads.running = true;
        $('.downloads .download').hide();
        $('.cancel-downloads').show();
        $('.btn-download-start').addClass('hide');
        $('.btn-download-stop').removeClass('hide');
        $('.btn-view-console').hide();
        $('.btn-hide-console').hide();
        setTimeout(() => {
            S.downloads.check();
            S.downloads.checkFeeds();
        }, 10);
    },

    stop: function () {
        S.downloads.running = false;
        $('.downloads .download').show();
        $('.cancel-downloads').hide();
        $('.btn-download-start').removeClass('hide');
        $('.btn-download-stop').addClass('hide');
        if ($('.section-downloads').hasClass('hide')) {
            $('.btn-view-console').show();
            $('.btn-hide-console').hide();
        } else {
            $('.btn-view-console').hide();
            $('.btn-hide-console').show();
        }
        setTimeout(() => {
            S.downloads.hub.invoke('StopQueue', S.downloads.id);
        }, 10);
    },

    check: function () {
        if (S.downloads.running === false) { return; }
        var id = S.math.rnd.guid(6);
        var feedId = feedid ? parseInt(feedid.value) : 0;
        S.downloads.id = id;
        setTimeout(() => {
            S.downloads.hub.invoke('CheckQueue', id, feedId, S.downloads.console.visible);
        });
    },

    update: function (msg) {
        $('.stat-log').html(msg);
        if (S.downloads.console.visible == false) { return;}
        //receive command from SignalR
        var div = document.createElement('div');
        div.className = 'cli-line';
        div.innerHTML = '> ' + msg;
        $('.downloads .console').append(div);
        //var box = $('.downloads .accordion > .box')[0];
        //box.scrollTop = box.scrollHeight;
    },

    articles: {
        list: [],

        found: function (json) {
            var article = JSON.parse(json);
            if (article == null || article.wordcount == null || article.wordcount == 0) { return; }
            $('.articles-list').append(S.downloads.articles.render(article));
            $('.no-articles').remove();
            S.downloads.articles.list.push(article);
        },

        render: function (article) {
            return temp_article_item.innerHTML
                .replace('#url#', article.url)
                .replace('#url#', article.url)
                .replace('#article-url#', encodeURIComponent(article.url))
                .replace('#title#', article.title)
                .replace('#score#', article.score)
                .replace('#words#', article.wordcount)
                .replace('#links#', article.linkcount)
                .replace('#filesize#', article.filesize);
        },

        sort: function () {
            var list = $('.articles-list');
            list.html('');
            S.downloads.articles.list.sort((a, b) => a.score < b.score ? 1 : -1)
            .forEach(a => {
                list.append(S.downloads.articles.render(a));
            });
        }
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
                S.downloads.update('check for new downloads in 10 seconds...');
                setTimeout(S.downloads.check, 10 * 1000); //wait 10 seconds
            } else {
                S.downloads.update('check for new downloads immediately');
                S.downloads.check(); //check immediately
            }
        } else {
            S.downloads.update('S.downloads.running = false');
        }
    },

    checkedfeed: function (links, msg) {
        $('.stat-log').html(msg);
        S.downloads.updateStat('links', links || 0);
        if (S.downloads.console.visible == false) { return; }
        //receive command from SignalR
        var div = document.createElement('div');
        div.className = 'cli-line';
        div.innerHTML = '> ' + msg;
        $('.downloads .console').append(div);
        //var box = $('.downloads .accordion > .box')[0];
        //box.scrollTop = box.scrollHeight;
    },

    updateStat: function (stat, increment) {
        var elem = $('.accordion .stats .stat-' + stat + ' .number');
        var val = parseInt(elem.html()) + increment;
        elem.html(val);
    },

    checkFeeds: function () {
        var feedId = feedid ? parseInt(feedid.value) : 0;
        S.downloads.hub.invoke('CheckFeeds', feedId);
        setTimeout(S.downloads.checkFeeds, 15 * 60 * 1001); // every 15 minutes
    },

    error: function (err) {
        S.downloads.update('An error occurred when communicating with the SignalR hub');
        console.log(err);
    },

    whitelist: {
        add: function (domain) {
            S.downloads.hub.invoke('Whitelist', domain);
            if ($('.popup.show .whitelist').length > 0) {
                //reload list
                S.downloads.whitelist.show();
            }
        },

        show: function () {
            S.ajax.post('CollectorDownloads/RenderWhitelist', {}, (response) => {
                S.popup.show('Whitelist Domains', response, { width: 400 });
                $('.popup.show .whitelist .close-btn').on('click', (e) => {
                    var domain = $(e.target).parents('.row.hover').first().attr('data-id');
                    S.downloads.whitelist.remove(domain);
                })
            });
        },

        remove: function (domain) {
            S.ajax.post('CollectorDownloads/DeleteWhitelist', {domain:domain}, (response) => {
                $('.popup.show .row.hover[data-id="' + domain + '"]').remove();
            });
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