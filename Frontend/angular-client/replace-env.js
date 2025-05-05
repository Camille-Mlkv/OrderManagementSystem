const fs = require('fs');
const path = require('path');


const indexPath = path.join(__dirname, 'dist/angular-client/browser/index.html'); 

const key = process.env.GOOGLE_MAPS_API_KEY;
if (!key) {
  console.error('GOOGLE_MAPS_API_KEY is not defined in the .env file.');
  process.exit(1);
}

fs.readFile(indexPath, 'utf8', (err, data) => {
  if (err) throw err;

  const result = data.replace('__GOOGLE_MAPS_API_KEY__', key);
  fs.writeFile(indexPath, result, 'utf8', (err) => {
    if (err) throw err;
    console.log('index.html updated with API key.');
  });
});
