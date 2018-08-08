/// <binding />

var gulp = require('gulp');
var promise = require('es6-promise').polyfill();
var concat = require('gulp-concat');
var autoprefix = require('gulp-autoprefixer');
var minifyCSS = require('gulp-minify-css');
var rename = require('gulp-rename');
var stripDebug = require('gulp-strip-debug');
var uglify = require('gulp-uglify');
var useref = require('gulp-useref');
var gulpif = require('gulp-if');
var environments = require('gulp-environments');
var development = environments.development;
var production = environments.production;
var browserify = require("gulp-browserify");
var browserSync = require('browser-sync').create();
var size = require('gulp-size');
var bust = require('gulp-buster');
var gutil = require('gulp-util');

gulp.task('bundleIndexFile', function () {
    return gulp.src('index.dev.html')
      .pipe(useref())
        .pipe(gulpif(['../appbuild/*.js'], uglify()))
        .on('error', function (err) { gutil.log(gutil.colors.red('[Error]'), err.toString()); })
        .pipe(gulpif(['../appbuild/*.js'], stripDebug()))
        .pipe(gulpif(['../assets/global/css/*.css'], minifyCSS({ processImport: false })))
       
        .pipe(gulp.dest('dest'))
});

gulp.task("browserify-dev", function() {
    browserSync.init({
        server: {
            baseDir: "./",
            index: "index.dev.html"
        },
        port:5050
    });
   gulp.watch(['app/**/*']).on('change', browserSync.reload)
  });

  gulp.task("browserify-prod", function() {
    browserSync.init({
        server: {
            baseDir: "./",
            index: "index.html"
        },
        port:2050
    });
   
  });

const versionConfig = {
    'value': '%MDS%',
    'append': {
        'key': 'v',
        'to': ['css', 'js'],
    },
};
var version = require('gulp-version-number');
var gulpRename = require('gulp-rename');
gulp.task('renameIndex', ['bundleIndexFile'], function () {
    return gulp.src('dest/index.dev.html')
        .pipe(gulpRename('index.html'))
        .pipe(version(versionConfig))
        //.pipe(bust())
        .pipe(gulp.dest('.'));
});


// default gulp task
gulp.task('MinifyUglifyIndexFile', []
    , function () {
        environments.current(production);
        gulp.start('renameIndex');
    });



