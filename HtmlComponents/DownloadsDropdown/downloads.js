S.downloads = {
    dropdown: {
        show: function () {
            var dotsmenu = $('.download-btns .btn-dots .dropmenu');
            $(document.body).on('click', () => { dotsmenu.hide(); });
            dotsmenu.show();
        }
    },
    domainTypes: {
        showMatches: function () {
            S.ajax.post('Collector-Downloads/RenderDomainTypeMatches', {}, (response) => {
                //load list of domaintype matches that exist in the database
                S.popup.show('Domain Type Matches', response, { width: 600 });
            });
        }
    }
};