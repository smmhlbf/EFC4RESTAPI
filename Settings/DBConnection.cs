namespace EFC4RESTAPI.Settings
{
    public class DBConnection
    {
        public string Host { get; set; }
        public int Port { get; set; }
        public string User { get; set; }
        public string Password { get; set; }
        public string Base { get; set; }
        public string Charset { get; set; }
        public string ConnString { get => $"server={Host};port={Port};database={Base};uid={User};pwd={Password};charset={Charset}"; }
    }
}