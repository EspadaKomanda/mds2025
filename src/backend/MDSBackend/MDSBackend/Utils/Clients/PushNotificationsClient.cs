using System.Net;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using MDSBackend.Exceptions.UtilServices.Api;

namespace MDSBackend.Utils;

public class PushNotificationsClient
{
    #region Fields

    private readonly HttpClient _httpClient;
    private readonly ILogger<PushNotificationsClient> _logger;
    private readonly string _applicationToken;
    private readonly string _projectId;
    #endregion
    
    #region Constructor

    public PushNotificationsClient(HttpClient httpClient, ILogger<PushNotificationsClient> logger, string applicationToken, string projectId)
    {
        _httpClient = httpClient;
        _logger = logger;
        _applicationToken = applicationToken;
        _projectId = projectId;
    }
    
    #endregion
    
    #region Methods

    public async Task SendPushNotification(PushNotification pushNotification)
    {
        try
        {
            var payload = new
            {
                message = new
                {
                    token = _applicationToken,
                    notification = new
                    {
                        body = pushNotification.Message,
                        title = pushNotification.Title,
                        image = pushNotification.Image
                    },
                    android = new
                    {
                        notification = new
                        {
                            body = pushNotification.Message,
                            title = pushNotification.Title,
                            image = pushNotification.Image,
                            click_action = pushNotification.ClickAction,
                            click_action_type = pushNotification.ClickActionType
                        }
                    }
                }
            };

            var jsonPayload = JsonSerializer.Serialize(payload);

            var request = new HttpRequestMessage(HttpMethod.Post,$"/{_projectId}/messages")
            {
                Content = new StringContent(jsonPayload, Encoding.UTF8, "application/json")
            };

            var response = await _httpClient.SendAsync(request);

            if (response.StatusCode == HttpStatusCode.BadRequest)
            {
                var responseContent = await response.Content.ReadAsStringAsync();
                _logger.LogError($"Failed to send push notification. Status Code: {response.StatusCode}, Response: {responseContent}");
                throw new BadRequestException($"Failed to send push notification: {response.StatusCode}");
            }
            else if (response.StatusCode == HttpStatusCode.Forbidden)
            {
                var responseContent = await response.Content.ReadAsStringAsync();
                _logger.LogError($"Failed to send push notification: {response.StatusCode}, Response: {responseContent}");
                throw new ForbiddenException($"Failed to send push notification: {response.StatusCode}");
            }
            else
            {
                var responseContent = await response.Content.ReadAsStringAsync();
                _logger.LogError($"Failed to send push notification: {response.StatusCode}, Response: {responseContent}");
                throw new Exception($"Failed to send push notification: {response.StatusCode}");
            }
        }
        catch (Exception e)
        {
            _logger.LogError(e, e.Message);
            throw;
        }
    }

    public async Task SendPushNotification(PushNotification pushNotification, string topic)
    {
        try
        {
           var payload = new
            {
                message = new
                {
                    notification = new
                    {
                        body = pushNotification.Message,
                        title = pushNotification.Title,
                        image = pushNotification.Image
                    }
                }
            };

            var jsonPayload = JsonSerializer.Serialize(payload);

            var request = new HttpRequestMessage(HttpMethod.Post,$"/{_projectId}/topics/{topic}/publish")
            {
                Content = new StringContent(jsonPayload, Encoding.UTF8, "application/json")
            };

            var response = await _httpClient.SendAsync(request);

            if (response.StatusCode == HttpStatusCode.BadRequest)
            {
                var responseContent = await response.Content.ReadAsStringAsync();
                _logger.LogError($"Failed to send push notification. Status Code: {response.StatusCode}, Response: {responseContent}");
                throw new BadRequestException($"Failed to send push notification: {response.StatusCode}");
            }
            else if (response.StatusCode == HttpStatusCode.Forbidden)
            {
                var responseContent = await response.Content.ReadAsStringAsync();
                _logger.LogError($"Failed to send push notification: {response.StatusCode}, Response: {responseContent}");
                throw new ForbiddenException($"Failed to send push notification: {response.StatusCode}");
            }
            else
            {
                var responseContent = await response.Content.ReadAsStringAsync();
                _logger.LogError($"Failed to send push notification: {response.StatusCode}, Response: {responseContent}");
                throw new Exception($"Failed to send push notification: {response.StatusCode}");
            }
        }
        catch (Exception e)
        {
            _logger.LogError(e, e.Message);
            throw;
        }
    }
    
    #endregion
}