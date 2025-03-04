namespace NotificationService.Core.EmailMessages.Templates;

public class ConfirmationEmailMessage
{
    public static string Body(string userName, string confirmationUrl) => 
            @$"<div class='container'>
                <h2>Здравствуйте, {userName}!</h2>
                <p>Благодарим Вас за регистрацию на нашем сайте. Для того чтобы завершить процесс регистрации и подтвердить Ваш email, пожалуйста, перейдите по следующей ссылке:</p>
                <a href='{confirmationUrl}' class='button'>Подтвердить почту</a>
                <p>Если Вы не регистрировались на нашем сайте, просто проигнорируйте это письмо.</p>
            </div>
            <div class='footer'>
                <p>PetHome, {DateTime.Now.Year}. Все права защищены.</p>
            </div>"; 

    public static string Styles() =>
        @"<style>
        body {
            font-family: Arial, sans-serif;
            color: #333;
            background: linear-gradient(to bottom, #a7c7e7, #82aaff);
            margin: 0;
            padding: 0;
            display: flex;
            justify-content: center;
            align-items: center;
            height: 100vh;
        }

        .container {
            background-color: #ffffff;
            padding: 40px;
            border-radius: 8px;
            box-shadow: 0 4px 6px rgba(0, 0, 0, 0.1);
            width: 100%;
            max-width: 600px;
            text-align: center;
        }

        h2 {
            color: #1a73e8;
            font-size: 24px;
            margin-bottom: 20px;
        }

        p {
            font-size: 16px;
            line-height: 1.6;
            color: #333;
            margin-bottom: 20px;
        }

        .button {
            background-color: #1a73e8;
            color: white;
            padding: 12px 25px;
            text-decoration: none;
            border-radius: 5px;
            font-size: 16px;
            display: inline-block;
            margin-top: 20px;
            transition: background-color 0.3s ease;
        }

        .button:hover {
            background-color: #0c58b3;
        }

        .footer {
            font-size: 12px;
            color: #888;
            margin-top: 30px;
            text-align: center;
        }
    </style>";

    public static string Subject() => "Подтверждение почты";
}
