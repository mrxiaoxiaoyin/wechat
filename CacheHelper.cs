using System;
using System.Web;

namespace WeChat.WeChatTool
{
    public class CacheHelper
    {
        public static void CacheInsertAddMinutes(string cacheKey, object objObject, int minutes)
        {
            System.Web.Caching.Cache objCache = HttpRuntime.Cache;
            objCache.Insert(cacheKey, objObject, null, DateTime.Now.AddMinutes(minutes), TimeSpan.Zero);
        }

        public static object CacheValue(string cacheKey)
        {
            System.Web.Caching.Cache objCache = HttpRuntime.Cache;
            return objCache[cacheKey];
        }
    }
}