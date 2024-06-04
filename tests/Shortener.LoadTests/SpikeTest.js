import http from 'k6/http';
import { check,sleep } from 'k6';

export const options = {
    thresholds: {
        http_req_duration: ['p(50)<500', 'p(50)<1000'],
        http_req_failed:   ['rate<0.01'],
        http_req_connecting : ['p(95)<10']
    },
    stages: [
        { duration: '10s', target: 1 },
        { duration: '10s', target: 100 },
        { duration: '10s', target: 0 },
        { duration: '10s', target: 200 },
        { duration: '10s', target: 0 }
    ]
};

export default function () {
    const url = 'http://google.com';
    const BASE_URL = 'http://localhost:5236/';

    const res = http.post(`${BASE_URL}/shorten`, {
        url: url
    });

    check(res, { 'short url created': (r) => r.status === 201 });
    
    sleep(0.2);
}