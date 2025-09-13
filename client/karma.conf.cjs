/** @type {import('karma').Config} */
module.exports = function (config) {
  config.set({
    frameworks: ['jasmine'],

    browsers: ['ChromeHeadless'],
    singleRun: true,

    reporters: ['progress', 'coverage'],

    coverageReporter: {
      dir: require('path').join(__dirname, 'coverage/client'),
      subdir: '.',
      reporters: [
        { type: 'html' },
        { type: 'lcovonly', file: 'lcov.info' },
        { type: 'text-summary' }
      ],
      fixWebpackSourcePaths: true
    },

    plugins: [
      require('karma-jasmine'),
      require('karma-chrome-launcher'),
      require('karma-coverage'),
      require('@angular-devkit/build-angular/plugins/karma')
    ]
  });
};