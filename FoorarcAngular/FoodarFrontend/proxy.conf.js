const { env } = require('process');

const target = 'http://localhost:3000'

const PROXY_CONFIG = [
  {
    context: [
      "/api/",
   ],
    target: target,
    secure: false,
    headers: {
      Connection: 'Keep-Alive'
    }
  }
]

module.exports = PROXY_CONFIG;