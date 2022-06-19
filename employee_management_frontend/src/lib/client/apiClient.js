export const apiClient = {
  get,
  post,
  put,
  delete: _delete,
};

const baseUrl = import.meta.env.VITE_BACKEND_API_BASE_ADDRESS

// main http verbs

function get(path) {
  return makeRequest(path, "GET");
}

function post(path, body, isBodyJson = true) {
  return makeRequest(path, "POST", body, isBodyJson);
}

function put(path, body) {
  return makeRequest(path, "PUT", body);
}

function _delete(path) {
  return makeRequest(path, "DELETE");
}

function makeRequest(path, method, body = null, isBodyJson = true) {

  const url = `${baseUrl}${path}`;

  const requestOptions = {
    method: method,
    headers: { },
    body: body ? (isBodyJson ? JSON.stringify(body) : body) : null,
  };

  if (isBodyJson) {
    requestOptions.headers["Content-Type"] = "application/json"
  }

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
