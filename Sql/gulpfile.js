//includes
var gulp = require('gulp'),
    concat = require('gulp-concat-util')

/*
Generate Install.sql
*/

gulp.task('install', function () {
    return gulp.src(['**/Tables/*.sql', '**/Sequences/*.sql', '**/*.sql', '*.sql', '!install.sql', '!migrate-*.sql', '!uninstall.sql'], { base: '.' })
        .pipe(concat('install.sql', {
            sep:
                '\n/* ////////////////////////////////////////////////////////////////////////////////////// */\n\n' +
                'GO\n\n' +
                '/* ////////////////////////////////////////////////////////////////////////////////////// */\n\n'
        }))
        .pipe(gulp.dest('.', { overwrite: true }));
});