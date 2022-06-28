module.exports = {
    env: {
        browser: true,
        es2021: true,
    },
    extends: [
        'airbnb',
        'airbnb/hooks',
        'plugin:react/recommended', // Uses the recommended rules from @eslint-plugin-react
        'plugin:@typescript-eslint/recommended', // Uses the recommended rules from the @typescript-eslint/eslint-plugin
        'plugin:prettier/recommended', // Enables eslint-plugin-prettier and eslint-config-prettier. This will display prettier errors as ESLint errors. Make sure this is always the last configuration in the extends array.
    ],
    parser: '@typescript-eslint/parser', // Specifies the ESLint parser
    parserOptions: {
        ecmaFeatures: {
            jsx: true, // Allows for the parsing of JSX
        },
        ecmaVersion: 'latest', // Allows for the parsing of modern ECMAScript features
        sourceType: 'module', // Allows for the use of imports
    },
    settings: {
        react: {
            version: 'detect', // Tells eslint-plugin-react to automatically detect the version of React to use
        },
        'import/resolver': {
            node: {
                extensions: ['.js', '.jsx', '.ts', '.tsx'],
            },
        },
    },
    plugins: [
        'react', //
        '@typescript-eslint',
        'prettier',
    ],
    rules: {
        'react/jsx-filename-extension': [1, { extensions: ['.tsx', '.jsx'] }],
        'import/extensions': [
            'error',
            'ignorePackages',
            {
                js: 'never',
                jsx: 'never',
                ts: 'never',
                tsx: 'never',
                mjs: 'never',
            },
        ],
        'react/react-in-jsx-scope': 'off',
        'no-unused-expressions': ['error', { allowTernary: true }],
        'no-use-before-define': 'off',
        'react/require-default-props': ['error', { ignoreFunctionalComponents: true }],
        'no-param-reassign': ['error', { props: false }],
        '@typescript-eslint/no-use-before-define': ['error'],
        '@typescript-eslint/no-explicit-any': 'error',
        '@typescript-eslint/explicit-module-boundary-types': 'error',
    },
};
