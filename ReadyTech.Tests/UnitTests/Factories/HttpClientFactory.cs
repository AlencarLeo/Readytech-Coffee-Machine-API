using System.Net;

using Moq;
using Moq.Protected;

namespace UnitTestsmock;

public static class HttpClientFactory
{
    public static HttpClient CreateWithResponse(string jsonResponse, HttpStatusCode statusCode = HttpStatusCode.OK)
    {
        var handlerMock = new Mock<HttpMessageHandler>();

        handlerMock.Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>())
            .ReturnsAsync(new HttpResponseMessage
            {
                StatusCode = statusCode,
                Content = new StringContent(jsonResponse),
            });

        return new HttpClient(handlerMock.Object);
    }
}
