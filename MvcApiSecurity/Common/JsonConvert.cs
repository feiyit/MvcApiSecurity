using System;
using System.Data;
using System.Text;
using System.Threading.Tasks;
using System.Web.Script.Serialization;

namespace Common
{
    /// <summary>
    /// Describe：Json帮助类
    /// Author：feiyit
    /// Date：2016/10/10
    /// Blogs:http://www.feiyit.com
    /// </summary>
    public class JsonConvert
    {
        #region Object 对象转换

        /// <summary>
        /// Object对象转动态类
        /// </summary>
        /// <param name="obj">Object对象</param>
        /// <returns></returns>
        public static dynamic JsonClass(object obj)
        {
            return Newtonsoft.Json.JsonConvert.DeserializeObject(Serialize(obj, true), typeof(object)) as dynamic;
        }
        /// <summary>
        /// Object对象转动态类（异步方式）
        /// </summary>
        /// <param name="obj">Object对象</param>
        /// <returns></returns>
        public static async Task<dynamic> JsonClassAsync(object obj)
        {
            dynamic x = await Task.Factory.StartNew(() => Newtonsoft.Json.JsonConvert.DeserializeObject(Serialize(obj, true))) as dynamic;
            return x;
        }

        /// <summary>
        /// Object 转 Json 包
        /// </summary>
        /// <param name="obj">Object对象</param>
        /// <param name="dateConvert">是否格式化日期（默认不格式化）</param>
        /// <returns></returns>
        public static string Serialize(object obj, bool dateConvert = false)
        {
            var str = Newtonsoft.Json.JsonConvert.SerializeObject(obj);
            if (dateConvert)
            {
                str = System.Text.RegularExpressions.Regex.Replace(str, @"\\/Date\((\d+)\)\\/", match =>
                {
                    DateTime dt = new DateTime(1970, 1, 1);
                    dt = dt.AddMilliseconds(long.Parse(match.Groups[1].Value));
                    dt = dt.ToLocalTime();
                    return dt.ToString("yyyy-MM-dd HH:mm:ss");
                });
            }
            return str;
        }
        /// <summary>
        /// Object 转 Json 包（异步方式）
        /// </summary>
        /// <param name="obj">Object对象</param>
        /// <param name="dateConvert">是否格式化日期（默认不格式化）</param>
        /// <returns></returns>
        public static async Task<string> SerializeAsync(object obj, bool dateConvert = false)
        {
            var str = Newtonsoft.Json.JsonConvert.SerializeObject(obj);
            if (dateConvert)
            {
                str = System.Text.RegularExpressions.Regex.Replace(str, @"\\/Date\((\d+)\)\\/", match =>
                {
                    DateTime dt = new DateTime(1970, 1, 1);
                    dt = dt.AddMilliseconds(long.Parse(match.Groups[1].Value));
                    dt = dt.ToLocalTime();
                    return dt.ToString("yyyy-MM-dd HH:mm:ss");
                });
            }
            return await Task.Run(() => str);
        }

        #endregion

        #region Json 转换

        /// <summary>
        /// Json 转 Dynamic 动态类
        /// </summary>
        /// <param name="json">json包</param>
        /// <returns></returns>
        public static dynamic ConvertJson(string json)
        {
            return Newtonsoft.Json.JsonConvert.DeserializeObject(json, typeof(object)) as dynamic;
        }
        /// <summary>
        /// Json 转 Dynamic 动态类（异步方式）
        /// </summary>
        /// <param name="json">json包</param>
        /// <returns></returns>
        public static async Task<dynamic> ConvertJsonAsync(string json)
        {
            dynamic x = await Task.Factory.StartNew(() => Newtonsoft.Json.JsonConvert.DeserializeObject(json, typeof(object))) as dynamic;
            return x;
        }

        /// <summary>
        /// Json 转 实体类
        /// </summary>
        /// <typeparam name="T">实体类</typeparam>
        /// <param name="json">json包</param>
        /// <returns></returns>
        public static T Deserialize<T>(string json)
        {
            return Newtonsoft.Json.JsonConvert.DeserializeObject<T>(json);
        }

        /// <summary>
        /// Json 转 实体类
        /// </summary>
        /// <typeparam name="T">实体类</typeparam>
        /// <param name="json">json包</param>
        /// <returns></returns>
        public static async Task<T> DeserializeAsync<T>(string json)
        {
            return await Task.Run(() => Newtonsoft.Json.JsonConvert.DeserializeObject<T>(json));
        }

        #endregion

        #region DataReader  DataTable 转 Json

        /// <summary>
        /// DataReader 转 Json
        /// </summary>
        /// <param name="dataReader">IDataReader对象</param>
        /// <returns></returns>
        public static string ToJson(IDataReader dataReader)
        {
            return Newtonsoft.Json.JsonConvert.SerializeObject(dataReader);

        }
        /// <summary>
        /// DataReader 转 Json（异步方式）
        /// </summary>
        /// <param name="dataReader">IDataReader对象</param>
        /// <returns></returns>
        public static async Task<string> ToJsonAsync(IDataReader dataReader)
        {
            return await Task.Run(() => Newtonsoft.Json.JsonConvert.SerializeObject(dataReader));
        }

        /// <summary>  
        /// DataTable 转 Json   
        /// </summary>   
        /// <param name="dt">DataTable对象</param>  
        /// <returns></returns>  
        public static string ToJson(DataTable dt)
        {
            return Newtonsoft.Json.JsonConvert.SerializeObject(dt);
        }
        /// <summary>  
        /// DataTable 转 Json（异步方式）
        /// </summary>   
        /// <param name="dt">DataTable对象</param>  
        /// <returns></returns>  
        public static async Task<string> ToJsonAsync(DataTable dt)
        {
            return await Task.Run(() => Newtonsoft.Json.JsonConvert.SerializeObject(dt));
        }
        /// <summary>
        /// 序列化对象为Json字符串
        /// </summary>
        /// <param name="obj">要序列化的对象</param>
        /// <param name="recursionLimit">序列化对象的深度，默认为100</param>
        /// <returns>Json字符串</returns>
        public static string SerializeToJson(object obj, int recursionLimit = 100)
        {
            try
            {
                JavaScriptSerializer serialize = new JavaScriptSerializer();
                serialize.RecursionLimit = recursionLimit;
                return serialize.Serialize(obj);
            }
            catch
            {
                return "";
            }
        }

        #endregion

        #region 帮助方法
        /// <summary>  
        /// 过滤特殊字符  
        /// </summary>  
        /// <param name="s">字符串</param>  
        /// <returns></returns>  
        public static string String2Json(string s)
        {
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < s.Length; i++)
            {
                char c = s.ToCharArray()[i];
                switch (c)
                {
                    case '\"':
                        sb.Append("\\\""); break;
                    case '\\':
                        sb.Append("\\\\"); break;
                    case '/':
                        sb.Append("\\/"); break;
                    case '\b':
                        sb.Append("\\b"); break;
                    case '\f':
                        sb.Append("\\f"); break;
                    case '\n':
                        sb.Append("\\n"); break;
                    case '\r':
                        sb.Append("\\r"); break;
                    case '\t':
                        sb.Append("\\t"); break;
                    case '\v':
                        sb.Append("\\v"); break;
                    case '\0':
                        sb.Append("\\0"); break;
                    default:
                        sb.Append(c); break;
                }
            }
            return sb.ToString();
        }

        #endregion
    }
}
