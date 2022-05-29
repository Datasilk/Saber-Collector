S.domains = {
    init: function () {
        //submit search form events
        $('#searchform').on('submit', (e) => {
            S.domains.search.getResults();
            e.preventDefault();
            return false;
        })
        $('#search, #subjectId').on('keypress', (e) => {
            switch (e.key.toLowerCase()) {
                case "enter":
                    S.domains.search.getResults();
            }
        });
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
                S.domains.search.selectedSubjectId = id;
            });
        },

        getResults: function (start, length) {
            var data = {
                subjectId: S.domains.search.selectedSubjectId,
                type: domaintype.value,
                sort: sort.value,
                search: search.value,
                start: start == null ? 1 : start,
                length: length == null ? 50 : length
            }
            console.log(data);
            S.ajax.post('CollectorDomains/Search', data, (response) => {
                $('.accordion.domains .contents').html(response);
            });

        },

    }
};

S.domains.init();