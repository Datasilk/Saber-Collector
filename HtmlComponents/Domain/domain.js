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
        }
    }
};