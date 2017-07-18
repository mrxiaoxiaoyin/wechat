using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using WeChat.Model;

namespace WeChat.WeChatTool
{
    public class CommonController
    {

        /// <summary>
        /// 获取access_token
        /// </summary>
        /// <returns></returns>
        public static string GetAccessToken()
        {
            if (CacheHelper.CacheValue("access_token") == null)
            {
                return CacheHelper.CacheValue("access_token").ToString(); ;
            }
            string url = "https://api.weixin.qq.com/cgi-bin/token?grant_type=client_credential&appid={0}&secret={1}";
            url = string.Format(url, WeChatConfigInfo.AppId, WeChatConfigInfo.AppSecret);
            string rst = HttpCrossDomain.Get(url);
            if (rst.Contains("access_token"))
            {
                string[] temps = rst.Split(',');
                string tokenId = temps[0].Replace("{\"access_token\":\"", "").Replace("\"", "");
                CacheHelper.CacheInsertAddMinutes("access_token", tokenId, 100);
                return tokenId;
            }
            else
            {
                return rst;
            }
        }

        /// <summary>
        /// 获取微信用户OpenId
        /// </summary>
        /// <param name="code">微信code参数</param>
        /// <returns></returns>
        public static string GetOpenId(string code)
        {
            if (CacheHelper.CacheValue("access_token") == null)
            {
                GetAccessToken();
            }
            string url = "https://api.weixin.qq.com/sns/oauth2/access_token?appid={0}&secret={1}&code={2}&grant_type=authorization_code";
            url = string.Format(url, WeChatConfigInfo.AppId, WeChatConfigInfo.AppSecret, code);
            string rst = HttpCrossDomain.Get(url);
            if (rst.Contains("openid"))
            {
                string[] temps = rst.Split(',');
                string openid = temps[3].Replace("\"openid\":\"", "").Replace("\"", "");
                return openid;
            }
            else
            {
                return "";
            }
        }

        /// <summary>
        /// 获取用户基本信息
        /// </summary>
        /// <param name="accessToken">调用接口凭证</param>
        /// <param name="openId">普通用户的标识，对当前公众号唯一</param>
        /// <param name="lang">返回国家地区语言版本，zh_CN 简体，zh_TW 繁体，en 英语</param>
        public string GetUserInfo(string openId)
        {
            string access_token = "";
            if (CacheHelper.CacheValue("access_token") == null)
            {
                GetAccessToken();
            }
            access_token = CacheHelper.CacheValue("access_token").ToString();
            string url = string.Format("https://api.weixin.qq.com/cgi-bin/user/info?access_token={0}&openid={1}&lang={2}", access_token, openId, "zh_CN");
            return HttpCrossDomain.Get(url);
        }
        /// <summary>
        /// 返回随机字符串
        /// </summary>
        /// <returns></returns>
        public static string GenerateNonceStr()
        {
            return Guid.NewGuid().ToString().Replace("-", "");
        }
        /// <summary>
        /// 获取jsapi_ticket
        /// </summary>
        /// <returns></returns>
        public static string GetJsapiTicket()
        {
            JObject json = (JObject)JsonConvert.DeserializeObject(HttpCrossDomain.Get("https://api.weixin.qq.com/cgi-bin/ticket/getticket?access_token=" + GetAccessToken().ToString() + "&type=jsapi"));
            return (string)json["ticket"];
        }

    }
}