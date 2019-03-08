namespace RedisCache.Redis
{
    public class RedisConfigItem
    {
        public string[] Hosts { get; set; }
        public string Password { get; set; }

        public RedisConfigItem()
        {
            this.Hosts = new string[0];
            this.Password = string.Empty;
        }
    }
}