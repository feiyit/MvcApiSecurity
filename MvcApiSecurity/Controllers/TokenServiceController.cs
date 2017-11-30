
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using Common;
using Models;

namespace Controllers
{
    public class TokenServiceController : ApiController
    {
        /// <summary>
        /// 根据用户名获取token
        /// </summary>
        /// <param name="staffId"></param>
        /// <returns></returns>
        public HttpResponseMessage GetToken(string appkey)
        {
            ResultMsg resultMsg = null;

            //判断参数是否合法
            if (string.IsNullOrEmpty(appkey))
            {
                resultMsg = new ResultMsg();
                resultMsg.StatusCode = (int)StatusCodeEnum.ParameterError;
                resultMsg.Info = StatusCodeEnum.ParameterError.GetEnumText();
                resultMsg.Data = "";
                return HttpResponseExtension.toJson(Newtonsoft.Json.JsonConvert.SerializeObject(resultMsg));
            }

            //插入缓存
            Token token =(Token)HttpRuntime.Cache.Get(appkey);
            if (HttpRuntime.Cache.Get(appkey) == null)
            {
                token = new Token();
                token.StaffId = appkey;
                token.SignToken = Guid.NewGuid();
                token.ExpireTime = DateTime.Now.AddDays(1);
                HttpRuntime.Cache.Insert(token.StaffId.ToString(), token, null, token.ExpireTime, TimeSpan.Zero);
            }

            //返回token信息
            resultMsg =new ResultMsg();
            resultMsg.StatusCode = (int)StatusCodeEnum.Success;
            resultMsg.Info = "";
            resultMsg.Data = token;

            return HttpResponseExtension.toJson(Newtonsoft.Json.JsonConvert.SerializeObject(resultMsg).ToLower());
        }
    }
}
