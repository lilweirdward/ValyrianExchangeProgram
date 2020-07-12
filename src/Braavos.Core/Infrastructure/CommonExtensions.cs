using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;

namespace Braavos.Core.Infrastructure
{
    public static class CommonExtensions
    {
        public static bool IsUnauthorizedStatusCode(this HttpResponseMessage responseMessage) => responseMessage.StatusCode == HttpStatusCode.Unauthorized;
    }
}
