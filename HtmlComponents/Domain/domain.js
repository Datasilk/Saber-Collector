S.domain = {
    details: {
        domainId: null,
        domain: null,

        show: function (domain) {
            S.ajax.post('CollectorDomains/Details', { domain: domain }, (response) => {
                S.domain.details.domainId = response.domainId;
                S.domain.details.domain = domain;
                S.popup.show(response.title, response.popup, { width: 600 });
                $('.popup.show .tab-info').on('click', () => { S.domain.details.tab('info'); });
                $('.popup.show .tab-links').on('click', S.domain.links.show);
                $('.popup.show .tab-rules').on('click', S.domain.rules.show);
                $('.popup.show .tab-download').on('click', S.domain.download.show);
                $('.popup.show .tab-advanced').on('click', S.domain.advanced.show);
            }, () => { }, true);
        },

        tab: function (id) {
            $('.popup.show .tab').removeClass('selected');
            $('.popup.show .tab-' + id).addClass('selected');
            $('.popup.show .tab-content > div').hide();
            $('.popup.show .content-' + id).show();
        }
    },

    info: {
        requireSubscription: function () {
            var data = {
                domainId: S.domain.details.domainId,
                required: domain_subscription.checked == true
            }
            S.ajax.post('CollectorDomains/RequireSubscription', data, S.domain.updateListItem);
        },

        hasFreeContent: function () {
            var data = {
                domainId: S.domain.details.domainId,
                free: domain_freecontent.checked == true
            }
            S.ajax.post('CollectorDomains/HasFreeContent', data, S.domain.updateListItem);
        },

        isEmpty: function () {
            var data = {
                domainId: S.domain.details.domainId,
                isempty: domain_empty.checked == true
            }
            S.ajax.post('CollectorDomains/IsEmpty', data, S.domain.updateListItem);
        },

        updateType: function () {
            var data = {
                domainId: S.domain.details.domainId,
                type: domain_type.value
            }
            S.ajax.post('CollectorDomains/UpdateDomainType', data, S.domain.updateListItem);
        },

        updateType2: function () {
            var data = {
                domainId: S.domain.details.domainId,
                type: domain_type2.value
            }
            S.ajax.post('CollectorDomains/UpdateDomainType2', data, S.domain.updateListItem);
        },

        updateLang: function () {
            var data = {
                domainId: S.domain.details.domainId,
                lang: domain_lang.value
            } 
            S.ajax.post('CollectorDomains/UpdateLanguage', data, S.domain.updateListItem);
        }
    },

    updateListItem: function () {
        //update domain list item on domains HTML component list
        S.ajax.post('CollectorDomains/GetDomainListItem', { domainId: S.domain.details.domainId }, (response) => { 
            var elem = $('.domain-item.id-' + S.domain.details.domainId);
            if (elem.length > 0) {
                var div = document.createElement('div');
                div.innerHTML = response;
                elem.parent()[0].replaceChild(div.firstChild, elem[0]);
            }
        });
    },

    links: {
        show: function () {
            S.domain.details.tab('links');
            S.ajax.post('CollectorDomains/RenderLinks', { domainId: S.domain.details.domainId }, (response) => {
                $('.popup.show .content-links').html(response);
            });
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
                    S.domain.updateListItem();
                });
            }
        }
    },

    advanced: {
        show: function () {
            S.domain.details.tab('advanced');
            S.ajax.post('CollectorDomains/RenderAdvanced', { domainId: S.domain.details.domainId }, (response) => {
                $('.popup.show .content-advanced').html(response);
            });
        },

        deleteAllArticles: function () {
            if (!confirm('Do you really want to delete articles for this domain? This cannot be undone!')) { return; }
            S.ajax.post('CollectorDomains/DeleteAllArticles', { domainId: S.domain.details.domainId }, (response) => {
                S.message.show('.popup.show .messages', '', 'All articles were deleted for your domain');
                S.domain.updateListItem();
            });
        },

        getDomainTitle: function () {
            S.ajax.post('CollectorDomains/GetDomainTitle', { domainId: S.domain.details.domainId }, (title) => {
                S.message.show('.popup.show .messages', '', 'Found new domain title.');
                $('.popup.show .title h6').html(title);
                S.domain.updateListItem();
            });
        },

        getDescription: function () {
            S.ajax.post('CollectorDomains/GetDescription', { domainId: S.domain.details.domainId }, (description) => {
                S.message.show('.popup.show .messages', '', 'Found new domain description.');
                $('.popup.show .description p').html(description);
                S.domain.updateListItem();
            });
        },

        whitelist: {
            add: function () {
                if (!confirm('Do you really want to add this domain to your whitelist? Articles may begin downloading from this domain immediately afterwards.')) { return; }
                S.ajax.post('CollectorDomains/Whitelist', { domain: S.domain.details.domain }, (response) => {
                    S.message.show('.popup.show .messages', '', 'This domain is now whitelisted.');
                    S.domain.advanced.show();
                    S.domain.updateListItem();
                });
            },

            remove: function () {
                if (!confirm('Do you really want to remove this domain from your whitelist? You will no longer be able to download articles from this domain. All existing articles for this domain will be kept intact.')) { return; }
                S.ajax.post('CollectorDomains/RemoveWhitelist', { domain: S.domain.details.domain }, (response) => {
                    S.message.show('.popup.show .messages', '', 'This domain is now removed from the whitelist.');
                    S.domain.advanced.show();
                    S.domain.updateListItem();
                });
            }
        },

        blacklist: {
            add: function () {
                if (!confirm('Do you really want to add this domain to your blacklist? All articles & downloads will be immediately deleted along with any information about this domain and no other downloads will be queued for this domain.')) { return; }
                S.ajax.post('CollectorDomains/Blacklist', { domain: S.domain.details.domain }, (response) => {
                    S.message.show('.popup.show .messages', '', 'This domain is now blacklisted.');
                    S.popup.hide();
                    $('.domain-item.id-' + S.domain.details.domainId).remove();
                });
            },

            remove: function () {
                if (!confirm('Do you really want to remove this domain from your blacklist? Any new links found for this domain will be added to the download queue.')) { return; }
                S.ajax.post('CollectorDomains/RemoveBlacklist', { domain: S.domain.details.domain }, (response) => {
                    S.message.show('.popup.show .messages', '', 'This domain is now removed from the blacklist');
                    S.domain.advanced.show();
                });
            }
        }
    }
};