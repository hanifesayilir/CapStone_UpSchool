namespace Application.Common.Helpers
{
    public static class MessagesHelper
    {
        public static class Email
        {
            public static class NotificationActivation
            {
                public static string Subject => "Notification Message After Crawling Ends";
                public static string ActivationMessage => "The crawl is successfully completed.";

                public static string Name(string firstName) =>$"Hi {firstName}";

               public static string ButtonText => "Activate My Account";

                public static string Buttonlink(string email, string emailToken) =>
                    $"https://upstorage.app/account/account-activation?email={email}&token={emailToken}";
    

            }

            public static class NotificationCrawler
            {
                public static string Subject => "Notification Message After Crawling Ends";
                public static string Message => "Crawling is done successfully and your orderNumber is ";

                public static string Name(string firstName) => $"Hi {firstName}";

                public static string OrderNumber(int OrderNumber) => $"{OrderNumber.ToString()}";

            }


             public static class Confirmation
        {
            public static string Subject => "Thank you for signing up to Final Crawler Project!";
            public static string ActivationMessage => "Thank you for signing up to Final Crawler Project! In order to activate your account please click the activation button given below.";
            
            public static string Name(string firstName) =>
                $"Hi {firstName}";
            
            public static string ButtonText => "Activate My Account";

            public static string Buttonlink(string email, string emailToken) =>
                $"https://upstorage.app/account/account-activation?email={email}&token={emailToken}";
            
            /*public static string Buttonlink(string email, string emailToken)
            {
                return $"https://upstorage.app/account/account-activation?email={email}&token={emailToken}";
            }*/
                
        }
        }

    }
}
