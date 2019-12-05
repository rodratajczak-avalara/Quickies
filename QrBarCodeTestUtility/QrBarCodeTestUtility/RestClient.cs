using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace QrBarCodeTestUtility
{
    /// <summary>
    /// Class for issuing HTTP requests to REST endpoints.
    /// </summary>
    public class RestClient : HttpClient
    {
        private readonly string _baseUrl;

        private ContentType? _contentType;
        private Token _token;
        private byte[] _body;
        private MultipartFormDataContent _content;
        private string _stringifiedBody;
        private readonly Dictionary<string, string> _customHeaders = new Dictionary<string, string>();

        public RestClient()
        {            
            DefaultRequestHeaders.Accept.Clear();
            DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            Timeout = TimeSpan.FromMinutes(30);
        }

        public RestClient(string baseUrl)
        {
            DefaultRequestHeaders.Accept.Clear();
            DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            Timeout = TimeSpan.FromMinutes(30);
            _baseUrl = baseUrl;
        }

        /// <summary>
        /// Sets the Content-Type header
        /// </summary>
        public RestClient WithContentType(ContentType contentType)
        {
            _contentType = contentType;
            return this;
        }

        /// <summary>
        /// Sets the <seealso cref="Token"/> to be used in the Authorization header
        /// </summary>
        public RestClient WithToken(Token token)
        {
            _token = token;
            if (_token != null)
            {
                DefaultRequestHeaders.Add("Authorization", _token.ToAuthorizationHeaderValue());
            }
            return this;
        }

        public RestClient WithHeader(string headerName, string value)
        {
            _customHeaders.Add(headerName, value);
            return this;
        }

        /// <summary>
        /// Sets the body of the request.
        /// </summary>
        public RestClient WithBody<T>(T body)
        {
            _stringifiedBody = typeof(T) == typeof(string) ? body.ToString() : JsonConvert.SerializeObject(body);
            _body = Encoding.UTF8.GetBytes(_stringifiedBody);
            return this;
        }

        /// <summary>
        /// Sets the body of the request.
        /// </summary>
        public RestClient WithBody(byte[] body)
        {
            _body = body;
            return this;
        }

        public RestClient WithContent(MultipartFormDataContent conent)
        {
            _content = conent;
            return this;
        }

        /// <summary>
        /// Issue an HTTP GET request to the specified endpoint
        /// </summary>
        public async Task<T> Get<T>(string query, params object[] args) where T : class
        {
            if (_body != null) throw new InvalidOperationException("Cannot specify a body with GET request");

            return await IssueRequestAsync<T>("GET", query, args);
        }

        /// <summary>
        /// Issue an HTTP POST request to the specified endpoint
        /// </summary>
        public async Task<T> Post<T>(string query, params object[] args) where T : class
        {
            return await IssueRequestAsync<T>("POST", query, args);
        }

        /// <summary>
        /// Issue an HTTP PUT request to the specified endpoint
        /// </summary>
        public async Task<T> Put<T>(string query, params object[] args) where T : class
        {
            return await IssueRequestAsync<T>("PUT", query, args);
        }

        /// <summary>
        /// Issue an HTTP DELETE request to the specified endpoint
        /// </summary>
        public async Task<T> Delete<T>(string query, params object[] args) where T : class
        {
            if (_body != null) throw new InvalidOperationException("Cannot specify a body with DELETE request");

            return await IssueRequestAsync<T>("DELETE", query, args);
        }

        private async Task<T> IssueRequestAsync<T>(string verb, string query, params object[] args) where T : class
        {
            if (typeof(T) == typeof(byte[]))
                return RequestAsync<byte[]>(BuildUrl(query, args), verb).Result as T;

            string response = await RequestAsync<string>(BuildUrl(query, args), verb);

            if (typeof(T) == typeof(string))
                return response as T;

            return JsonConvert.DeserializeObject<T>(response);
        }

        private async Task<T> RequestAsync<T>(string query, string method) where T : class
        {
            // Set up the request
            if (_customHeaders.Any())
            {
                foreach (var item in _customHeaders)
                {
                    DefaultRequestHeaders.Add(item.Key, item.Value);
                }

            }
            string fullUrl = BuildUrl(query);
            HttpResponseMessage responseMessage = new HttpResponseMessage();
            switch (method)
            {
                case "GET":
                    {
                        var responceTask = this.GetAsync(fullUrl);
                        responseMessage = responceTask.Result;
                        break;
                    }
                case "POST":
                    {
                        var responceTask = _content != null ? this.PostAsync(fullUrl, _content)
                             : this.PostAsync(fullUrl, new StringContent(_stringifiedBody, Encoding.UTF8, "application/json"));
                        responseMessage = responceTask.Result;
                        break;
                    }
                case "PUT":
                    {
                        var responceTask = this.PutAsync(fullUrl, new StringContent(_stringifiedBody, Encoding.UTF8, "application/json"));
                        responseMessage = responceTask.Result;
                        break;
                    }
                case "DELETE":
                    {
                        var responceTask = this.DeleteAsync(fullUrl);
                        responseMessage = responceTask.Result;
                        break;
                    }
            }

            string exceptionResponseStart = string.Format("Url: {0} | Method: {1}", fullUrl, method);

            if (responseMessage.StatusCode == HttpStatusCode.OK || responseMessage.StatusCode == HttpStatusCode.Created ||
                responseMessage.StatusCode == HttpStatusCode.Accepted)
            {

                if (_contentType.HasValue && responseMessage.Content.Headers.ContentType != null)
                {
                    responseMessage.Content.Headers.ContentType.MediaType = _contentType.ToHeaderValue();
                }

                // Issue request
                try
                {
                    var data = ReadResponseContent<T>(responseMessage).Result;
                    return data;
                }
                catch (HttpRequestException ex)
                {
                    string response = "No response message.";

                    if (ex.Data != null)
                    {
                        response = ex.Message;
                    }

                    string message = string.Format("{0} | Response: {1} | Exception: {2} | Body:{3}", exceptionResponseStart, response, ex, _stringifiedBody);

                    Exception outerException = new Exception(message, ex);


                    if (responseMessage.StatusCode == HttpStatusCode.ServiceUnavailable ||
                        responseMessage.StatusCode == HttpStatusCode.GatewayTimeout)
                    {
                        outerException = new TransientServiceException(message, ex);
                    }
                    else
                    {
                        outerException = new RestClientException(responseMessage.StatusCode, response, message, ex);
                    }
                    throw outerException;
                }
                catch (Exception ex)
                {
                    string message = string.Format("{0} | Exception: {1} | BODY: {2}", exceptionResponseStart, ex, _stringifiedBody);
                    Exception outerException = new Exception(message, ex);
                    throw outerException;
                }
            }
            else
            {
                string message = string.Format("{0} | Response: {1} | Body:{2}", exceptionResponseStart,
                    responseMessage, _stringifiedBody);
                throw new Exception(message);
            }

        }

        private static string BuildUrl(string query, params object[] args)
        {
            if (string.IsNullOrEmpty(query)) return null;

            object[] formattedArgs = args.Select(x => x.ToString()).Select(WebUtility.UrlEncode).ToArray();
            return string.Format(query, formattedArgs);
        }

        private string BuildUrl(string query)
        {
            if (string.IsNullOrEmpty(query)) return _baseUrl;

            string fullUrl = _baseUrl;
            if (!fullUrl.EndsWith("/"))
            {
                fullUrl += "/";
            }
            fullUrl += query;
            return fullUrl;
        }


        private async Task<T> ReadResponseContent<T>(HttpResponseMessage resp) where T : class
        {
            if (resp == null)
                return null;

            if (typeof(T) == typeof(string))
            {
                return resp.Content.ReadAsStringAsync().Result as T;
            }

            if (typeof(T) == typeof(byte[]))
            {
                var bytes = resp.Content.ReadAsByteArrayAsync().Result;
                return bytes as T;
            }

            throw new NotImplementedException("Unknown type: " + typeof(T).Name);
        }

        private static string Truncate(string input, int length)
        {
            if (string.IsNullOrEmpty(input) || input.Length <= length)
                return input;

            return input.Substring(0, length) + "...(truncated)";
        }
    }

    public class Token
    {
        public const string BearerIdentifier = "Bearer";
        public const string BasicAuthIdentifier = "Basic";

        public static Token CreateBasicAuthToken(string username, string password)
        {
            return new Token
            {
                EffectiveFrom = DateTime.MinValue,
                EffectiveTo = DateTime.MaxValue,
                Type = BasicAuthIdentifier,
                Value = Base64Encode(string.Format("{0}:{1}", username, password))
            };
        }

        public static Token CreateBearerToken(string token)
        {
            return new Token
            {
                Value = token,
                Type = BearerIdentifier
            };
        }

        public static Token CreateAccessKeyToken(string token)
        {
            return CreateBearerToken(token);
        }

        public string Value { get; set; }
        public string Type { get; set; }
        public DateTime EffectiveFrom { get; set; }
        public DateTime EffectiveTo { get; set; }

        public string ToAuthorizationHeaderValue()
        {
            return Type + " " + Value;
        }

        private static string Base64Encode(string input)
        {
            return Convert.ToBase64String(Encoding.UTF8.GetBytes(input));
        }
    }

    public enum ContentType
    {
        Text,
        Json,
        Pdf,
        Csv
    }

    public static class ContentTypeHelper
    {
        public static string ToHeaderValue(this ContentType? contentType)
        {
            return contentType.HasValue ? contentType.Value.ToHeaderValue() : "application/json";
        }

        public static string ToHeaderValue(this ContentType contentType)
        {
            switch (contentType)
            {
                case ContentType.Json: return "application/json";
                case ContentType.Pdf: return "application/pdf";
                case ContentType.Csv: return "text/csv";
                case ContentType.Text: return "text/plain";
                default:
                    throw new ArgumentOutOfRangeException("contentType", contentType, null);
            }
        }
    }

    public class TransientServiceException : Exception
    {
        public TransientServiceException() : base() { }
        public TransientServiceException(string message) : base(message) { }
        public TransientServiceException(string message, Exception innerException) : base(message, innerException) { }
    }

    public class RestClientException : Exception
    {
        public RestClientException(HttpStatusCode statusCode, string rawMessage, string message, Exception innerException)
            : base(message, innerException)
        {
            RawMessage = rawMessage;
            StatusCode = statusCode;
        }

        public string RawMessage { get; set; }
        public HttpStatusCode StatusCode { get; set; }
    }
}
