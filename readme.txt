Server1, Server2 - робочі сервери, на які я відправляю запити (Asp.Net Core WebApi)
WebTask2 - основний сервер (Asp.Net Core MVC)

Запуск проекту потрібно зробити з Visual Studio
- для солюшину налаштувати Multiple Startup (Server1 - Start without debugging, Server1 - Start without debugging, WebTask2 - Start)
- в налаштуваннях проекту потрібно вибрати IIS Express
- для роботи проекту потрібно налаштувати локальну бд, для цього на комп'ютері повинен бути встановлений ms sql server, в Connection String потрібно додати адресу локальної бд

Для реєстрації і авторизації я використовував ASP.Net Core Identity

Api endpoints:
main/ - виконання головного завдання і повернення результату
main/busy - повернення статусу сервера
main/progress - повернення прогресу виконання завдання
