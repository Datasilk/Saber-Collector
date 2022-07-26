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
                $('.domains-paging, .domains.accordion').remove();
                $('.website > .content').append(response);
            });

        },

        customResults: function (domainsearch, subjectId, type, domainsort) {
            var data = {
                subjectId: subjectId,
                type: type,
                sort: domainsort,
                search: domainsearch,
                start: 1,
                length: 200
            }

            domaintype.value = type;
            sort.value = domainsort;
            search.value = domainsearch;

            if (subjectId != null && subjectId > 0) {
                //update subjectIds
                S.ajax.post('CollectorSubjects/NavigateDropdown', {subjectId: subjectId, parent: true }, (response) => {
                    $('#subjectId').html(response);
                });
            }

            S.ajax.post('CollectorDomains/Search', data, (response) => {
                $('.domains-paging, .domains.accordion').remove();
                $('.website > .content').append(response);
            });

        },

    },

    collections: {
        show: function (force) {
            var container = $('.domains-collections');
            $(document.body).on('click', S.domains.collections.hide);
            if (force === true) {
                container.html('');
            }
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

        hide: function (e) {
            if (e) {
                var target = $(e.target);
                if (target.parents('.domains-collections').length > 0) { return; }
            }
            $('.domains-collections').hide();
            $(document.body).off('click', S.domains.collections.hide);
        },

        add: {
            show: function () {
                $('.btn-new-collection').hide();
                $('.new-collection').show();
                $('.new-category').hide();
                S.ajax.post("CollectorDomains/RenderGroups", {}, (response) => {
                    $('#col_groupid').html(response);
                });
            },

            hide: function () {
                $('.btn-new-collection').show();
                $('.new-collection').hide();
            },

            submit: function () {
                var data = {
                    colgroupId: col_groupid.value,
                    name: col_name.value,
                    search: search.value,
                    subjectId: S.domains.search.selectedSubjectId,
                    type: domaintype.value,
                    sort: sort.value
                }
                S.ajax.post('CollectorDomains/AddCollection', data, () => {
                    S.domains.collections.show(true);
                }, (err) => {
                    console.log(err);
                    S.message.show('.new-collection .messages', 'error', err.responseText);
                });
            }
        }
    },

    group: {
        add: {
            show: function () {
                $('.new-collection').hide();
                $('.new-category').show();
            },

            hide: function () {
                $('.new-category').hide();
                $('.new-collection').show();
            },

            submit: function () {
                S.ajax.post('CollectorDomains/AddGroup', {name:cat_name.value}, () => {
                    S.domains.collections.add.show();
                });
            }
        },

        toggle: function (e) {
            var target = $(e.target).parents('.group-row').first();
            target.toggleClass('expanded');
        }
    }
};

S.domains.init();