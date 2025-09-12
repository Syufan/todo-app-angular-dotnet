module.exports = {
    'client/**/*.{ts,js,html,css,scss,json}': [
      'npm exec --prefix client prettier -- --write',
      'npm exec --prefix client eslint -- --fix'
    ],
    ignore: [
      'client/node_modules/**',
      'client/dist/**',
      'client/.angular/**',
      'client/coverage/**'
    ]
  }
