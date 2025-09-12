module.exports = {
    'client/**/*.{ts,js,html,css,scss,json}': [
      'npm --prefix client exec prettier -- --check',
    ],
    ignores: [
      'client/node_modules/**',
      'client/dist/**',
      'client/.angular/**',
      'client/coverage/**',
    ],
  };