S.downloads = {
    hub: null,
    id: null,
    running: false,
    checkingQueue: false,
    timerFeeds: null,

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
        S.downloads.hub.on('checkedfeeds', S.downloads.finishedCheckingFeeds);
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
        S.downloads.checkFeeds();
    },

    stop: function () {
        S.downloads.running = false;
        S.downloads.checkingQueue = false;
        clearTimeout(S.downloads.timerFeeds);
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
        setTimeout(() => {
            //remove iframe if page is inside an iframe
            var id = S.util.location.queryString('iframe', window.location.href);
            if (window.parent != null && typeof id == 'string') {
                var iframe = window.parent.document.getElementById('iframe_' + id);
                iframe.parentNode.parentNode.removeChild(iframe.parentNode);
            }
        }, 500);
    },

    checkFeeds: function () {
        var feedId = feedid ? parseInt(feedid.value) : 0;
        S.downloads.hub.invoke('CheckFeeds', feedId);
        S.downloads.timerFeeds = setTimeout(S.downloads.checkFeeds, 15 * 60 * 1001); // every 15 minutes
    },

    finishedCheckingFeeds: function () {
        S.downloads.running = true;
        if (S.downloads.checkingQueue == false) {
            S.downloads.checkingQueue = true;
            S.downloads.check();
        }
    },

    check: function () {
        if (S.downloads.running === false) { return; } 
        var id = S.math.rnd.guid(6);
        var feedId = feedid ? parseInt(feedid.value) : 0;
        var domain = download_domain ? download_domain.value : '';
        var sort = download_sort ? parseInt(download_sort.value) : 0;
        S.downloads.id = id;
        setTimeout(() => {
            S.downloads.hub.invoke('CheckQueue', id, feedId, domain, sort);
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

    checked: function (skipped, downloaded, article, links, words, important) {
        //update stats
        S.downloads.updateStat('downloads', downloaded || 0);
        S.downloads.updateStat('articles', article || 0);
        S.downloads.updateStat('links', links || 0);
        S.downloads.updateStat('words', words || 0);
        S.downloads.updateStat('important', important || 0);

        //check for new downloads
        if (S.downloads.running == true) {
            if (downloaded == 0 && skipped != 1) {
                S.downloads.update('get next URL in the queue after 10 seconds...');
                setTimeout(S.downloads.check, 10 * 1000); //wait 10 seconds
            } else {
                S.downloads.update('getting next URL in the queue...');
                S.downloads.check(); //check immediately
            }
        } else {
            S.downloads.update('stopped running...'); 
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
            if ($('.popup.show .blacklist').length > 0) {
                //reload list
                S.downloads.blacklist.show();
            }
        },

        show: function () {
            S.ajax.post('CollectorDownloads/RenderBlacklist', {}, (response) => {
                S.popup.show('Blacklist Domains', response, { width: 400 });
                $('.popup.show .blacklist .close-btn').on('click', (e) => {
                    var domain = $(e.target).parents('.row.hover').first().attr('data-id');
                    S.downloads.blacklist.remove(domain);
                })
            });
        },

        remove: function (domain) {
            S.ajax.post('CollectorDownloads/DeleteBlacklist', { domain: domain }, (response) => {
                $('.popup.show .row.hover[data-id="' + domain + '"]').remove();
            });
        }
    },

    articles: {
        visible: false,
        list: [],

        found: function (json) {
            if (S.downloads.articles.visible == false) { return; }
            var article = JSON.parse(json);
            if (article == null || article.wordcount == null || article.wordcount == 0) { return; }
            $('.articles-list').append(S.downloads.articles.render(article));
            $('.no-articles').remove();
            $('.btn-sort-articles').show();
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
            if (S.downloads.articles.visible == false) { return; }
            var list = $('.articles-list');
            list.html('');
            S.downloads.articles.list.sort((a, b) => a.score < b.score ? 1 : -1)
            .forEach(a => {
                list.append(S.downloads.articles.render(a));
            });
        }
    },

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

S.util.location = {
    queryString: function (key, url) {
        if (!url) url = window.location.href;
        key = key.replace(/[\[\]]/g, "\\$&");
        var regex = new RegExp("[?&]" + key + "(=([^&#]*)|&|#|$)"),
            results = regex.exec(url);
        if (!results) return null;
        if (!results[2]) return '';
        return decodeURIComponent(results[2].replace(/\+/g, " "));
    }
};

// init html component
S.downloads.init();