S.domains = {
    init: function () {
        //submit search form events
        $('#searchform').on('submit', (e) => {
            S.domains.search.getResults();
            e.preventDefault();
            return false;
        });
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
            
            S.ajax.post('CollectorSubjects/NavigateDropdown', { subjectId: id, parent: parent }, (response) => {
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
                length: length == null ? 200 : length
            }
            S.ajax.post('CollectorDomains/Search', data, (response) => {
                $('.website > .content').html(response);
            });

        },

    },

    collections: {
        show: function () {
            var container = $('.domains-collections');
            $(document.body).on('click', S.domains.collections.hide);

            if (container.html().trim() != '') {
                //collections already loaded, show dropdown list
                container.show();
                return;
            }
            S.ajax.post('Collector-Domains/RenderCollections', {}, (response) => {
                //load list of collections and show dropdown list
                container.html(response).show();
            });
        },

        hide: function () {
            $('.domains-collections').hide();
            $(document.body).off('click', S.domains.collections.hide);
        },

        add: {
            show: function () {

            },

            hide: function () {

            },

            submit: function() {

            }
        }
    },

    group: {
        add: {
            show: function () {

            },

            hide: function () {

            },

            submit: function () {
                
            }
        }
    }
};

S.domains.init();