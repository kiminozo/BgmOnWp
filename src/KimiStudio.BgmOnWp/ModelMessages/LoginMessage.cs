namespace KimiStudio.BgmOnWp.ModelMessages
{
    using Models;


    public class LoginMessage : Message
    {
        public AuthUser AuthUser { get; set; }
    }
}
