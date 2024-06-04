import http from 'k6/http';
import { check } from 'k6';

export const options = {
    VUs: 2,
    duration:'2s',
    thresholds: { 
        http_req_duration: ['p(50)<500', 'p(50)<1000'],
        http_req_failed:   ['rate<0.01'],
        http_req_connecting : ['p(95)<10']
    }
};

export default function () {
    const url = 'http://google.com';
    const BASE_URL = 'http://localhost:5236/';
    
    const res = http.post(`${BASE_URL}/shorten`, {
        url: url
    });
    
    check(res, { 'short url created': (r) => r.status === 301 || r.status === 307 });
}