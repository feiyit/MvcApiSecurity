using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace Common
{
    public class SignExtension
    {
        public static bool Validate(string timeStamp, string nonce, string token,string data,string signature)
        {
            var hash = System.Security.Cryptography.MD5.Create();
            //拼接签名数据
            var signStr = token + timeStamp + nonce + data;
            var newSign=System.Web.Security.FormsAuthentication.HashPasswordForStoringInConfigFile(signStr, "Md5").ToLower();
            return newSign == signature;
        }
    }
}