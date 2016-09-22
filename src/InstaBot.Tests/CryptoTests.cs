using System;
using System.Security.Cryptography;
using InstaBot.Console.Utils;
using NUnit.Framework;

namespace InstaBot.Tests
{
    [TestFixture]
    public class CryptoTests
    {
        [Test]
        public void Crypto_sha256_Token()
        {
            var signKey = "313402966dbce954860042c7d18f898a4290c833ced8c1913866fdb89d8a9562";
            var key = "08e36957fe025891daf5d49f42d3ff87ccfee05d9eeb87da8d9a7414e27f9d9f";
            var data = "{\"phone_id\":\"b0db1c60-71a1-46ed-90ff-9a6740416170\",\"_csrftoken\":\"Set-Cookie: csrftoken=20dKvBI37srBZg0arj0JtCxNIm4DQ0jO\",\"username\":\"login\",\"guid\":\"32d70991-81b0-46a5-8261-cb6ff4a5b792\",\"device_id\":\"android-e1700f4db4922ad5\",\"password\":\"password\",\"login_attempt_count\":\"0\"}";
            var sutKey = Crypto.CreateToken(data,signKey);
            Assert.AreEqual(key, sutKey);
        }
    }
}
