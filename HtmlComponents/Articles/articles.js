S.articles = {
    init: function () {
        $('#btnaddarticle').on('click', S.articles.add.show);
        $('#searchform').on('submit', (e) => {
            S.articles.search.getResults();
            e.preventDefault();
            return false;
        })
    },

    add: {
        show: function () {
            S.popup.show("Add Article", $('#template_addarticle').html());
            $('.popup form').on('submit', S.articles.add.submit);
            $('.popup form .button.cancel').on('click', S.articles.add.hide);
        },

        hide: function () {
            S.popup.hide();
        },

        submit: function (e) {
            e.preventDefault();
            var url = $('#newarticle_url').val();
            if (url == '' || url == null) {
                alert('URL cannot be empty');
                return false;
            }
            window.location.href = "/article?url=" + encodeURIComponent(url);
            return false;
        }
    },

    search: {
        selectedSubjectId: 0,

        selectSubject: function () {
            var id = subjectId.value;
            var breadcrumb = $('.subjects-breadcrumb > span');
            var text = subjectId.options[subjectId.selectedIndex].text;
            var parent = false;
            if (text == "<< Parent Subject") {
                //go back
                var crumbs = breadcrumb.html().replace(/\&gt\;/g, '>').split(' > ');
                crumbs.pop();
                breadcrumb.html(crumbs.join(' > '));
                if (crumbs.length == 0) {
                    breadcrumb.addClass('hide');
                }
            } else if (id == '0') {
                //go to root
                breadcrumb.addClass('hide').html('');
                parent = true;
            } else {
                //select subject
                breadcrumb.removeClass('hide');
                breadcrumb.html(breadcrumb.html() + (breadcrumb.html().trim().length > 0 ? ' > ' : '') + text);
            }
            
            S.ajax.post('CollectorSubjects/NavigateDropdown', { subjectId: id, parent: parent, score: parseInt(score.value) }, (response) => {
                subjectId.innerHTML = response;
                S.articles.search.selectedSubjectId = id;
            });
        },

        getResults: function (start, length) {
            var data = {
                subjectId: S.articles.search.selectedSubjectId,
                feedId: feedId.value,
                score: parseInt(score.value),
                search: search.value,
                start: start == null ? 1 : start,
                length: length == null ? 50 : length
            }
            console.log(data);
            S.ajax.post('CollectorArticles/Search', data, (response) => {
                $('.articles .accordion .contents').html(response);
            });

        },

    }
};

S.articles.init();