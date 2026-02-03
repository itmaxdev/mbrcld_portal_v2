module.exports = {
  purge: ['./src/**/*.html', './src/**/*.ts'],
  theme: {
    extend: {
      colors: {
        blue: {
          'DEFAULT': '#0071BE',
          '50': '#77C8FF',
          '100': '#62BFFF',
          '200': '#39AFFF',
          '300': '#119EFF',
          '400': '#0089E7',
          '500': '#0071BE',
          '600': '#005086',
          '700': '#002E4E',
          '800': '#000D16',
          '900': '#000000',
        },
      },
    },
  },
  variants: {},
  plugins: [require('tailwindcss-rtl')],
}
