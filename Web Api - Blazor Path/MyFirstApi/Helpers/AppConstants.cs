namespace MyFirstApi.Helpers
{
    public static class AppConstants
    {
        
        public const int JwtTokenExpiration = 720;
        public const string Audience = "MyFirstApi/Audience";
        public const string Issuer = "MyFirstApi/Issuer";
        public const string JwtScheme = nameof(JwtScheme);
    }
}