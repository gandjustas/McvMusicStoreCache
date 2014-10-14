using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Caching;
using System.Web.Helpers;
using StackExchange.Redis;

namespace MvcMusicStore.Infrastructure
{
public static class Redis
{
    public static readonly ConnectionMultiplexer Client = ConnectionMultiplexer.Connect("localhost");

    public static CacheDependency CreateDependency(string key)
    {
        return new RedisCacheDependency(key);
    }

    public static T GetCached<T>(string key, Func<T> getter) where T:class 
    {
        var localCache = HttpRuntime.Cache;
        var result = (T) localCache.Get(key);
        if (result != null) return result;

        var redisDb = Client.GetDatabase();

        var value = redisDb.StringGet(key);
        if (!value.IsNullOrEmpty)
        {
            result = Json.Decode<T>(value);
            localCache.Insert(key, result, CreateDependency(key));
            return result;
        }

        result = getter();

        redisDb.StringSet(key, Json.Encode(result));
        localCache.Insert(key, result, CreateDependency(key));
        return result;
    }

    public static void DeleteKey(string key)
    {
        HttpRuntime.Cache.Remove(key);
        var redisDb = Client.GetDatabase();
        redisDb.KeyDelete(key);
    }


    private class RedisCacheDependency: CacheDependency
    {
        public RedisCacheDependency(string key):base()
        {
            Client.GetSubscriber().Subscribe("__keyspace@0__:" + key, (c, v) =>
            {
                this.NotifyDependencyChanged(new object(), EventArgs.Empty );                                        
            });
        }
    }
}
}