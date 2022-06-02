var article = {
    hub: null,

    init: function () {
        $('.analyze-article .analyze > .button').on('click', article.analyze.start);

        //set up signalR hub
        article.hub = new signalR.HubConnectionBuilder().withUrl('/articlehub').build();
        article.hub.on('update', article.analyze.update);
        article.hub.on('append', article.analyze.append);
        article.hub.on('rawhtml', article.rawHtml.load);
        article.hub.on('words', article.words.load);
        article.hub.on('phrases', article.phrases.load);
        article.hub.start().catch(article.analyze.error);
        if ($('.analyzer.article-only').length > 0) {
            $('.analyzer .article-text .box').removeClass('box');
            setTimeout(article.analyze.start, 500);
        }
    },

    analyze: {
        start: function () {
            var url = util.location.queryString('url');
            if (url.indexOf('http') != 0 ||
                (url.indexOf('http://') != 0 && url.indexOf('https://') != 0)
            ) {
                article.message.error('Invalid URL');
                return;
            }
            $('.analyze-article .analyze').hide();
            $('.analyze-article > .box').css({ height: 400 });

            //send command via SignalR
            if (typeof articleOnly == 'undefined') {
                window.articleOnly = false;
            }
            article.hub.invoke('AnalyzeArticle', url, articleOnly === true);
        },

        update: function (code, msg) {
            //receive command from SignalR
            var box = $('.analyze-article > .box')[0];
            var div = document.createElement('div');
            div.className = 'cli-line';
            div.innerHTML = '> ' + msg;
            $('.analyze-article .console').append(div);
            box.scrollTop = box.scrollHeight;
        },

        append: function (html) {
            //receive command from SignalR
            $('.analyzer').append(html);
            accordion.load();
            if ($('.analyzer.article-only').length > 0) {
                $('.analyzer .article-text .box').removeClass('box');
            }

            //remove empty tags (temporary bug-fix)
            $('.analyzer').find('p').filter((i, a) => $(a).children().length == 0).remove();

            //find small images
            var win = S.window.pos();
            $('.analyzer img').on('load', (e) => {
                var a = $(e.target);
                var nextElem = $(a).parent().next();
                var text = nextElem != null ? article.getText(nextElem) : [];
                if (text == null) { text = []; }
                var small = false;

                if (win.w > 900 && 100 / win.w * $(a).width() < 40) {
                    $(a).parent().addClass('small-img');
                    small = true;
                } else {
                    $(a).parent().addClass('large-img');
                }
                //check for image description below image
                var alltext = text.join('');
                if (text.length <= 5 && alltext.length > 5 && alltext.length < 100) {
                    nextElem.addClass('img-description');
                    if (small) {
                        //move description into image div
                        $(a).parent().append(nextElem);
                    }
                }
            });
        },

        error: function (err) {

        }
    },

    rawHtml: {
        lines: [],
        scrollY: 0,
        length:100, 
        empty: {
            top: { hidden: 0, height: 0 },
            bottom: { hidden: 0, height: 0 }
        },
        displayed: [],
        injecting: false,
        mousedown:false,

        load: function (raw) {
            article.rawHtml.lines = raw;
            article.rawHtml.inject(0, article.rawHtml.length);
            $('.accordion.raw-html > .box').on('scroll', article.rawHtml.onscroll);
            $('.accordion.raw-html > .box').on('wheel', (e) => { article.rawHtml.onscroll(e, 3); });
            $('.accordion.raw-html > .box').on('mousedown', article.rawHtml.onmousedown);
            $('.accordion.raw-html > .box').on('mouseup', article.rawHtml.onmouseup);
        },

        inject: function (start, length, area) {
            var raw = article.rawHtml;
            if (raw.injecting || start > raw.lines.length || start < 0) { return; }
            article.rawHtml.injecting = true;
            var container = $('.accordion.raw-html .contents');
            var box = $('.accordion.raw-html > .box')[0];
            var emptyTop = container.find('.empty-top');
            var emptyBottom = container.find('.empty-bottom');
            var empty = raw.empty;
            var html = '';
            var h = 0;
            var lines = raw.lines;
            var end = start + length;
            if (end >= lines.length) { end = lines.length; }
            var skipped = [];

            //combine raw html lines
            var added = 0;
            for (var x = start; x < end; x++) {
                if (raw.displayed.indexOf(x) >= 0) {
                    skipped.push(x);
                    added++;
                } else {
                    html += lines[x];
                }
            }
            added = end - start - added; //invert total skipped into total added

            var children;
            if (area == 'top') {
                //prepend
                children = container.children();
                emptyTop.after(html);
            } else {
                //append
                emptyBottom.before(html);
                children = container.children();
            }

            //get height of appended raw HTML content
            for (var x = start; x < end; x++) {
                if (skipped.indexOf(x) >= 0) { continue; }
                var y = x - empty.top.hidden;
                h += $(children[y + 1]).height();
            }

            //get a list of raw HTML lines that are out of range of what's visible
            var deleted = [];
            var deletedElems = [];
            var skip = 1;
            if (area == 'top') {
                skip = added;
                children = container.children();
            }

            for (var x = 0; x < raw.displayed.length; x++) {
                var i = raw.displayed[x];
                if (i < start - raw.length || i >= end + raw.length) {
                    var elem = $(children[x + skip]);
                    if (i < start) {
                        empty.top.height += elem.height();
                        empty.top.hidden++;
                    } else {
                        empty.bottom.height += elem.height();
                        empty.bottom.hidden++;
                    }
                    deleted.push(i);
                    deletedElems.push(elem);
                }
            }

            //add items to displayed list
            var displayed = [];
            var u = 0;
            //add existing items to front of displayed list
            for (var x = 0; x < raw.displayed.length; x++) {
                if (raw.displayed[x] < start) {
                    if (deleted.indexOf(raw.displayed[x]) >= 0) { continue;}
                    displayed.push(raw.displayed[x]);
                } else {
                    break;
                }
            }
            //add new items to displayed list
            for (var x = start; x < end; x++) {
                displayed.push(x);
            }

            //add existing items to end of list
            for (var x = 0; x < raw.displayed.length; x++) {
                if (raw.displayed[x] < end || 
                    deleted.indexOf(raw.displayed[x]) >= 0) { continue; }
                displayed.push(raw.displayed[x]);
            }

            //update empty area height for top & bottom areas
            if (area == 'top') {
                empty.top.height = displayed[0] * 18;
                    empty.top.hidden = displayed[0];
            } else {
                empty.bottom.height = (lines.length - end) * 18; //18px height per line
                if (empty.bottom.height == 0 && empty.bottom.hidden == 0) {
                    empty.bottom.hidden = lines.length - end;
                } else {
                    empty.bottom.hidden = lines.length - displayed[displayed.length - 1] - 1;
                }
            }

            //delete all HTML elements that are out of scope
            for (var x = 0; x < deletedElems.length; x++) {
                var i = raw.displayed.indexOf(deleted[x]);
                deletedElems[x][0].parentNode.removeChild(deletedElems[x][0]);
            }

            //resize top & bottom empty areas
            if (empty.top.height < 0) { empty.top.height = 0; }

            emptyTop.css({ height: empty.top.height });
            emptyBottom.css({ height: empty.bottom.height });
            
            //update global vars
            article.rawHtml.displayed = displayed;
            article.rawHtml.empty = empty;
            article.rawHtml.injecting = false;
            
        },

        onmousedown: function() {
            article.rawHtml.mousedown = true;
        },

        onmouseup: function () {
            article.rawHtml.mousedown = false;
        },

        onscroll: function (e, c) {
            if (article.rawHtml.mousedown == false && c !== 3 && c !== true) { return;}
            var raw = article.rawHtml;
            //if (raw.injecting == true) { return; }
            var target = $('.raw-html .box')[0];
            var y = target.scrollTop;
            var y2 = raw.scrollY;
            var bottom = $('.raw-html .empty-bottom');
            var top = $('.raw-html .empty-top');
            if (y > y2) {
                //scrolling down
                if (target.scrollTop + target.clientHeight > bottom.position().top - raw.length) {
                    if (raw.displayed.length > 0 && raw.lines.length > raw.displayed[raw.displayed.length - 1]) {
                        var scrollBottom = $('.raw-html .contents').height() - target.scrollTop - target.clientHeight;
                        var length = raw.lines.length - raw.displayed[raw.displayed.length - 1] - (scrollBottom / 18);
                        if (length < raw.length) { length = raw.length; }
                        raw.inject(raw.displayed[raw.displayed.length - 1] + 1, length);
                    } else if (raw.displayed.length == 0) {
                        raw.inject(0, raw.length);
                    }
                }
            } else {
                //scrolling up
                if (target.scrollTop < top.position().top + top.height() + raw.length) {
                    if (raw.displayed[0] - raw.length < 0) {
                        if (raw.displayed[0] - raw.length > -raw.length) {
                            raw.inject(0, raw.length, 'top');
                        }
                    } else {
                        raw.inject(raw.displayed[0] - raw.length, raw.length, 'top');
                    }
                    
                }
            }
            article.rawHtml.scrollY = y;
            if (c !== true) {
                //execute a second time to ensure no empty space
                setTimeout(() => { article.rawHtml.onscroll(e, true) },100);
            }
        }
    },

    message: {
        error: function (msg) {
            article.message.error(msg.toString());
        }
    },

    menu: {
        toggle: function () {
            var menu = $('.article-menu');
            if (menu.css('display') == 'none') {
                menu.show();
                $(document.body).on('click', (e) => {
                    var target = $(e.target);
                    if (target.parents('.article-menu').length > 0) { return; }
                    article.menu.toggle();
                });
            } else {
                $(document.body).off('click');
                menu.hide();
            }
        }
    },

    delete: function (articleId) {
        if (!confirm('Do you really want to delete the cached HTML file for this article? This cannot be undone.')) { return false; }
        S.ajax.post('CollectorArticles/DeleteJsonCachedHtmlFile', { articleId: articleId }, () => {
            S.message.show('.analyzer .messages', '', 'Cached HTML file deleted for article successfully');
        });
    },

    getText: function (elem) {
        var textNodes = [];
        if (elem) {
            if (typeof elem.hasClass == 'function') {
                if (elem.length > 0) {
                    elem = elem[0];
                } else {
                    //no elements
                    return;
                }
            }
            for (var nodes = elem.childNodes, i = nodes.length; i--;) {
                var node = nodes[i], nodeType = node.nodeType;
                if (nodeType == 3) {
                    textNodes.push(node);
                }
                else if (nodeType == 1 || nodeType == 9 || nodeType == 11) {
                    textNodes = textNodes.concat(article.getText(node));
                }
            }
        }
        return textNodes;
    },

    words: {
        selected: [],

        load: function (html) {
            $('.website > .content').append(html);
            $('.words .word').on('click', article.words.toggle);
            S.accordion.load();
        },

        common: {
            add: function () {
                if (!confirm('Do you really want to convert these selected words into common words? They will no longer show up in your Article Analyzer > Words list')) { return;}
                S.ajax.post('CollectorAnalyzer/AddCommonWords', { words: article.words.selected }, () => {
                    //remove all selected words from list
                    $('.words .selected').remove();
                }, (err) => {
                    S.message.show('.words .messages', 'error', err.responseText);
                });
            }
        },
        toggle: function (e) {
            var target = $(e.target);
            if (!target.hasClass('word')) {
                target = target.parents('.word').first();
            }
            var word = target.html();
            if (target.hasClass('selected')) {
                //deselect
                target.removeClass('selected');
                article.words.selected.splice(article.words.selected.indexOf(word), 1);
            } else {
                //select
                target.addClass('selected');
                article.words.selected.push(word);
            }
            //toggle add phrase button
            if ($('.words .selected').length > 1) {
                $('.word-toolbar').find('.add-phrase').show();
            } else {
                $('.word-toolbar').find('.add-phrase').hide();
            }
            //toggle other buttons
            if ($('.words .selected').length > 0) {
                //show buttons
                $('.word-toolbar').find('.add-common-words, .deselect-all-words').show();
            } else {
                //hide buttons
                $('.word-toolbar').find('.add-common-words, .add-phrase, .deselect-all-words').hide();
            }
        },

        deselectAll: function () {
            $('.words .word.selected').removeClass('selected');
            article.words.selected = [];
                //hide buttons
            $('.word-toolbar').find('.add-common-words, .add-phrase, .deselect-all-words').hide();
        }
    },

    phrases: {
        selected: [],

        load: function (html) {
            $('.website > .content').append(html);
            $('.phrases .phrase').on('click', article.phrases.toggle);
            S.accordion.load();
        },

        add: function (phrase) {
            S.ajax.post('CollectorSubjects/AddWords', { subjectId: article.subjects.selectedSubjectId, words: [phrase] }, () => {
                S.message.show('.words .messages', '', 'Phrase "' + phrase + '" added to subject "' + article.subjects.selectedSubject + '"');
            });
        },

        toggle: function (e) {
            var target = $(e.target);
            if (!target.hasClass('phrase')) {
                target = target.parents('.phrase').first();
            }
            var phrase = target.html();
            if (target.hasClass('selected')) {
                //deselect
                target.removeClass('selected');
                article.phrases.selected.splice(article.phrases.selected.indexOf(phrase), 1);
            } else {
                //select
                target.addClass('selected');
                article.phrases.selected.push(phrase);
            }
            //toggle buttons
            if ($('.phrases .selected').length > 0) {
                //show buttons
                $('.phrase-toolbar').find('.add-to-subject').show();
            } else {
                //hide buttons
                $('.phrase-toolbar').find('.add-to-subject').hide();
            }
        }
    },

    subjects: {
        selectedSubjectId: null,
        selectedSubject: '',

        phrase: {
            add: function (phrase) {
                S.ajax.post("CollectorSubjects/AddWords", { subjectId: article.subjects.selectedSubjectId, words: [phrase] }, () => {
                    S.message.show('.words .messages', '', 'Phrase "' + phrase + '" added to subject "' + article.subjects.selectedSubject + '"');
                });
            }
        },

        words: {
            add: function () {
                S.ajax.post("CollectorSubjects/AddWords", { subjectId: article.subjects.selectedSubjectId, words: article.words.selected }, () => {
                    S.message.show('.words .messages', '', article.words.selected.length + ' words added to subject "' + article.subjects.selectedSubject + '"');
                });
            }
        },

        select: function () {
            var id = words_subjectId.value;
            var breadcrumb = $('.words .subjects-breadcrumb > span');
            var text = words_subjectId.options[words_subjectId.selectedIndex].text;
            var parent = false;
            if (text == "<< Parent Subject") {
                //go back
                var crumbs = breadcrumb.html().replace(/\&gt\;/g, '>').split(' > ');
                crumbs.pop();
                breadcrumb.html(crumbs.join(' > '));
                if (crumbs.length == 0) {
                    breadcrumb.addClass('hide');
                    $('.words .subjects-buttons').hide();
                }
            } else if (id == '0') {
                //go to root
                breadcrumb.addClass('hide').html('');
                $('.words .subjects-buttons').hide();
                parent = true;
            } else {
                //select subject
                breadcrumb.removeClass('hide');
                breadcrumb.html(breadcrumb.html() + (breadcrumb.html().trim().length > 0 ? ' > ' : '') + text);
                $('.words .subjects-buttons').show();
            }

            S.ajax.post('CollectorSubjects/NavigateDropdown', { subjectId: id, parent: parent }, (response) => {
                words_subjectId.innerHTML = response;
                article.subjects.selectedSubjectId = id;
                article.subjects.selectedSubject = text;
            });
        }
    }
    
};

article.init();