export const apiClient = {
  get,
  post,
  put,
  delete: _delete,
};

// main http verbs

function get(path) {
  return makeRequest(path, "GET");
}

function post(path, body) {
  return makeRequest(path, "POST", body);
}

function put(path, body) {
  return makeRequest(path, "PUT", body);
}

function _delete(path) {
  return makeRequest(path, "DELETE");
}

function makeRequest(path, method, body = null) {

  url = `https://localhost:7284/api/v1${path}`;

  const requestOptions = {
    method: method,
    headers: {
      "Content-Type": "application/json"
    },
    body: body ? JSON.stringify(body) : null,
  };

  return fetch(url, requestOptions).then(async (response) => {
    const responseText = await response.text();
    const responseObject = isJsonString(responseText) ? JSON.parse(responseText) : null;
    if (response.ok) {
      return Promise.resolve(responseObject);
    } else {
      const error = responseObject && responseObject.message 
          ? responseObject.message 
          : response.statusText;
        return Promise.reject(error);
    }
  });
}

function isJsonString(str) {
  try {
    JSON.parse(str);
  } catch (e) {
    return false;
  }
  return true;
}
