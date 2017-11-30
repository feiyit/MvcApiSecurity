using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Formatting;
using System.Web.Http;
using Newtonsoft.Json.Serialization;
using Filter;

namespace MvcApiSecurity
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Web API 配置和服务
            GlobalConfiguration.Configuration.Formatters.XmlFormatter.SupportedMediaTypes.Clear();
            //默认返回 json  
            GlobalConfiguration.Configuration.Formatters.JsonFormatter.MediaTypeMappings.Add(
                new QueryStringMapping("datatype", "json", "application/json"));

            // 解决json序列化时的循环引用问题
            config.Formatters.JsonFormatter.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;

            //API过滤器
            config.Filters.Add(new ApiSecurityFilter());

            // Web API 路由
            config.MapHttpAttributeRoutes();
            //webapi全局设置：将json输出为首字母小写
            //config.Formatters.JsonFormatter.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );

            //Cors跨域框架
            //config.EnableCors();
        }
    }
}
