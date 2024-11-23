const path = require('path');

module.exports = {
    mode: "production",
    entry: {
      barcodescanner: path.resolve(__dirname, 'NpmTS', 'src', 'barcodescanner.ts')  
    },
    module: {
        rules: [
            {
                test: /\.ts$/,
                use: 'ts-loader',
                exclude: /node_modules/,
            },
        ],
    },
    resolve: {
        extensions: ['.ts', '.js'],
    },
    output: {
        filename: '[name]-bundled.js',
        path: path.resolve(__dirname, 'wwwroot', 'js'),
    },
};