S.domain = {
    details: {
        domainId: null,

        show: function (domain) {
            S.ajax.post('CollectorDomains/Details', { domain: domain }, (response) => {
                S.domain.details.domainId = response.domainId;
                S.popup.show(response.title, response.popup, { width: 450 });
                $('.popup.show .tab-info').on('click', () => { S.domain.details.tab('info'); });
                $('.popup.show .tab-articles').on('click', S.domain.articles.show);
                $('.popup.show .tab-rules').on('click', S.domain.rules.show);
                $('.popup.show .tab-download').on('click', S.domain.download.show);
            }, () => { }, true);
        },

        tab: function (id) {
            $('.popup.show .tab').removeClass('selected');
            $('.popup.show .tab-' + id).addClass('selected');
            $('.popup.show .tab-content > div').hide();
            $('.popup.show .content-' + id).show();
        }
    },

    articles: {
        show: function () {
            S.domain.details.tab('articles');
        }
    },

    rules: {
        show: function () {
            S.domain.details.tab('rules');
            S.ajax.post('CollectorDomains/RenderAnalyzerRulesList', { domainId: S.domain.details.domainId }, (response) => {
                $('.popup.show .content-rules').html(response);
            });
        },

        add: {
            show: function () {
                S.popup.show("Create Analyzer Rule", temp_new_rule.innerHTML, {
                    width:450,
                    backButton: true
                });
            },

            submit: function () {
                var data = {
                    domainId: S.domain.details.domainId,
                    selector: rule_selector.value,
                    protect: rule_type.value == '1'
                };
                S.ajax.post('CollectorDomains/AddAnalyzerRule', data, (response) => {
                    S.popup.back();
                    $('.popup.show .content-rules').html(response);
                });
            }
        },

        remove: function (ruleId) {
            if (!confirm('Do you really want to remove this analyzer rule? This cannot be undone!')) { return;}
            S.ajax.post('CollectorDomains/RemoveAnalyzerRule', { ruleId: ruleId }, () => {
                $('.popup.show .content-rules .rule-' + ruleId).remove();
            });
        }
    },

    download: {
        show: function () {
            S.domain.details.tab('download');
            S.ajax.post('CollectorDomains/RenderDownloadRulesList', { domainId: S.domain.details.domainId }, (response) => {
                $('.popup.show .content-download').html(response);
            });
        },

        add: {
            show: function () {
                S.popup.show("Create Download Rule", temp_new_downloadrule.innerHTML, {
                    width: 450,
                    backButton: true
                });
            },

            submit: function () {
                var data = {
                    domainId: S.domain.details.domainId,
                    rule: download_rule.value == '1',
                    url: download_url.value,
                    title: download_title.value,
                    summary: download_summary.value,
                };
                S.ajax.post('CollectorDomains/AddDownloadRule', data, (response) => {
                    S.popup.back();
                    $('.popup.show .content-download').html(response);
                });
            }
        },

        remove: function (ruleId) {
            if (!confirm('Do you really want to remove this download rule? This cannot be undone!')) { return; }
            S.ajax.post('CollectorDomains/RemoveDownloadRule', { ruleId: ruleId }, () => {
                $('.popup.show .content-download .rule-' + ruleId).remove();
            });
        },

        cleanup: {
            show: function () {
                S.ajax.post('CollectorDomains/RenderCleanupDownloads', { domainId: S.domain.details.domainId }, (response) => {
                    S.popup.show("Clean Up Downloads", response, {
                        width: 450,
                        backButton: true
                    });
                    $('#do_cleanup').on('click', S.domain.download.cleanup.submit);
                });
            },

            submit: function () {
                if (!confirm('Do you really want to run this cleanup? This will permanently delete all affected articles, downloads, and associated files on disk.')) { return; }
                S.ajax.post('CollectorDomains/CleanupDownloads', { domainId: S.domain.details.domainId }, () => {
                    S.popup.back();

                });
            }
        }
    }
};