var gulp = require('gulp'),
    sevenBin = require('7zip-bin'),
    sevenZip = require('node-7z')

var app = 'Saber-Collector';
var release = '../../bin/Release/net6.0/';
var publish = '../../bin/';

function publishToPlatform(platform) {

    return gulp.src([
        //include custom resources
        //'websitesettings.html', 'websitesettings.js'
        //include all files from published folder
        release + platform + '/publish/*',
        //exclude unwanted dependencies
        '!' + release + platform + '/publish/Core.dll',
        '!' + release + platform + '/publish/Saber.Core.dll',
        '!' + release + platform + '/publish/Saber.Vendor.dll',
        '!' + release + platform + '/publish/*.deps.json'
    ]).pipe(gulp.dest(publish + '/' + platform + '/' + app, { overwrite: true }));
}

gulp.task('publish:win-x64', () => {
    return publishToPlatform('win-x64');
});

gulp.task('publish:linux-x64', () => {
    return publishToPlatform('linux-x64');
});

gulp.task('publish:all', () => {
    return publishToPlatform('win-x64');
    return publishToPlatform('linux-x64');
});

gulp.task('zip', () => {
    setTimeout(() => {
        //wait 500ms before creating zip to ensure no files are locked
        process.chdir(publish);
        sevenZip.add(app + '.7z', app, {
            $bin: sevenBin.path7za,
            recursive: true
        });
        process.chdir('../..');
    }, 500);
    return gulp.src('.');
});

gulp.task('publish', gulp.series('publish:win-x64', 'zip'));