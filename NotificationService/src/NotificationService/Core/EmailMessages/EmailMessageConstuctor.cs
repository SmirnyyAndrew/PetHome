namespace NotificationService.Core.EmailMessages;

public static class EmailMessageConstructor
{
    public static string Build(string styles, string body) =>
        @$"<html>
        <head>
           {styles}
        </head>
          <body>
            {body}
          </body>
        </html>";
}
