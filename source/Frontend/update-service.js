const https = require('https')

const data = JSON.stringify({
  todo: 'data'
})

const options = {
  hostname: 'portainer.mauricehessing.de',
  port: 443,
  path: '/api/webhooks/78855ef1-2f68-4b65-8661-df189a69be68',
  method: 'POST',
  headers: {
    'Content-Type': 'application/json',
    'Content-Length': data.length
  }
}

const req = https.request(options, res => {
  console.log(`statusCode: ${res.statusCode}`)

  res.on('data', d => {
    process.stdout.write(d)
  })
})

req.on('error', error => {
  console.error(error)
})

req.write(data)
req.end()