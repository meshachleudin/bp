import http from 'k6/http';
import { check, sleep } from 'k6';

// Small load: 5 users for 30 seconds
export const options = {
  vus: 5,
  duration: '30s',

  thresholds: {
    http_req_failed: ['rate<0.01'],     // < 1% requests fail
    http_req_duration: ['p(95)<500'],   // 95% under 500ms
  },
};

const BASE_URL = __ENV.BASE_URL || 'http://localhost:5000';

// Helper to build application/x-www-form-urlencoded body
function formBody(data) {
  return Object.keys(data)
    .map(k => `${encodeURIComponent(k)}=${encodeURIComponent(data[k])}`)
    .join('&');
}

export default function () {
  // 1) GET homepage (this also sets the antiforgery cookie for us)
  const resGet = http.get(`${BASE_URL}/`);

  check(resGet, {
    'GET / status is 200': r => r.status === 200,
  });

  // 👉 Extract the __RequestVerificationToken hidden field from the HTML
  const tokenMatch = resGet.body.match(
    /name="__RequestVerificationToken"[^>]*value="([^"]+)"/
  );

  const token = tokenMatch && tokenMatch[1];

  check(token, {
    '__RequestVerificationToken found': t => t !== null && t !== undefined,
  });

  // 2) Prepare some valid BP values (Systolic > Diastolic and within ranges)
  const scenarios = [
    { systolic: 110, diastolic: 70 },  // Ideal
    { systolic: 125, diastolic: 80 },  // PreHigh-ish but valid
    { systolic: 150, diastolic: 90 },  // High
  ];

  const sample = scenarios[Math.floor(Math.random() * scenarios.length)];

  // Field names must match your Razor form's asp-for bindings: BP.Systolic, BP.Diastolic
  const payload = formBody({
    '__RequestVerificationToken': token,
    'BP.Systolic': sample.systolic,
    'BP.Diastolic': sample.diastolic,
  });

  const params = {
    headers: {
      'Content-Type': 'application/x-www-form-urlencoded',
    },
  };

  // 3) POST the form
  const resPost = http.post(`${BASE_URL}/`, payload, params);

  check(resPost, {
    'POST / is 200 or 302': r => r.status === 200 || r.status === 302,
  });

  sleep(1);
}
