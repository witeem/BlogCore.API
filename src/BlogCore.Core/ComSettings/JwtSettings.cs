using System;
using System.Collections.Generic;
using System.Text;

namespace BlogCore.Core
{
    public class JwtSettings
    {
        public string Issuer { get; set; }

        public bool ValidateIssuer { get; set; }

        public string Audience { get; set; }

        public bool ValidateAudience { get; set; }

        public string RawSigningKey { get; set; }

        public bool ValidateIssuerSigningKey { get; set; }

        public bool ValidateLifetime { get; set; }

        public bool RequireExpirationTime { get; set; }

        public int JwtExpiresInMinutes { get; set; }

        public bool ValidateIntervaltime { get; set; }

        public int IntervalExpiresInMinutes { get; set; }
    }
}
