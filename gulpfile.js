var gulp = require('gulp'),
    zip = require('gulp-zip');


var app = 'Collector';
var release = '../../bin/win-x64/';
var publish = '../../bin/';
var root = '../../';
var version = "0.1";

gulp.task('publish', () => {
    //copy all Content folders
    gulp.src([root + 'Content/**',
        '!' + root + 'Content/Collector/**'
    ]).pipe(gulp.dest(release + 'Content'));

    //copy all wwwroot folders
    gulp.src([root + 'wwwroot/**',
        '!' + root + 'wwwroot/content/collector/**'
    ]).pipe(gulp.dest(release + 'wwwroot'));

    //zip contents of release
    return gulp.src([release
        //remove any unwanted files from release
        //'!' + release + 'web.config'
    ])
        .pipe(zip(app + '-' + version + '.zip'))
        .pipe(gulp.dest(publish));
});