var svgstore = require('./index')
var gulp = require('gulp')
var cheerio = require('gulp-cheerio')
var replace = require('gulp-replace');


gulp.task('svg', function () {

  return gulp
    .src('../Icons/*.svg')
    .pipe(cheerio({
      run: function ($) {
        $('[fill="none"]').removeAttr('fill')
      },
      parserOptions: { xmlMode: true }
    }))
      .pipe(svgstore({ filename: 'icons.svg' }))
    .pipe(replace('svg"><defs>', 'svg">\n\n\n' +
 //   '<style type="text/css">\n' +
 //   '    .editor path:not(.svg-nocolor){fill:currentColor}\n' +
 //   '    .editor use:not(.svg-nocolor):visited{color:currentColor}\n' +
 //   '    .editor use:not(.svg-nocolor):hover{color:currentColor}\n' +
 //   '    .editor use:not(.svg-nocolor):active{color:currentColor}\n' +
 //   '    .for-popup path:not(.svg-nocolor){fill:currentColor}\n' +
 //   '    .for-popup use:not(.svg-nocolor):visited{color:currentColor}\n' +
 //   '    .for-popup use:not(.svg-nocolor):hover{color:currentColor}\n' +
 //   '    .for-popup use:not(.svg-nocolor):active{color:currentColor}\n' +
 //   '</style>\n\n\n' + 
    '<defs>'))
    .pipe(replace(' fill="#FFFFFF"', ''))
    .pipe(replace(' fill="#ffffff"', ''))
	.pipe(gulp.dest('../'));
});