var gulp = require('gulp'),
    zip = require('gulp-zip');


var app = 'Collector';
var release = '../../bin/win-x64/';
var publish = '../../bin/';
var root = '../../';
var version = "0.1.41";

gulp.task('publish:content', () => {
    //copy all Content folders
    return gulp.src([root + 'Content/**',
        '!' + root + 'Content/Collector/**',
        '!' + root + 'Content/_Collector/**'
    ]).pipe(gulp.dest(release + 'Content'));
});

gulp.task('publish:wwwroot', () => {
    //copy all wwwroot folders
    return gulp.src([root + 'wwwroot/**',
        '!' + root + 'wwwroot/content/collector/**',
        '!' + root + 'wwwroot/content/_collector/**'
    ]).pipe(gulp.dest(release + 'wwwroot'));
});

gulp.task('publish:vendors', () => {
    //copy all wwwroot folders
    var vendor = 'Vendors/Collector/';
    return gulp.src([
        'config.json',
        'editor.js',
        'icon.svg',
        'README.md',
        'LICENSE',
        '**/*.html',
        '**/Profiles/*',
        '!**/node_modules/**'

    ]).pipe(gulp.dest(release + vendor));
});

gulp.task('publish:zip', () => {
    //zip contents of release
    return gulp.src([release + '**'
        //remove any unwanted files from release
        //'!' + release + 'web.config'
    ])
        .pipe(zip(app + '-' + version + '.zip'))
        .pipe(gulp.dest(publish));
});

gulp.task('publish', gulp.series('publish:content', 'publish:wwwroot', 'publish:vendors', 'publish:zip'));